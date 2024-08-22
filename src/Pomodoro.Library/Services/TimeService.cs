using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;
using TimeZoneNames;

namespace Pomodoro.Library.Services;

public static class TimeService
{
    public static IDictionary<string, string> GetIanaTimeZones()
    {
        var tzs = TZNames.GetDisplayNames("en-US", useIanaZoneIds: true);
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

    public static DateTime ConvertToUserTime(DateTime utcTime,  TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
    }

    public static DateTime ConvertToUserTime(DateTime utcTime, string timeZoneId)
    {
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
    }

}
