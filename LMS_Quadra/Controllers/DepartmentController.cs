using LMS_Quadra.Domain;
using LMS_Quadra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LMS_Quadra.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly DataManager _dataManager;
        public DepartmentController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index() 
        {
            ViewBag.Departments = await _dataManager.DepartmentRepository.GetDepartmentAsync();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Department());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Department entity)
        {
            if (ModelState.IsValid)
            {
                await _dataManager.DepartmentRepository.SaveDepartmentAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _dataManager.DepartmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Department entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _dataManager.DepartmentRepository.SaveDepartmentAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataManager.DepartmentRepository.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
