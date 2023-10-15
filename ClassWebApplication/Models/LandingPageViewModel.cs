namespace ClassWebApplication.Models
{
    public class LandingPageViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Lector> Lectors { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
