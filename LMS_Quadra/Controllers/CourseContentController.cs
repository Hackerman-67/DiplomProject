using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Quadra.Controllers
{
    [Authorize]
    [Route("/Course/{courseId}/[controller]")]
    public class CourseContentController : Controller
    {
        private readonly DataManager _dataManager;

        public CourseContentController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create(int courseId)
        {
            return View(new CourseContent { CourseId = courseId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(int courseId, CourseContent model)
        {
            if (model.CourseId != courseId)
            {
                return BadRequest();
            }
            model.CourseId = courseId;
            await _dataManager.CourseContentRepository.SaveCourseContentAsync(model);
            return RedirectToAction("Show", "Course", new { id = courseId });
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int courseId, int id)
        {
           var content = await _dataManager.CourseContentRepository.GetCourseContentByIdAsync(id);

           if (content == null || content.CourseId != courseId)
           {
               return NotFound();
           }
               return View(content);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int courseId, int id)
        {
            var content = await _dataManager.CourseContentRepository.GetCourseContentByIdAsync(id);
            if (content?.CourseId != courseId) return NotFound();

            return View(content);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int courseId, int id, CourseContent model)
        {
            if (id != model.Id || courseId != model.CourseId) return NotFound();

            await _dataManager.CourseContentRepository.SaveCourseContentAsync(model);
            return RedirectToAction("Show", "Course", new { id = courseId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int courseId, int id)
        {
            await _dataManager.CourseContentRepository.DeleteCourseContentAsync(id);
            return RedirectToAction("Show", "Course", new { id = courseId });
        }

    }
}
