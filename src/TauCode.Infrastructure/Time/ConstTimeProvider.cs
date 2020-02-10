using System;

namespace TauCode.Infrastructure
{
    public class ConstTimeProvider : ITimeProvider
    {
        private readonly DateTime _dateTime;

        public ConstTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public ConstTimeProvider()
            : this(DateTime.UtcNow)
        {

        }

        public DateTime GetCurrent() => _dateTime;
    }
}
