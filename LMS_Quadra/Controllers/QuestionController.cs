using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Quadra.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/Course/{courseId}/[controller]")]
    public class QuestionController : Controller
    {
        private readonly DataManager _dataManager;

        public QuestionController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int courseId)
        {
            var questions = await _dataManager.QuestionRepository.GetQuestionsByCourseIdAsync(courseId);
            ViewBag.CourseId = courseId;
            return View(questions);
        }

        [HttpGet("Create")]
        public IActionResult Create(int courseId)
        {
            return View(new Question { CourseId = courseId });
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, Question model)
        {
            if (model.CourseId != courseId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _dataManager.QuestionRepository.SaveQuestionAsync(model);
                return RedirectToAction("Index", new { courseId });
            }
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int courseId, int id)
        {
            var question = await _dataManager.QuestionRepository.GetQuestionByIdAsync(id);
            if (question?.CourseId != courseId) return NotFound();

            return View(question);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int courseId, int id, Question model)
        {
            if (id != model.Id || courseId != model.CourseId) return NotFound();

            if (ModelState.IsValid)
            {
                await _dataManager.QuestionRepository.SaveQuestionAsync(model);
                return RedirectToAction("Index", new { courseId });
            }
            return View(model);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int courseId, int id)
        {
            await _dataManager.QuestionRepository.DeleteQuestionAsync(id);
            return RedirectToAction("Index", new { courseId });
        }
    }
}