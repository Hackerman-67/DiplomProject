using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LMS_Quadra.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly UserManager<IdentityUser> _userManager;
        public CourseController(DataManager dataManager, UserManager<IdentityUser> userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Employee") || await _userManager.IsInRoleAsync(user, "HR"))
            {
                var worker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);

                var assignedCourses = await _dataManager.CourseCompletionRepository
                    .GetByWorkerIdWithCoursesAsync(worker.Id, DateTime.Now);

                ViewBag.ActiveCourses = assignedCourses
                    .Where(cc => cc.DateCompleted == null)
                    .Select(cc => cc.Course)
                    .ToList();

                ViewBag.CompletedCourses = assignedCourses
                    .Where(cc => cc.DateCompleted != null)
                    .Select(cc => cc.Course)
                    .ToList();
            }
            else if (await _userManager.IsInRoleAsync(user, "Admin")) 
            {
                ViewBag.ActiveCourses = await _dataManager.CourseRepository.GetCourseAsync();
                ViewBag.CompletedCourses = new List<Course>();
            }

            else
            {
                ViewBag.ActiveCourses = new List<Course>();
                ViewBag.CompletedCourses = new List<Course>();
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {
            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (course == null) return NotFound();

            course.CourseContents = (await _dataManager.CourseContentRepository.GetCourseContentByCourseIdAsync(id)).ToList();

            var worker = await _dataManager.WorkerRepository.GetByUserIdAsync(user.Id);
            var completion = await _dataManager.CourseCompletionRepository.GetByCourseAndWorkerAsync(id, worker.Id);

            ViewBag.IsTestAvailable = completion != null
                              && (completion.DateCompleted == null)
                              && DateTime.Now >= completion.DateOpen;

            ViewBag.TestResult = completion?.Result;

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Course());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Course entity)
        {
            if (ModelState.IsValid)
            {
                await _dataManager.CourseRepository.SaveCourseAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _dataManager.CourseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Course entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _dataManager.CourseRepository.SaveCourseAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataManager.CourseRepository.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
