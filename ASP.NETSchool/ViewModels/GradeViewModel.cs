using ASP.NETSchool.Models;

namespace ASP.NETSchool.ViewModels
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int Mark { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public int StudentId { get; set; }
    }
}
