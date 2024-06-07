namespace BotMarket2.Client.Shared
{
    public static class TimeStampExtension
    {
        public static long ToTimeStamp(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double totalMilliseconds = (utcDateTime - epoch).TotalMilliseconds;

            return (long)totalMilliseconds;
        }

    }
}
