using Microsoft.AspNetCore.Mvc;
using TekniskOpgave.DAL.Interfaces;
using TekniskOpgave.DAL.Models;

namespace TekniskOpgave_Montorer_Overmontorer.Controllers
{
    public class OvermontorerController : Controller
    {
        private readonly IOvermontorerSqlDataAccess _overmontorerDataAccess;
        private readonly IMontorerSqlDataAccess _montorerSqlDataAccess;

        public OvermontorerController(IOvermontorerSqlDataAccess overmontorerDataAccess, IMontorerSqlDataAccess montorerSqlDataAccess)
        {
            _overmontorerDataAccess = overmontorerDataAccess;
            _montorerSqlDataAccess = montorerSqlDataAccess;
        }

        public IActionResult Index()
        {
            var overmontorer = _overmontorerDataAccess.GetAllOvermontors();
            return View(overmontorer);
        }

        public IActionResult Details(int id)
        {
            var overmontor = _overmontorerDataAccess.GetOvermontorById(id);

            // Hent alle montører, som allerede er tilknyttet en overmontør
            var allMontors = _montorerSqlDataAccess.GetAllMontors();
            var assignedMontors = overmontor.Montorer.Select(m => m.MontorId).ToList();

            // Filtrer de montører, der allerede er tilknyttet overmontøren
            var availableMontors = allMontors.Where(m => !assignedMontors.Contains(m.MontorId)).ToList();

            // Send de ledige montører til viewet
            ViewBag.AvailableMontors = availableMontors;

            return View(overmontor);
        }


        public IActionResult AddMontorToOvermontor(int montorId, int overmontorId)
        {
            try
            {
                _overmontorerDataAccess.AddMontorToOvermontor(montorId, overmontorId);
                return RedirectToAction("Details", new { id = overmontorId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to add montor to overmontor.";
                return View("Error");
            }
        }


        public IActionResult RemoveMontorFromOvermontor(int montorId, int overmontorId)
        {
            var overmontor = _overmontorerDataAccess.GetOvermontorById(overmontorId);
            if (overmontor == null)
            {
                return NotFound();
            }

            var montor = _montorerSqlDataAccess.GetMontorById(montorId);
            if (montor == null)
            {
                return NotFound();
            }

            if (montor.Overmontorer.Count == 1)
            {
                TempData["ErrorMessage"] = "Denne montør kan ikke fjernes, da den kun har denne overmontør tilknyttet. Tilknyt montøren til en anden overmontør og prøv igen.";
                return RedirectToAction("Details", new { id = overmontorId });
            }

            try
            {
                _overmontorerDataAccess.RemoveMontorFromOvermontor(montorId, overmontorId);
                return RedirectToAction("Details", new { id = overmontorId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kan ikke fjerne montøren fra overmontøren.";
                return RedirectToAction("Details", new { id = overmontorId });
            }
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Overmontor overmontor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _overmontorerDataAccess.CreateOvermontor(overmontor);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Unable to create overmontør.";
                    return View("Error");
                }
            }
            return View(overmontor);
        }


        public IActionResult Delete(int id)
        {
            var overmontor = _overmontorerDataAccess.GetOvermontorById(id);
            if (overmontor == null)
            {
                return NotFound();
            }
            return View(overmontor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var overmontor = _overmontorerDataAccess.GetOvermontorById(id);

            if (overmontor == null)
            {
                return NotFound();
            }

            List<string> montorWithOnlyOneOvermontor = new List<string>();

            foreach (var montor in overmontor.Montorer)
            {
                if (_montorerSqlDataAccess.GetMontorById(montor.MontorId).Overmontorer.Count == 1)
                {
                    montorWithOnlyOneOvermontor.Add(montor.MontorNavn);
                }
            }

            if (montorWithOnlyOneOvermontor.Count > 0)
            {
                string montorNavne = string.Join(", ", montorWithOnlyOneOvermontor);
                ViewBag.ErrorMessage = $"Denne overmontør kan ikke slettes, da følgende montører kun har denne overmontør tilknyttet: {montorNavne}. Tilknyt dem en anden overmontør og prøv igen";
                return View("Delete", overmontor);
            }

            try
            {
                _overmontorerDataAccess.DeleteOvermontor(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Kan ikke slette overmontøren.";
                return View("Error");
            }
        }

        public IActionResult Edit(int id)
        {
            Overmontor overmontor = _overmontorerDataAccess.GetOvermontorById(id);
            if (overmontor == null)
            {
                return NotFound();
            }
            return View(overmontor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Overmontor overmontor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _overmontorerDataAccess.UpdateOvermontor(overmontor);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Unable to update overmontor.";
                    return View("Error");
                }
            }
            return View(overmontor);
        }
    }
}
