
namespace CalendarMaker
{
    public class AcademicDay
    {
        public DateOnly Date { get; set; }
        public required string WeekdayName { get; set; }
        public required string WeekDayNameShort { get; set; }
        public int WeekDayIndex { get; set; }
        public int WeekNumber { get; set; }
        public required string MonthName { get; set; }
        public required string MonthNameShort { get; set; }
        public int MonthIndex { get; set; }
        public required string Module { get; set; }
        public required string AcademicYear { get; set; }
        public required string AcademicYearShort { get; set; }
        public int AcademicYearStart { get; set; }
        public int AcademicYearEnd { get; set; }
        public required string FullModuleName { get; set; }
        public int ModuleIndex { get; set; }
       

        

        




    }
}
