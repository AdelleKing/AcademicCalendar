
namespace CalendarMaker
{
    public class SchoolTermValidator : ISchoolTermValidator
    {
        public bool Validate(SchoolTerm schoolTerm)
        {
            //check that the start date is not greater than the end date
            //It needs to be a valid Full Module
            //It needs to be a valid Short Academic Year
            //It needs to be a valid Module 
            return true;
        }

        
        private static readonly HashSet<string> ValidModules = new HashSet<string>{ "some value1", "some value2" };
    }
}
