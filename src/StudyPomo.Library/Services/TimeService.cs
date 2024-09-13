using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;
using TimeZoneNames;

namespace StudyPomo.Library.Services;

public static class TimeService
{
    public static IDictionary<string, string> GetTimeZones()
    {
        var tzs = TZNames.GetDisplayNames("en-US");
        return tzs;
    }

    public static TimeZoneInfo GetTimeZoneInfo(string? ianaTimeZoneId)
    {
        if (ianaTimeZoneId == null)
        {
            return TimeZoneInfo.Utc;
        }

        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ianaTimeZoneId);

        return tzi;
    }

    public static DateTime ConvertToUserTime(DateTime utcTime, TimeZoneInfo userTimeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, userTimeZoneInfo);
    }

    public static DateTime ConvertToUserTime(DateTime utcTime, string timeZoneId)
    {
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
    }

    public static DateTime ConvertFromUserTime(DateTime userTime, TimeZoneInfo userTimeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeToUtc(userTime, userTimeZoneInfo);
    }

}
