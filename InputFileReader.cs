
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace CalendarMaker
{
    public static class InputFileReader
    {
        public static List<SchoolTerm> ReadSchoolTerms()
        {
            var filePath = "\\tru-fs-stf01.latrust.org.uk\\TRUStaffData$\\adelle.king\\Desktop\\CalendarTableMaker";
            using var fileReader = new StreamReader(filePath);
            using var csv = new CsvReader(fileReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            { 
                HasHeaderRecord = true 
            });

            if (csv.Read())
            {
                csv.ReadHeader();
                var headers = csv.HeaderRecord;
            }

            return csv.GetRecords<SchoolTerm>().ToList();
        }        
    }
}

