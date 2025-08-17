using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LMS_Quadra.Controllers
{
    [Route("Course/{courseId}/Test")]
    [Authorize]
    public class TestController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly UserManager<IdentityUser> _userManager;

        public TestController(
            DataManager dataManager,
            UserManager<IdentityUser> userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }

        [HttpGet("Start")]
        public async Task<IActionResult> Start(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);
            var completion = await _dataManager.CourseCompletionRepository
                .GetByCourseAndWorkerAsync(courseId, worker.Id);

            if (completion?.DateCompleted != null)
                return RedirectToAction("Show", "Course", new { id = courseId });

            var questions = await _dataManager.QuestionRepository
                .GetRandomQuestionsAsync(courseId, completion.Course.NumQuestions);

            ViewBag.CourseId = courseId;


            var questionIds = questions.Select(q => q.Id).ToList();
            HttpContext.Session.SetString($"TestQuestions_{courseId}",
                                        JsonSerializer.Serialize(questionIds));
            return View(questions);
        }

        [HttpPost("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            int courseId,
            [FromForm] Dictionary<int, int> selectedAnswers)
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);
            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(courseId);
            var completion = await _dataManager.CourseCompletionRepository
                .GetByCourseAndWorkerAsync(courseId, worker.Id);

            var questionIdsJson = HttpContext.Session.GetString($"TestQuestions_{courseId}");
            var questionIds = JsonSerializer.Deserialize<List<int>>(questionIdsJson);
            var testQuestions = await _dataManager.QuestionRepository.GetQuestionsByIdsAsync(questionIds);

            var totalQuestions = completion.Course.NumQuestions;

            if (selectedAnswers.Count != testQuestions.Count)
            {
                ModelState.AddModelError("", "Ответьте на все вопросы!");
                ViewBag.CourseId = courseId;
                return View("Start", testQuestions);
            }


            var correctCount = testQuestions.Count(q =>
                selectedAnswers.TryGetValue(q.Id, out var answerId) &&
                q.Answers.Any(a => a.Id == answerId && a.IsCorrect)
            );

            var resultPercent = (correctCount / (double)totalQuestions) * 100;
            var isSuccess = resultPercent >= completion.Course.MinResult;

            if (isSuccess)
            {
                completion.Result = (decimal)Math.Round(resultPercent);
                completion.DateCompleted = DateTime.Now;
                await _dataManager.CourseCompletionRepository.SaveCourseCompletionAsync(completion);
            }

            HttpContext.Session.Remove($"TestQuestions_{courseId}");

            return RedirectToAction("Result", new
            {
                courseId,
                percent = (decimal)Math.Round(resultPercent),
                isSuccess
            });
        }

        [HttpGet("Result")]
        public async Task<IActionResult> Result(int courseId, decimal percent, bool isSuccess)
        {
            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(courseId);
            ViewBag.CourseId = courseId;
            ViewBag.Percent = percent;
            ViewBag.MinResult = course.MinResult;
            ViewBag.IsSuccess = isSuccess;
            return View();
        }
    }
}
