using EntitiesFitness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace AppFitness.Controllers
{
    public class ExerciceController : Controller
    {
        private readonly SqliteContext _context;
        public ExerciceController(SqliteContext context) 
        {
            _context = context;
        }
        public IActionResult ListeExercice()
        {
            List<Exercice> liste = _context.Exercices
                .Include(ne => ne.NomExercice)
                .Include(m => m.NomExercice.Muscle)
                .Include(s => s.Serie)                                
                .GroupBy(e => e.NomExercice.Nom)
                .Select(g => g.OrderByDescending(r => r.Serie.Rep * r.Serie.Poids)
                .First())
                .ToList();
            return View(liste);
        }

        [HttpPost]
        public IActionResult DeleteExercice(int id) 
        {
            Exercice exo = _context.Exercices.Find(id);
            NomExercice ne = _context.NomExercices.Find(exo.NomExerciceId);
            List<Exercice> liste = _context.Exercices
                .Where(e => e.NomExerciceId == ne.Id)
                .ToList();
            foreach (Exercice e in liste) 
            {
                _context.Exercices.Remove(e);
            }
            _context.NomExercices.Remove(ne);
            _context.SaveChanges();
            return RedirectToAction("ListeExercice");
        }
        
        [HttpGet]
        public IActionResult CreateExercice() 
        {

            List<NomExercice> listExo = _context.NomExercices.
               OrderBy(m => m.Nom).
               ToList();
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
            return View();
        }

        [HttpPost]
        public IActionResult CreateExercice(NomExercice e) 
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
            bool doublon = _context.NomExercices.Any(d => d.Nom == e.Nom);
            if (doublon)
            {
                ViewBag.Message = "Cet exercice existe !";
                return View(e);
            }
            _context.NomExercices.Add(e);
            _context.SaveChanges();
            NomExercice e1 = _context.NomExercices.FirstOrDefault(s => s.Nom == e.Nom);
            Exercice exo = new Exercice();
            exo.NomExerciceId = e1.Id;
            exo.SerieId = 1;
            exo.Date = DateTime.Now.ToString("MM/dd/yyyy");
            _context.Exercices.Add(exo);
            _context.SaveChanges();
            return RedirectToAction("ListeExercice");
        }

        [HttpGet]
        public IActionResult EditExercice(int id) 
        {
            NomExercice exo = _context.NomExercices
                .Include(e => e.Muscle)
                .FirstOrDefault(exo => exo.Id == id);
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
            return View(exo);
        }
        [HttpPost]
        public IActionResult EditExercice(NomExercice ne)
        {
            List<Exercice> liste = _context.Exercices
                .Where(e => e.NomExerciceId == ne.Id)
                .ToList();
            _context.NomExercices.Update(ne);
            List<Muscle> listMuscle = _context.Muscles.
               OrderBy(m => m.Nom).
               ToList();
            foreach (Exercice e in liste) 
            {
                _context.Exercices.Update(e);
            }
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
            _context.SaveChanges();
            return RedirectToAction("ListeExercice");
        }

        [HttpGet]
        public IActionResult DetailExercice(int id)
        {
            List<Exercice> liste = _context.Exercices
                .Include(m => m.NomExercice)
                .Include(s => s.Serie)
                .Include(r => r.NomExercice.Muscle)
                .Where (e => e.NomExerciceId == id)
                .ToList();
            NomExercice ne = _context.NomExercices.FirstOrDefault(e => e.Id == id);
            ViewBag.nomExo = ne.Nom;
            return View(liste);
        }
    }
}
