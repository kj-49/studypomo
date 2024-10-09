using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data.Database;

public class SlowQueryDetectionHelper : DbCommandInterceptor
{

    private const int slowQueryThresholdInMilliSecond = 3000;
    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        if (eventData.Duration.TotalMilliseconds > slowQueryThresholdInMilliSecond)
        {
            Log.Warning($"Slow Query Detected. {command.CommandText}  TotalMilliSeconds: {eventData.Duration.TotalMilliseconds}");
        }
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        if (eventData.Duration.TotalMilliseconds > slowQueryThresholdInMilliSecond)
        {
            Log.Warning($"Slow Query Detected. {command.CommandText}  TotalMilliSeconds: {eventData.Duration.TotalMilliseconds}");
        }
        return base.ReaderExecuted(command, eventData, result);
    }
}
