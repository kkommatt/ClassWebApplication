using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassWebAPI.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "ПІБ")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Неправильний формат")]
        [Display(Name = "Електронна адреса")]
        public string Email { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }

}
