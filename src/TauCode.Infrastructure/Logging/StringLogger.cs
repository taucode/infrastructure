﻿using Microsoft.Extensions.Logging;
using System;
using System.Text;
using TauCode.Infrastructure.Time;

namespace TauCode.Infrastructure.Logging
{
    public class StringLogger : ILogger
    {
        private readonly StringBuilder _stringBuilder;

        public StringLogger(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));
        }

        public StringLogger()
            : this(new StringBuilder())
        {
        }

        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var timeStamp = TimeProvider.GetCurrentTime();
            var timeStampString = timeStamp.ToString("yyyy-MM-dd HH:mm:ss+00:00");
            var message = formatter(state, exception);
            var exceptionString = exception == null ? "" : exception.StackTrace;

            var logRecord = $"[{timeStampString}] [{logLevel}] {message} {exceptionString}";
            _stringBuilder.AppendLine(logRecord);
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}