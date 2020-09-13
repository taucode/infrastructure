using System;

namespace TauCode.Infrastructure.Time
{
    public class ConstTimeProvider : ITimeProvider
    {
        private readonly DateTimeOffset _moment;

        public ConstTimeProvider(DateTimeOffset moment)
        {
            _moment = moment;
        }

        public DateTimeOffset GetCurrent() => _moment;
    }
}
