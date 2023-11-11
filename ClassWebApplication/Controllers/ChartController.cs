using ClassWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("countStudents")]
        public JsonResult GetCountStudents()
        {
            var coursesData = _context.Courses
                .Select(course => new
                {
                    label = course.Title,
                    value = course.StudentCourses.Count 
                })
                .ToList();

            return new JsonResult(coursesData);
        }

        [HttpGet("lecturersData")]
        public JsonResult GetLecturersData()
        {
            var lecturersData = _context.Lectors
                .Select(lector => new
                {
                    lecturer = lector.FullName,
                    coursesCount = lector.LectorCourses.Count 
                })
                .ToList();

            return new JsonResult(lecturersData);
        } 
    }
}
