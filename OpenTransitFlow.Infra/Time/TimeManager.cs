namespace OpenTransitFlow.Infra.Time
{
    /// <summary>
    /// Class that keeps track of time. 
    /// </summary>
    public static class TimeManager
    {
        /// <summary>
        /// Internal representation of current simulation time.
        /// Unit is milliseconds.
        /// </summary>
        private static ulong _time = 0L;

        /// <summary>
        /// Simulation tik size. Default is 1 second tik size.
        /// </summary>
        private static ushort speed = 1000;

        internal static DayOfWeek MillisecondsToDayOfWeek(ulong milliseconds)
        {
            const long MillisecondsPerDay = 24L * 60 * 60 * 1000;
            int dayIndex = (int)((milliseconds / MillisecondsPerDay) % 7);
            if (dayIndex < 0) dayIndex += 7; 
            int sundayBased = (dayIndex + 1) % 7;
            return (DayOfWeek)sundayBased;
        }

        /// <summary>
        /// Holds the current day of the simlation time.
        /// </summary>
        public static DayOfWeek DayOfWeek => MillisecondsToDayOfWeek(_time);
    }
}