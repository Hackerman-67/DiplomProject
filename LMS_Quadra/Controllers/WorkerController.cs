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
    [Authorize]
    public class WorkerController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly UserManager<IdentityUser> _userManager;
        public WorkerController(DataManager dataManager, UserManager<IdentityUser> userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Workers = await _dataManager.WorkerRepository.GetWorkerAsync();
            return View();
        }

        private async Task LoadSelectLists(Worker? worker = null)
        {
            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Departments = await _dataManager.DepartmentRepository.GetDepartmentAsync();
            ViewBag.WorkerPositions = await _dataManager.WorkerPositionRepository.GetWorkerPositionsAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View(new Worker());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Worker entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataManager.WorkerRepository.SaveWorkerAsync(entity);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ошибка сохранения: " + ex.InnerException?.Message);
                }
            }
            await LoadSelectLists(entity);
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var worker = await _dataManager.WorkerRepository.GetWorkerByIdAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            await LoadSelectLists();
            return View(worker);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Worker entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _dataManager.WorkerRepository.SaveWorkerAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            await LoadSelectLists(entity);
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataManager.WorkerRepository.DeleteWorkerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
