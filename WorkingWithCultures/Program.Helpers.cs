partial class Program
{
   static void OutputCultures(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("*");
        WriteLine($"* {title}");
        WriteLine("*");

        CultureInfo globalisation = CultureInfo.CurrentCulture;
        CultureInfo localisation = CultureInfo.CurrentUICulture;
        WriteLine("Days of the week: {0}", string.Join(",", globalisation.DateTimeFormat.MonthNames));
        WriteLine("The current localization cultures is {0}: {1}", localisation.Name, localisation.DisplayName);
        WriteLine("Days of the week: {0}",
            string.Join(",", globalisation.DateTimeFormat.MonthNames
            .TakeWhile(month => !string.IsNullOrEmpty(month))));
        WriteLine("1st day of this year: {0}",
            new DateTime(year: DateTime.Today.Year, month: 1, day: 1)
            .ToString("D", globalisation));
        WriteLine("Number group seperator: {0}", globalisation.NumberFormat.NumberGroupSeparator);
        WriteLine("Number decimal seperator: {0}", globalisation.NumberFormat.NumberDecimalSeparator);
        RegionInfo region = new RegionInfo(globalisation.LCID);
        WriteLine("Currency symbol: {0}", region.CurrencySymbol);
        WriteLine("Currency name: {0} ({1})", 
            region.CurrencyNativeName, region.CurrencyEnglishName);
        WriteLine("IsMatric: {0}", region.IsMetric);
        WriteLine();
        ForegroundColor = previousColor;

    }
}

