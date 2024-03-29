﻿using System.Globalization;

SectionTitle("Specifying date and time values");
WriteLine($"DateTime.MinValue: {DateTime.MinValue}");
WriteLine($"DateTime.MaxValue: {DateTime.MaxValue}");
WriteLine($"DateTime.UnixEpoch: {DateTime.UnixEpoch}");
WriteLine($"DateTime.Now: {DateTime.Now}");
WriteLine($"DateTime.Today: {DateTime.Today}");

WriteLine("Christmas day: ");

DateTime xmas = new(year: 2024, month: 12, day: 25);
WriteLine($"Christmas (default format): {xmas}");
WriteLine($"Christmas (custom format): {xmas:dddd, dd MMMM, yyyy}");
WriteLine($"Christmas is in month {xmas.Month} of the year.");
WriteLine($"Christmas is day {xmas.DayOfYear} of the year 2024.");
WriteLine($"Christmas {xmas.Year} is on a {xmas.DayOfWeek}.");

SectionTitle("Date and time calculation");
DateTime beforeXmas = xmas.Subtract(TimeSpan.FromDays(12));
DateTime afterXmas = xmas.AddDays(12);
WriteLine($"12 days before Christmas: {beforeXmas:d}");
WriteLine($"12 after before Christmas: {afterXmas:d}");
TimeSpan untilXmas = xmas - DateTime.Now;
WriteLine($"Now: {DateTime.Now}");
WriteLine("There are {0} days and {1} hours until Christmas 2024.", 
    arg0: untilXmas.Days, arg1: untilXmas.Hours);
WriteLine("There are {0:N0} hours until Christmas 2024.", 
    arg0: untilXmas.TotalHours);

WriteLine("Date kids wake up to presents...");
DateTime kidsWakeUp = new(year: 2024, month: 12, day: 25, hour: 00, minute: 00, second: 0);
WriteLine($"Kids wake up: {kidsWakeUp}");
WriteLine("The kids woke me up at {0}", arg0: kidsWakeUp.ToShortTimeString());

SectionTitle("Milli-, micro-, and nanoseconds");
DateTime preciseTime = new(year: 2022, month: 11, day: 8,
    hour: 12, minute: 0, second: 0, millisecond: 6, microsecond: 999);
WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
    preciseTime.Millisecond, preciseTime.Microsecond, preciseTime.Nanosecond);
preciseTime = DateTime.UtcNow;
WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
    preciseTime.Millisecond, preciseTime.Microsecond, preciseTime.Nanosecond);

SectionTitle("Globalization with dates and times");
WriteLine($"Current culture is: {CultureInfo.CurrentCulture.Name}");
string textDate = "4 July 2024";
DateTime independenceDay = DateTime.Parse(textDate);
WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");
textDate = "7/4/2024";
independenceDay = DateTime.Parse(textDate);
WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");
independenceDay = 
    DateTime.Parse(textDate, provider: CultureInfo.GetCultureInfo("en-US"));
WriteLine($"Text: {textDate}, DateTime {independenceDay:d MMMM}");

SectionTitle("Leap year 2022 to 2036");
for (int year = 2022; year <= 2036; year++)
{
    Write($"{year} is a leap year: {DateTime.IsLeapYear(year)}.");
    WriteLine("There are {0} days in February {1}.", 
        arg0: DateTime.DaysInMonth(year: year, month: 2), arg1: year);
}

WriteLine("Is Christmas daylight saving time? {0}", 
    arg0: xmas.IsDaylightSavingTime());
WriteLine("Is July 4th daylight saving time? {0}", 
    arg0: independenceDay.IsDaylightSavingTime());

SectionTitle("Localizing the DayOfWeek enum");
CultureInfo previousCulture = Thread.CurrentThread.CurrentCulture;
Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-DK");
WriteLine("Culture: {0}, DayOfWeek: {1}", 
    Thread.CurrentThread.CurrentCulture.NativeName, 
    DateTimeFormatInfo.CurrentInfo.GetDayName(DateTime.Now.DayOfWeek));
Thread.CurrentThread.CurrentCulture = previousCulture;

SectionTitle("Working with only a date or a time");
DateOnly coronation = new(year: 2023, month: 5, day: 6);
WriteLine($"The King's Coronation is on {coronation.ToLongDateString()}.");
TimeOnly starts = new(hour: 11, minute: 30);
WriteLine($"The King's Coronation starts at {starts}");
DateTime calenderEntry = coronation.ToDateTime(starts);
WriteLine($"Add to your calender: {calenderEntry}.");
