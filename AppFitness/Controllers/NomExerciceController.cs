using EntitiesFitness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace AppFitness.Controllers
{
    public class NomExerciceController : Controller
    {
        private readonly SqliteContext _context;
        public  NomExerciceController(SqliteContext context) 
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult ListeNomExercice()
        {
            List<NomExercice> list = _context.NomExercices
                .Include(m => m.Muscle)
                .OrderBy(n => n.Nom)
                .ToList();
            return View(list);
        }
        [HttpPost]
        public IActionResult DeleteNomExercice(int id) 
        {
            NomExercice exo = _context.NomExercices.Find(id);
            if (exo == null) 
            {
                ViewBag.Message = "Problème lors de la suppression !";
                return RedirectToAction("ListeNomExercice");
            }
            _context.NomExercices.Remove(exo);
            _context.SaveChanges();
            return RedirectToAction("ListeNomExercice");
        }

        [HttpGet]
        public IActionResult CreateNomExercice() 
        {
            List<Muscle> listMuscle = _context.Muscles.
               OrderBy(m => m.Nom).
               ToList();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (Muscle m in listMuscle)
            {
                selectListItem.Add(new SelectListItem()
                {
                    Text = $"{m.Nom}",
                    Value = m.Id.ToString()
                });
            }
            ViewBag.listeMuscle = selectListItem;
            return PartialView("_CreateNomExercicePartial");
        }
        [HttpPost]
        public IActionResult CreateNomExercice(NomExercice exo)
        {
            bool doublon = _context.NomExercices.Any(d => d.Nom == exo.Nom);
            if (doublon)
            {
                ModelState.AddModelError("Nom","Cet Exercice existe déjà.");
                return PartialView("ListeNomExercice", exo);
            }
            List<Muscle> listMuscle = _context.Muscles.
               OrderBy(m => m.Nom).
               ToList();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (Muscle m in listMuscle)
            {
                selectListItem.Add(new SelectListItem()
                {
                    Text = $"{m.Nom}",
                    Value = m.Id.ToString()
                });
            }
            ViewBag.listeMuscle = selectListItem;
            _context.NomExercices.Add(exo);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult EditNomExercice(int id) 
        {
            NomExercice exo = _context.NomExercices.Find(id);
            List<Muscle> listMuscle = _context.Muscles.
               OrderBy(m => m.Nom).
               ToList();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (Muscle m in listMuscle)
            {
                selectListItem.Add(new SelectListItem()
                {
                    Text = $"{m.Nom}",
                    Value = m.Id.ToString()
                });
            }
            ViewBag.listeMuscle = selectListItem;
            return PartialView("_EditNomExercicePartial", exo);
        }

        [HttpPost]
        public IActionResult EditNomExercice(NomExercice exo)
        {
            _context.NomExercices.Update(exo);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
