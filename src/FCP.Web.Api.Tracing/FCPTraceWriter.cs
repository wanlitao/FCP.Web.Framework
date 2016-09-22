using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Tracing;
using FCP.Util;

namespace FCP.Web.Api.Tracing
{
    public abstract class FCPTraceWriter : ITraceWriter
    {
        #region Const String
        private const string systemWebHttpRequestCategory = "System.Web.Http.Request";

        private const string traceLevelOutOfRangeErrorMsg = "The TraceLevel must be a value between TraceLevel.Off and TraceLevel.Fatal.";
        #endregion

        #region System Defined Categories
        protected static readonly Lazy<string[]> systemDefinedTraceCategories = new Lazy<string[]>(GetSystemDefinedTraceCategories);

        private static string[] GetSystemDefinedTraceCategories()
        {
            return typeof(TraceCategories).GetFields()
                .Select(m => m.GetValue(null).ToString()).ToArray();
        }
        #endregion

        #region Category Predicate
        protected virtual bool TraceCategoryPredicate(string category)
        {
            return !IsSystemDefinedTraceCategory(category);
        }

        protected static bool IsSystemDefinedTraceCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
                return false;

            return systemDefinedTraceCategories.Value.Contains(category, StringComparer.OrdinalIgnoreCase);
        }

        protected static bool IsSystemWebHttpRequestCategory(string category)
        {
            return String.Equals(category, systemWebHttpRequestCategory, StringComparison.Ordinal);
        }
        #endregion

        #region Trace Level
        private TraceLevel _minLevel = TraceLevel.Info;

        public TraceLevel MinimumLevel
        {
            get
            {
                return _minLevel;
            }
            set
            {
                if (value < TraceLevel.Off || value > TraceLevel.Fatal)
                    throw new ArgumentOutOfRangeException("value", value, traceLevelOutOfRangeErrorMsg);

                _minLevel = value;
            }
        }
        #endregion

        #region Message Format
        protected virtual string FormatMessage(TraceRecord traceRecord)
        {
            if (IsSystemWebHttpRequestCategory(traceRecord.Category))
                return FormatRequestEnvelope(traceRecord);

            if (traceRecord.Kind == TraceKind.Begin)
                return null;

            var messages = new List<string>();

            messages.Add($"[{FormatDateTime(traceRecord.Timestamp)}] Level={traceRecord.Level.ToString()}");

            if (!traceRecord.Category.isNullOrEmpty())
                messages.Add($"Categorty='{traceRecord.Category}'");

            if (!traceRecord.Message.isNullOrEmpty())
                messages.Add($"Message='{traceRecord.Message}'");

            if (traceRecord.Status != 0)
                messages.Add($"Status={(int)traceRecord.Status} ({traceRecord.Status.ToString()})");

            if (traceRecord.Exception != null)
                messages.Add($"Exception={traceRecord.Exception.ToString()}");

            return String.Join(", ", messages);
        }

        protected virtual string FormatRequestEnvelope(TraceRecord traceRecord)
        {
            var messages = new List<string>();

            messages.Add(traceRecord.Kind == TraceKind.Begin ? $"[{FormatDateTime(traceRecord.Timestamp)}] Request received"
                : $"[{FormatDateTime(traceRecord.Timestamp)}] Sending response");

            if (traceRecord.Status != 0)
                messages.Add($"Status={(int)traceRecord.Status} ({traceRecord.Status.ToString()})");

            if (traceRecord.Request != null)
            {
                messages.Add($"Method={traceRecord.Request.Method}");

                if (traceRecord.Request.RequestUri != null)
                    messages.Add($"Url={traceRecord.Request.RequestUri.ToString()}");
            }

            if (!traceRecord.Message.isNullOrEmpty())
                messages.Add($"Message='{traceRecord.Message}'");

            if (traceRecord.Exception != null)
                messages.Add($"Exception={traceRecord.Exception.ToString()}");

            return String.Join(", ", messages);
        }

        protected virtual string FormatDateTime(DateTime dateTime)
        {
            // The 'o' format is ISO 8601 for a round-trippable DateTime.
            // It is culture-invariant and can be parsed.
            return dateTime.ToString("o", CultureInfo.InvariantCulture);
        }
        #endregion

        public virtual void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (traceAction == null)
                throw new ArgumentNullException(nameof(traceAction));

            if (level < TraceLevel.Off || level > TraceLevel.Fatal)
                throw new ArgumentOutOfRangeException("level", level, traceLevelOutOfRangeErrorMsg);

            if (MinimumLevel == TraceLevel.Off || level < MinimumLevel)
                return;

            if (!TraceCategoryPredicate(category))
                return;

            TraceRecord traceRecord = new TraceRecord(request, category, level);
            traceAction(traceRecord);

            TraceTraceRecord(traceRecord);
        }

        protected abstract void TraceTraceRecord(TraceRecord traceRecord);
    }
}
