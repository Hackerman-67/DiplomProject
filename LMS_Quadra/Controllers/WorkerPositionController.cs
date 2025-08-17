using LMS_Quadra.Domain;
using LMS_Quadra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LMS_Quadra.Controllers
{
    [Authorize]
    public class WorkerPositionController : Controller
    {
        private readonly DataManager _dataManager;
        public WorkerPositionController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.WorkerPositions = await _dataManager.WorkerPositionRepository.GetWorkerPositionsAsync();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new WorkerPosition());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(WorkerPosition entity)
        {
            if (ModelState.IsValid)
            {
                await _dataManager.WorkerPositionRepository.SaveWorkerPositionAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var position = await _dataManager.WorkerPositionRepository.GetWorkerPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, WorkerPosition entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _dataManager.WorkerPositionRepository.SaveWorkerPositionAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataManager.WorkerPositionRepository.DeleteWorkerPositionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
