using EntitiesFitness;
using EntityFitness.MyEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppFitness.Controllers
{
    public class SeanceController : Controller
    {
        private readonly SqliteContext _context;
        public SeanceController(SqliteContext context)
        {
            _context = context;
        }

        public IActionResult ListeSeance()
        {
            List<Seance> liste = _context.Seances
                .Include(n => n.NomSeance)
                .OrderBy(o => o.NomSeance)
                .ToList();
            int index = 0;
            for (int i = 1; i < liste.Count;)
            {
                if (liste[index].NomSeanceId == liste[i].NomSeanceId)
                {
                    liste.Remove(liste[i]);
                }
                else
                {
                    index = i;
                    i++;
                }
            }
            return View(liste);
        }

        [HttpGet]
        public IActionResult CreateSeance()
        {
            List<Exercice> listeExo = _context.Exercices
                .Include(n => n.NomExercice.Muscle)
                .Include(m => m.NomExercice)
                .OrderBy(o => o.Id)
                .ToList();
            List<SelectListItem> s = new List<SelectListItem>();
            foreach (Exercice e in listeExo)
            {
                s.Add(new SelectListItem()
                {
                    Text = $"{e.NomExercice.Nom} ({e.NomExercice.Muscle.Nom})",
                    Value = e.Id.ToString(),
                });
            }
            ViewBag.listeExo = s;
            return View();
        }
        [HttpPost]
        public IActionResult CreateSeance(SeanceExercice se)
        {
            NomSeance ns = new NomSeance();
            Seance s = new Seance();
            bool doublon = _context.NomSeances.Any(n => n.Nom == se.Seance.NomSeance.Nom);
            if (doublon)
            {
                return View(se);
            }
            ns = se.Seance.NomSeance;
            _context.NomSeances.Add(ns);
            _context.SaveChanges();
            NomSeance recherche = _context.NomSeances
                .FirstOrDefault(m => m.Nom == ns.Nom);
            int IdNomExercice = recherche.Id;
            foreach (int e in se.exo)
            {
                s.NomSeanceId = IdNomExercice;
                s.ExerciceId = e;
                _context.Seances.Add(s);
                _context.SaveChanges();
                s = new Seance();
            }
            return RedirectToAction("ListeSeance");
        }

        [HttpPost]
        public IActionResult DeleteSeance(int id)
        {
            Seance s = _context.Seances.Find(id);
            NomSeance nomSeance = _context.NomSeances
                .FirstOrDefault(n => n.Id == s.NomSeanceId);
            _context.Seances.Remove(s);
            _context.SaveChanges();
            _context.NomSeances.Remove(nomSeance);
            _context.SaveChanges();
            return RedirectToAction("ListeSeance");
        }

        [HttpGet]
        public IActionResult DetailSeance(int id) 
        {
            List<Seance> listeSeance = _context.Seances
                .Include(c => c.NomSeance)
                .Include(c => c.Exercice)
                .Include(ne => ne.Exercice.NomExercice)
                .Include(r => r.Exercice.Serie)
                .OrderBy(a => a.Exercice.NomExercice.Nom)
                .ThenBy(d => d.Exercice.Date)
                .Where(s => s.NomSeanceId == id)
                .ToList();
            return View(listeSeance);
        }
        [HttpGet]
        public IActionResult EditSeance(int id) 
        {
            Seance s = _context.Seances
                .Include(c => c.NomSeance)
                .Include(c => c.Exercice)
                .Include(ne => ne.Exercice.NomExercice)
                .Include(r => r.Exercice.Serie)
                .FirstOrDefault(i => i.Id == id);
            return PartialView("_EditSeancePartial",s);
        }
    }
}
