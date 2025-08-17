using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace LMS_Quadra.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class CourseCompletionController : Controller
    {
        private readonly DataManager _dataManager;
        public CourseCompletionController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseCompletions = await _dataManager.CourseCompletionRepository.GetCourseCompletionAsync();
            return View();
        }

        private async Task LoadSelectLists()
        {
            ViewBag.Workers = await _dataManager.WorkerRepository.GetWorkerAsync();
            ViewBag.Courses = await _dataManager.CourseRepository.GetCourseAsync();
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View(new CourseCompletion());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCompletion entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataManager.CourseCompletionRepository.SaveCourseCompletionAsync(entity);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ошибка сохранения: " + ex.InnerException?.Message);
                }
            }
            await LoadSelectLists();
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseCompletion = await _dataManager.CourseCompletionRepository.GetCourseCompletionByIdAsync(id);
            if (courseCompletion == null)
            {
                return NotFound();
            }
            await LoadSelectLists();
            return View(courseCompletion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseCompletion entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _dataManager.CourseCompletionRepository.SaveCourseCompletionAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            await LoadSelectLists();
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataManager.CourseCompletionRepository.DeleteCourseCompletionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
