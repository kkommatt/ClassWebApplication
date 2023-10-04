using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassWebAPI.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name="Назва")]
        public string Title { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Матеріали")]
        [Url(ErrorMessage = "Неправильний формат")]
        public string MaterialLink { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<LectorCourse> LectorCourses { get; set; } = new List<LectorCourse>();
    }
}
