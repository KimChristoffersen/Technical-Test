using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TekniskOpgave.DAL.Interfaces;
using TekniskOpgave.DAL.Models;

namespace TekniskOpgave_Montorer_Overmontorer.Controllers
{
    public class MontorerController : Controller
    {
        private readonly IOvermontorerSqlDataAccess _overmontorerDataAccess;
        private readonly IMontorerSqlDataAccess _montorerSqlDataAccess;

        public MontorerController(IOvermontorerSqlDataAccess overmontorerDataAccess, IMontorerSqlDataAccess montorerSqlDataAccess)
        {
            _overmontorerDataAccess = overmontorerDataAccess;
            _montorerSqlDataAccess = montorerSqlDataAccess;
        }

        public IActionResult Index()
        {
            var montorer = _montorerSqlDataAccess.GetAllMontors();
            return View(montorer);
        }

        public IActionResult Details(int id)
        {
            Montor montor = _montorerSqlDataAccess.GetMontorById(id);
            if (montor == null)
            {
                return NotFound();
            }

            return View(montor);
        }

        public IActionResult Create()
        {
            var overmontorer = _overmontorerDataAccess.GetAllOvermontors();

            ViewBag.Overmontorer = new SelectList(overmontorer, "OvermontorId", "OvermontorNavn");

            return View(new Montor() { MontorId = 0, MontorNavn = "", MontorTelefonnummer = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Montor montor, int? overmontorId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _montorerSqlDataAccess.CreateMontor(montor, overmontorId);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Unable to create montor.";
                    return View("Error");
                }
            }
            return View(montor);
        }


        public IActionResult Delete(int id)
        {
            try
            {
                _montorerSqlDataAccess.DeleteMontor(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to delete montor.";
                return View("Error");
            }
        }


        public IActionResult Edit(int id)
        {
            Montor montor = _montorerSqlDataAccess.GetMontorById(id);
            if (montor == null)
            {
                return NotFound();
            }
            return View(montor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Montor montor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _montorerSqlDataAccess.UpdateMontor(montor);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Unable to update montor.";
                    return View("Error");
                }
            }
            return View(montor);
        }

    }
}
