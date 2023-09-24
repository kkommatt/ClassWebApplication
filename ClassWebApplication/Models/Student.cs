using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassWebApplication.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "ПІБ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [EmailAddress(ErrorMessage = "Неправильний формат")]
        [Display(Name = "Електронна адреса")]
        public string Email { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

}
