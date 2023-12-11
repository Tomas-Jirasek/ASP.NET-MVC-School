using ASP.NETSchool.Models;

namespace ASP.NETSchool.ViewModels
{
    public class GradesDropdownsViewModel
    {
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }

        public GradesDropdownsViewModel()   // neni pouzit DI, proto inicializace listů
        {
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }
    }
}
