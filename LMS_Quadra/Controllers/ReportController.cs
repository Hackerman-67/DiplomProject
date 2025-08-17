using LMS_Quadra.Domain;
using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Quadra.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportController(
            DataManager dataManager,
            UserManager<IdentityUser> userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> CompletedCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);

            var completions = await _dataManager.CourseCompletionRepository
                .GetUserCompletionsWithCoursesAsync(worker.Id);

            return View(completions);
        }

        public async Task<IActionResult> DepartmentProgress()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentWorker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);

            var workers = await _dataManager.WorkerRepository
                .GetByDepartmentIdAsync(currentWorker.DepartmentId);

            var result = new List<Worker>();
            foreach (var worker in workers)
            {
                worker.CompletedCoursesCount = await _dataManager.CourseCompletionRepository
                    .GetCompletedCount(worker.Id);
                result.Add(worker);
            }
            ViewBag.CurrentWorkerId = currentWorker.Id;
            return View(result.OrderByDescending(w => w.CompletedCoursesCount).ToList());
        }

        public async Task<IActionResult> DepartmentAverage()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentWorker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);

            var workers = await _dataManager.WorkerRepository
                .GetByDepartmentIdAsync(currentWorker.DepartmentId);

            foreach (var worker in workers)
            {
                worker.AverageResult = await _dataManager.CourseCompletionRepository
                    .GetAverageResult(worker.Id);
            }

            ViewBag.CurrentWorkerId = currentWorker.Id;
            return View(workers.OrderByDescending(w => w.AverageResult).ToList());
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public async Task<IActionResult> WorkerCoursesReport()
        {
            var workers = await _dataManager.WorkerRepository.GetWorkerAsync();
            ViewBag.Workers = new SelectList(workers, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPost]
        public async Task<IActionResult> WorkerCoursesReport(int workerId)
        {
            if (workerId == 0)
            {
                ModelState.AddModelError("", "Выберите сотрудника");
                var workers = await _dataManager.WorkerRepository.GetWorkerAsync();
                ViewBag.Workers = new SelectList(workers, "Id", "Name");
                return View();
            }

            var completions = await _dataManager.CourseCompletionRepository
                .GetUserCompletionsWithCoursesAsync(workerId);

            var worker = await _dataManager.WorkerRepository.GetWorkerByIdAsync(workerId);
            ViewBag.SelectedWorker = worker?.Name;

            return View("WorkerCoursesResult", completions);
        }



        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CourseAverageReport()
        {
            var courses = await _dataManager.CourseRepository.GetCourseAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CourseAverageReport(int courseId)
        {
            var courses = await _dataManager.CourseRepository.GetCourseAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            if (courseId == 0)
            {
                ModelState.AddModelError("", "Выберите курс");
                return View();
            }

            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(courseId);
            var averageResult = await _dataManager.CourseCompletionRepository
                .GetAverageResultByCourse(courseId) ?? 0;

            ViewBag.CourseTitle = course.Title;
            ViewBag.AverageResult = averageResult;

            return View("CourseAverageResult");
        }



        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CourseTopResultReport()
        {
            var courses = await _dataManager.CourseRepository.GetCourseAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CourseTopResultReport(int courseId)
        {
            var courses = await _dataManager.CourseRepository.GetCourseAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            if (courseId == 0)
            {
                ModelState.AddModelError("", "Выберите курс");
                return View();
            }

            var topResult = await _dataManager.CourseCompletionRepository
                .GetTopResultByCourse(courseId);

            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(courseId);
            ViewBag.CourseTitle = course.Title;

            return View("CourseTopResultResult", topResult);
        }



        [Authorize(Roles = "Admin,HR")]
        public IActionResult DepartmentReport()
        {
            return View(new DateRangeViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DepartmentReport(DateRangeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError("", "Дата начала не может быть позже даты конца периода");
                return View(model);
            }

            var departments = await _dataManager.DepartmentRepository
                .GetDepartmentReport(model.StartDate, model.EndDate);

            ViewBag.StartDate = model.StartDate.ToString("dd.MM.yyyy");
            ViewBag.EndDate = model.EndDate.ToString("dd.MM.yyyy");

            return View("DepartmentResult", departments);
        }



        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> WorkerPeriodReport()
        {
            ViewBag.Workers = new SelectList(
                await _dataManager.WorkerRepository.GetWorkerAsync(),
                "Id",
                "Name"
            );
            return View(new WorkerPeriodReportViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> WorkerPeriodReport(WorkerPeriodReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Workers = new SelectList(
                    await _dataManager.WorkerRepository.GetWorkerAsync(),
                    "Id",
                    "Name"
                );
                return View(model);
            }

            var worker = await _dataManager.WorkerRepository.GetWorkerByIdAsync(model.WorkerId);
            if (worker == null)
            {
                ModelState.AddModelError("", "Сотрудник не найден");
                return View(model);
            }

            var startDate = model.StartDate.Date;
            var endDate = model.EndDate.Date.AddDays(1).AddTicks(-1);

            var completedCount = await _dataManager.CourseCompletionRepository
                .GetCompletedCountWithDate(worker.Id, startDate, endDate);

            ViewBag.Period = $"{model.StartDate:dd.MM.yyyy} - {model.EndDate:dd.MM.yyyy}";

            return View("WorkerPeriodResult", Tuple.Create(worker, completedCount));
        }
    }
}
