﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkingWithCultures;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        services.AddTransient<ProjectResources>();
    }).Build();

OutputEncoding = System.Text.Encoding.Unicode;
OutputCultures("Current culture");

WriteLine("Example ISO culture codes:");
string[] cultureCodes = new[] {
  "da-DK", "en-GB", "en-US", "fa-IR",
  "fr-CA", "fr-FR", "he-IL", "pl-PL", "sl-SI" };
foreach (string code in cultureCodes)
{
    CultureInfo culture = CultureInfo.GetCultureInfo(code);
    WriteLine("  {0}: {1} / {2}",
      culture.Name, culture.EnglishName, culture.NativeName);
}

WriteLine();
Write("Enter an ISO culture code: ");
string? cultureCode = ReadLine();
if (string.IsNullOrWhiteSpace(cultureCode))
{
    cultureCode = "en-US";
}
CultureInfo ci;
try
{
    ci = CultureInfo.GetCultureInfo(cultureCode);
}
catch (CultureNotFoundException)
{
    WriteLine($"Culture code not found: {cultureCode}");
    WriteLine("Exiting the app.");
    return;
}
// change the current cultures on the thread
CultureInfo.CurrentCulture = ci;
CultureInfo.CurrentUICulture = ci;
OutputCultures("After changing the current culture");
ProjectResources resources =
    host.Services.GetRequiredService<ProjectResources>();
Write(resources.GetEnterYourNamePrompt());
string? name = ReadLine();
if (string.IsNullOrWhiteSpace(name))
{
    name = "Bob";
}
Write(resources.GetEnterYourDobPrompt());
string? dobText = ReadLine();
if (string.IsNullOrWhiteSpace(dobText))
{
    // if they do not enter a DOB then use
    // sensible defaults for their culture
    dobText = ci.Name switch
    {
        "en-US" or "fr-CA" => "1/27/1990",
        "da-DK" or "fr-FR" or "pl-PL" => "27/1/1990",
        "fa-IR" => "1990/1/27",
        _ => "1/27/1990"
    };
}
Write(resources.GetEnterYourSalaryPrompt());
string? salaryText = ReadLine();
if (string.IsNullOrWhiteSpace(salaryText))
{
    salaryText = "34500";
}
DateTime dob = DateTime.Parse(dobText);
int minutes = (int)DateTime.Today.Subtract(dob).TotalMinutes;
decimal salary = decimal.Parse(salaryText);
WriteLine(resources.GetPersonDetails(name, dob, minutes, salary));