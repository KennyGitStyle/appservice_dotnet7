using System.Collections.ObjectModel;

partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("*");
        WriteLine($"* {title}");
        WriteLine("*");
        ForegroundColor = previousColor;
    }

    static void OutputTimeZones()
    {
        ReadOnlyCollection<TimeZoneInfo> zones =
            TimeZoneInfo.GetSystemTimeZones();
        WriteLine("*");
        WriteLine($"* {zones.Count} time zones");
        WriteLine("*");

        foreach (TimeZoneInfo zone in zones.OrderBy(z => z.Id))
        {
            WriteLine($"{zone.Id}");
        }
    }

    static void OutputDateTime(DateTime dateTime, string title) 
    {
        SectionTitle(title);
        WriteLine($"Value: {dateTime}");
        WriteLine($"Kind: {dateTime.Kind}");
        WriteLine($"IsDayLightSavingTime: {dateTime.IsDaylightSavingTime()}");
        WriteLine($"ToLocalTime(): {dateTime.ToLongDateString()}");
        WriteLine($"ToUniversalTime(): {dateTime.ToUniversalTime()}");
    }

    static void OutputTimeZone(TimeZoneInfo zone, string title)
    {
        SectionTitle(title);
        WriteLine($"Id: {zone.Id}");
        WriteLine("IsDaylightSavingTime(DateTime.Now): {0}", 
            zone.IsDaylightSavingTime(DateTime.Now));
        WriteLine($"StandardName: {zone.StandardName}");
        WriteLine($"DaylightName: {zone.DisplayName}");
        WriteLine($"BaseUtcOffset: {zone.BaseUtcOffset}");
    }

    static string GetCurrentZoneName(TimeZoneInfo zone, DateTime when)
    {
        return zone.IsDaylightSavingTime(when) 
            ? zone.DaylightName 
            : zone.StandardName;
    }
}

