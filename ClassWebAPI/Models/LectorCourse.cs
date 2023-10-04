using System.Text.Json.Serialization;

namespace ClassWebAPI.Models
{
    public class LectorCourse
    {
        public int LectorId { get; set; }
        [JsonIgnore]
        public Lector Lector { get; set; } = new Lector();

        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; } = new Course();
    }
}
