using NLog;
using System;
using System.Web.Http.Tracing;
using FCP.Util;
using System.Reflection;
using System.Diagnostics;

namespace FCP.Web.Api.Tracing
{
    /// <summary>
    /// Trace Writer By NLog
    /// </summary>
    public class NLogTraceWriter : FCPTraceWriter
    {
        private static readonly Assembly systemAssembly = typeof(Trace).Assembly;

        #region TraceLevel Map LogLevel
        private static readonly LogLevel[] traceLevelToLogLevelMap = new LogLevel[]
        {
            // TraceLevel.Off
            LogLevel.Off,

            // TraceLevel.Debug
            LogLevel.Debug,

            // TraceLevel.Info
            LogLevel.Info,

            // TraceLevel.Warn
            LogLevel.Warn,

            // TraceLevel.Error
            LogLevel.Error,

            // TraceLevel.Fatal
            LogLevel.Fatal
        };
        #endregion

        #region LoggerName
        protected virtual void SetLoggerName(LogEventInfo ev, string category)
        {
            if (!category.isNullOrEmpty())
            {
                ev.LoggerName = category;
                return;
            }

            SetDefaultLoggerName(ev);
        }

        protected virtual void SetDefaultLoggerName(LogEventInfo ev)
        {
            var stack = new StackTrace();
            int userFrameIndex = -1;
            MethodBase userMethod = null;

            for (int i = 0; i < stack.FrameCount; ++i)
            {
                var frame = stack.GetFrame(i);
                var method = frame.GetMethod();

                if (method.DeclaringType == this.GetType())
                {
                    // skip all methods of this type
                    continue;
                }

                if (method.DeclaringType.Assembly == systemAssembly)
                {
                    // skip all methods from System.dll
                    continue;
                }

                userFrameIndex = i;
                userMethod = method;
                break;
            }

            if (userFrameIndex >= 0)
            {
                ev.SetStackTrace(stack, userFrameIndex);
                if (userMethod.DeclaringType != null)
                {
                    ev.LoggerName = userMethod.DeclaringType.FullName;
                }
            }

            ev.LoggerName = ev.LoggerName ?? string.Empty;
        }
        #endregion

        protected override void TraceTraceRecord(TraceRecord traceRecord)
        {
            var ev = FormatLogEventInfo(traceRecord);

            ILogger logger = LogManager.GetLogger(ev.LoggerName);
            logger.Log(ev);
        }

        protected virtual LogEventInfo FormatLogEventInfo(TraceRecord traceRecord)
        {
            if (traceRecord == null)
                throw new ArgumentNullException(nameof(traceRecord));

            var ev = new LogEventInfo();

            ev.TimeStamp = traceRecord.Timestamp;            
            ev.Level = traceLevelToLogLevelMap[(int)traceRecord.Level];
            ev.Exception = traceRecord.Exception;
            ev.Message = FormatMessage(traceRecord);

            SetLoggerName(ev, traceRecord.Category);

            foreach (var property in traceRecord.Properties)
                ev.Properties.Add(property.Key, property.Value);

            return ev;
        }
    }
}
