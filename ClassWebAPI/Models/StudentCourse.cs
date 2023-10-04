using System.Text.Json.Serialization;

namespace ClassWebAPI.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        [JsonIgnore]
        public Student Student { get; set; } = new Student();

        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; } = new Course();
    }
}
