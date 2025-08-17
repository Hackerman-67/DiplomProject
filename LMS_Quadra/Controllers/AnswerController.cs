using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Quadra.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/Course/{courseId}/Question/{questionId}/[controller]")]
    public class AnswerController : Controller
    {
        private readonly DataManager _dataManager;

        public AnswerController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        [HttpGet("Create")]
        public IActionResult Create(int courseId, int questionId)
        {
            return View(new Answer { QuestionId = questionId });
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, int questionId, Answer model)
        {
            var question = await _dataManager.QuestionRepository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                ModelState.AddModelError("", "Указанный вопрос не существует");
                return View(model);
            }
            model.Question = question;
            model.QuestionId = questionId;
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataManager.AnswerRepository.SaveAnswerAsync(model);
                    return RedirectToQuestion(courseId, questionId);
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении ответа");
                }
            }
            return View(model);
        }

        [HttpGet("Edit/{answerId}")]
        public async Task<IActionResult> Edit(int courseId, int questionId, int answerId)
        {
            var answer = await ValidateAnswer(courseId, questionId, answerId);
            if (answer == null) return NotFound();

            return View(answer);
        }

        [HttpPost("Edit/{answerId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int courseId, int questionId, int answerId, Answer model)
        {
            if (answerId != model.Id || questionId != model.QuestionId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _dataManager.AnswerRepository.SaveAnswerAsync(model);
                return RedirectToQuestion(courseId, questionId);
            }
            return View(model);
        }

        [HttpPost("Delete/{answerId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int courseId, int questionId, int answerId)
        {
            await _dataManager.AnswerRepository.DeleteAnswerAsync(answerId);
            return RedirectToQuestion(courseId, questionId);
        }

        private async Task<Answer?> ValidateAnswer(int courseId, int questionId, int answerId)
        {
            var answer = await _dataManager.AnswerRepository.GetAnswerByIdAsync(answerId);
            if (answer?.Question?.CourseId != courseId || answer.QuestionId != questionId)
            {
                return null;
            }
            return answer;
        }

        private IActionResult RedirectToQuestion(int courseId, int questionId)
        {
            return RedirectToAction("Edit", "Question", new { courseId, id = questionId });
        }
    }
}