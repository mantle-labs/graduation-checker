namespace GraduationChecker.Models
{
    public class Graduation
    {
        public string PermanentCode { get; set; }
        public University University { get; set; }
        public Program Program { get; set; }
        public string GraduationYear { get; set; }
        public decimal Gpa { set; get; }

        public override string ToString()
        {
            return $"{PermanentCode}\r\n\r\n{University.ShortName}\r\n\r\n{Program.Degree}\r\n\r\n{Program.ShortName}\r\n\r\n{Gpa}\r\n\r\n{GraduationYear}";
        }
    }

    public class GraduationRequest
    {
        public string PermanentCode { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string GraduationYear { get; set; }
        public string Program { get; set; }
        public decimal Gpa { set; get; }
    }
}
