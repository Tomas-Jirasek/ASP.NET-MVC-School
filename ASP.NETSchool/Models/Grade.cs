namespace ASP.NETSchool.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int Mark { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public Student Student { get; set; }
    }
}
