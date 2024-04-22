namespace BlazorModel
{
    public static class Helper
    {
        public static string ConvertSecondsToDate(this double? seconds)
        {
            var data = seconds ?? 0;
            TimeSpan t = TimeSpan.FromSeconds(data);

            if (t.Days > 0)
                return t.ToString(@"d\d\,\ hh\:mm\:ss");
            return t.ToString(@"hh\:mm\:ss");
        }
    }
    public static class FormatDate
    {
        /// <summary>
        /// Định dạng thời gian dd/MM/yyyy
        /// </summary>
        public const string DateTime_103 = "dd/MM/yyyy";
        /// <summary>
        /// Định dạng thời gian MM/dd/yyyy
        /// </summary>
        public const string DateTime_101 = "MM/dd/yyyy";
        /// <summary>
        /// Định dạng thời gian dd-MM-yyyy HH:mm:ss
        /// </summary>
        public const string DateTime_121 = "dd-MM-yyyy HH:mm:ss";
        /// <summary>
        /// Định dạng thời gian dd/MM/yyyy HH:mm
        /// </summary>
        public const string DateTime_ddMMyyyyHHmm = "dd/MM/yyyy HH:mm";
        /// <summary>
        /// Định dạng thời gian dd/MM/yyyy HH:mm:ss
        /// </summary>
        public const string DateTime_ddMMyyyyHHmmss = "dd/MM/yyyy HH:mm:ss";
        /// <summary>
        /// Định dạng thời gian dd/MM/yyyy 00:00
        /// </summary>
        public const string DateTime_ddMMyyyyHHmm_FirstTime = "dd/MM/yyyy 00:00";
        /// <summary>
        /// Định dạng thời gian dd/MM/yyyy 23:59
        /// </summary>
        public const string DateTime_ddMMyyyyHHmm_LastTime = "dd/MM/yyyy 23:59";
    }
}
