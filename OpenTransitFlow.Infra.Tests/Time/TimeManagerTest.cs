using NUnit.Framework;
using OpenTransitFlow.Infra.Time;

namespace OpenTransitFlow.Infra.Tests.Time
{
    internal class TimeManagerTests
    {
        [TestCase(DayOfWeek.Monday, 0UL)]
        [TestCase(DayOfWeek.Tuesday, (24L * 60 * 60 * 1000)*1UL)]
        public void DayOfWeekTests(DayOfWeek day, ulong timeTiks)
        {
            Assert.That(TimeManager.MillisecondsToDayOfWeek(timeTiks), Is.EqualTo(day));
        }
    }
}