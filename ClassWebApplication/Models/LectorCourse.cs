namespace ClassWebApplication.Models
{
    public class LectorCourse
    {
        public int LectorId { get; set; }
        public Lector Lector { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
