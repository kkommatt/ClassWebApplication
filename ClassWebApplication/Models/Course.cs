using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassWebApplication.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name="Назва")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Матеріали")]
        [Url(ErrorMessage = "Неправильний формат")]
        public string MaterialLink { get; set; }

        public string MaterialFileIdentifier { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
        public ICollection<LectorCourse> LectorCourses { get; set; }
    }
}
