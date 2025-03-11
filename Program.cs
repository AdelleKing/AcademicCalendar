
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;



namespace CalendarMaker
{
    public class Program
    {
        static List <SchoolTerm> termDates = InputFileReader.ReadSchoolTerms();
        static void Main(string[] args)
        {
            DateOnly startDate = termDates[0].Start;
            DateOnly endDate = termDates[termDates.Count - 1].End;
            
            int numberOfWeeks = 1;
            var weekStartDate = "";
            var constantMonth = 1;


            var thisTerm = currentTerm(startDate);
         


            DataTable table = new DataTable("Calendar");

            // Define columns
            table.Columns.Add("Date");
            table.Columns.Add("WeekDay");
            table.Columns.Add("WeekDayNameShort");
            table.Columns.Add("WeekDayIndex");
            table.Columns.Add("WeekNumber");
            table.Columns.Add("MonthName");
            table.Columns.Add("MonthNameShort");
            table.Columns.Add("MonthIndex");
            table.Columns.Add("Module");
            table.Columns.Add("AcademicYear");
            table.Columns.Add("AcademicYearShort");
            table.Columns.Add("AcademicYearStart");
            table.Columns.Add("AcademicYearEnd");
            table.Columns.Add("FullModuleName");
            table.Columns.Add("ModuleIndex");
            table.Columns.Add("Week_Start_Date");
            table.Columns.Add("DFE_Term");
            table.Columns.Add("Full_WeekName");
            table.Columns.Add("ConstantMonthCount");


            for (DateOnly loopDay = startDate; loopDay < endDate; loopDay = loopDay.AddDays(1))
            {

                if (loopDay > thisTerm.End)
                {
                    thisTerm = currentTerm(loopDay);
                }


                //MonthIndex
                int monthIndex = 0;

                if (loopDay.Month > 8 && loopDay.Month < 13)
                {
                    monthIndex = loopDay.Month - 8;
                }
                else
                {
                    monthIndex = loopDay.Month + 4;
                }


                //running week 
                if (loopDay.DayOfWeek == 0 && thisTerm.FullModule != "0")
                {
                    numberOfWeeks++;
                }



                // Short Year
                var shortYear = thisTerm.AcademicYear.Split("-")[0].ToString().Substring(2) + "/" + thisTerm.AcademicYear.Split("-")[1];

                //Academic Year End
                var academicyear = thisTerm.AcademicYear.Split(' ')[0];

                string[] parts = academicyear.Split('-');

                string yearEnd;

                if (thisTerm.FullModule == "0" || parts.Length != 2)
                {

                    yearEnd = "0";
                }
                else
                {

                    yearEnd = "20" + parts[1];
                }




                
                

                foreach (var day in loopDay.ToString())
                {
                    if (loopDay.DayOfWeek.ToString() == "Monday")
                    {
                        weekStartDate = loopDay.ToString();
                      
                    }                   

                }

                


                //DFE Term
                var dfeTerm = " ";
                var termModule = Int32.Parse(thisTerm.Module.Split(" ")[1]);

                if (termModule < 3)
                {
                    dfeTerm = "Autumn Term";
                }
                else if (termModule >= 2 && termModule < 5)
                {
                    dfeTerm = "Spring Term";
                }
                else if (termModule >= 5 && termModule < 7)
                {
                    dfeTerm = "Summer Term";
                }

                // Constant Month Count                

                var splitDate= loopDay.ToString().Split("/");
                var firstDay = splitDate[0];

                
                    if (firstDay == "01")
                    {                    
                        constantMonth++; 
                    }

                  

                //CREATE THE TABLE ROWS

                DataRow row = table.NewRow();

                row["Date"] = loopDay;
                row["WeekDay"] = loopDay.DayOfWeek;
                row["WeekDayNameShort"] = loopDay.DayOfWeek.ToString().Substring(0, 3);
                row["WeekDayindex"] = (int)loopDay.DayOfWeek;
                if (!thisTerm.IsHoliday)
                {
                    row["WeekNumber"] = numberOfWeeks;
                }
                else
                {
                    row["WeekNumber"] = "0";
                }                 
               
                row["MonthName"] = loopDay.ToDateTime(TimeOnly.MinValue).ToString("MMMM");
                row["MonthNameShort"] = loopDay.ToDateTime(TimeOnly.MinValue).ToString("MMM");
                row["MonthIndex"] = monthIndex;
                row["Module"] = thisTerm.Module;
                row["AcademicYear"] = thisTerm.AcademicYear;

                if (thisTerm.IsHoliday)
                {

                    row["AcademicYearShort"] = "0";
                }
                else
                {
                    row["AcademicYearShort"] = shortYear;
                }
                                
                row["AcademicYearStart"] = thisTerm.AcademicYear.Split("-")[0];
                if (thisTerm.IsHoliday)
                {

                    row["AcademicYearEnd"] = "0";
                }
                else
                {
                    row["AcademicYearEnd"] = "20" + thisTerm.AcademicYear.Split("-")[1];
                }
                
                row["FullModuleName"] = thisTerm.AcademicYear + thisTerm.Module;
                row["ModuleIndex"] = thisTerm.Module.Split(" ")[1];
                row["Week_Start_Date"] = weekStartDate;
                row["DFE_Term"] = dfeTerm;
                row["Full_WeekName"] = thisTerm.Module + " - " + weekStartDate;
                row["ConstantMonthCount"] = constantMonth;


                table.Rows.Add(row);

                
            }
            string download = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            CsvExporter.ExportToCsv(table,Path.Combine(download, "AcademicCalendar.csv"));

        }
        

        public static class CsvExporter
        {
            public static void ExportToCsv(DataTable table, string filePath)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Write the header
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            writer.Write(table.Columns[i]);
                            if (i < table.Columns.Count - 1)
                            {
                                writer.Write(",");
                            }
                        }

                        writer.WriteLine();

                        // Write the data
                        foreach (DataRow row in table.Rows)
                        {
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                writer.Write(row[i].ToString());
                                if (i < table.Columns.Count - 1)
                                {
                                    writer.Write(",");
                                }
                            }

                            writer.WriteLine();
                        }
                    }

                    Console.WriteLine("CSV file created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }





        private static SchoolTerm currentTerm(DateOnly startDate)
        {
            for (int i = 0; i < termDates.Count; i++)
            {
                DateOnly TermEnd;
                if (i + 1 > termDates.Count -1)
                {
                    TermEnd = termDates[termDates.Count - 1].End;
                }
                else
                {
                    TermEnd = termDates[i + 1].Start.AddDays(-1);
                }

                               

                if (termDates[i].Start <= startDate && termDates[i].End >= startDate)
                {
                    termDates[i].IsHoliday = false;
                    return termDates[i];
                }

                if (termDates[i].Start <= startDate && TermEnd >= startDate)
                    return new SchoolTerm
                    {
                        StartDateString = termDates[i].Start.ToString(),
                        EndDateString = TermEnd.ToString(),
                        FullModule = "0",
                        AcademicYear = termDates[i].AcademicYear,
                        Module = termDates[i].Module,
                        IsHoliday = true,
                    };
            }


            return new SchoolTerm
            {
                StartDateString = startDate.ToString(),
                EndDateString = startDate.ToString(),
                FullModule = "0",
                AcademicYear = termDates[0].AcademicYear,
                Module = "0",
                IsHoliday = true,
            };
        }

    }
}
