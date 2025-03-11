//class to map against the data to be read into the app
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace CalendarMaker
{
    public class SchoolTerm
    {
        [Name("Start")]
        public required string StartDateString { get; set; }

        [Name("End")]
        public required string EndDateString { get; set; }

        [Name("Full Module")]
        public required string FullModule { get; set; }

        [Name("Academic Year")]
        public required string AcademicYear { get; set; }

        [Name("Module")]
        public required string Module { get; set; }

        public bool IsHoliday { get; set; }
        public DateOnly Start => ParseDate(StartDateString);
        public DateOnly End => ParseDate(EndDateString);

        private static DateOnly ParseDate(string date)
        {
            if (DateOnly.TryParseExact(date, ["d/M/yyyy", "dd/MM/yyyy"], CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }
            throw new FormatException($"Invalid date format: '{date}'");
        }     

    }
}
