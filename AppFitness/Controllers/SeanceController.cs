using EntitiesFitness;
using EntityFitness.MyEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
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
            if (se.exo.Count == 0) 
            {
                List<Exercice> listeExo = _context.Exercices
                    .Include(n => n.NomExercice.Muscle)
                    .Include(m => m.NomExercice)
                    .OrderBy(o => o.Id)
                    .ToList();
                List<SelectListItem> sel = new List<SelectListItem>();
                foreach (Exercice e in listeExo)
                {
                    sel.Add(new SelectListItem()
                    {
                        Text = $"{e.NomExercice.Nom} ({e.NomExercice.Muscle.Nom})",
                        Value = e.Id.ToString(),
                    });
                }
                ViewBag.listeExo = sel;
                return View("CreateSeance");
            }
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
        public IActionResult DeleteSeance(int Nom)
        {
            List<Seance> listeSeance = _context.Seances
                .Where(n => n.NomSeanceId == Nom)
                .ToList();
            foreach (Seance se in listeSeance) 
            {
                _context.Seances.Remove(se);
                _context.SaveChanges();
            }
            NomSeance nomSeance = _context.NomSeances
                .FirstOrDefault(n => n.Id == Nom);
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
        public IActionResult AddExercice(int id) 
        {
            NomSeance Nom = _context.NomSeances.FirstOrDefault(n => n.Id == id);
            ViewBag.NomSeance = Nom.Nom;
            ViewBag.NomSeanceId = id;
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
            return PartialView("_AddExercicePartial");
        }
        [HttpPost]
        public IActionResult AddExercice(int pageIndex, Seance s) 
        {
            _context.Seances.Add(s);
            _context.SaveChanges();
            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
        }

        [HttpPost]
        public IActionResult DeleteExercice(int pageIndex, int exoId) 
        {
            List<Seance> listeSeance = _context.Seances
                .Where(a => a.NomSeanceId == pageIndex)
                .ToList();
            if (listeSeance.Count == 1) 
            {
                TempData["Erreur"] = "Une Séance doit au minimum avoir un Exercice !";
                return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
            }
            Seance s = _context.Seances
                .FirstOrDefault(e => e.ExerciceId == exoId);
            _context.Seances.Remove(s);
            _context.SaveChanges();
            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
        }

        public IActionResult AddSerie(int pageIndex, int Poids, int Rep, int NomExo, string Date) 
        {
            List<Serie> listeSerie = _context.Series.ToList();
            bool IndexNonUtiliser;
            int index = 0;
            Serie s = new Serie();
            s.Rep = Rep;
            s.Poids = Poids;
            for (int i = 1; i < 10000; i++) 
            {
                IndexNonUtiliser = listeSerie.Any(s => s.Id == i);
                if (IndexNonUtiliser == false) 
                {
                    index = i;
                    i = 10000 - 1;
                }
            }
            s.Id = index;
            _context.Series.Add(s);
            _context.SaveChanges();

            List<Exercice> listeExo = _context.Exercices.ToList();
            Exercice exo = new Exercice();
            exo.SerieId = s.Id;
            exo.Date = Date;
            exo.NomExerciceId = NomExo;
            bool IndexNonUtiliser1;
            int index1 = 0;
            for (int i = 1; i < 10000; i++)
            {
                IndexNonUtiliser1 = listeExo.Any(s => s.Id == i);
                if (IndexNonUtiliser1 == false)
                {
                    index1 = i;
                    i = 10000 - 1;
                }
            }
            exo.Id = index1;
            exo.NumSerie = 1;
            _context.Exercices.Add(exo);
            _context.SaveChanges();

            Seance seance = new Seance();
            seance.NomSeanceId = pageIndex;
            seance.ExerciceId = exo.Id;
            _context.Seances.Add(seance);
            _context.SaveChanges();

            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
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
            Serie serie = s.Exercice.Serie;
            ViewBag.Message = s.Exercice.NomExercice.Nom;
            ViewBag.Message1 = s.NomSeanceId;
            return PartialView("_EditSeancePartial",serie);
        }

        [HttpPost]
        public IActionResult EditSeance(Serie s, int pageIndex, int numSerie)
        {
            Exercice exo = _context.Exercices.FirstOrDefault(n => n.SerieId == s.Id);
            exo.NumSerie = numSerie;
            _context.Exercices.Update(exo);
            _context.Series.Update(s);
            _context.SaveChanges();
            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
        }

        [HttpPost]
        public IActionResult DeleteSerie(int pageIndex, int SerieId) 
        {
            Exercice exo = _context.Exercices
                .FirstOrDefault(s => s.SerieId == SerieId);
            Seance seance = _context.Seances
                .FirstOrDefault(o => o.ExerciceId == exo.Id);
            Serie s = _context.Series
                .FirstOrDefault(s => s.Id == SerieId);
            _context.Seances.Remove(seance);
            _context.SaveChanges();
            _context.Exercices.Remove(exo);
            _context.SaveChanges();
            _context.Series.Remove(s);
            _context.SaveChanges();
            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
        }

        [HttpPost]
        public IActionResult StartExercice(int pageIndex, int NomExoId, string date)
        {
            string dateAuj = DateTime.Now.ToString("dd.MM.yyyy");
            if (date != dateAuj)
            {
                Serie serie = new Serie();
                List<Serie> listeSerie = _context.Series.ToList();
                bool IndexNonUtiliser;
                int index = 0;
                serie.Poids = 0;
                serie.Rep = 1;
                for (int i = 1; i < 10000; i++)
                {
                    IndexNonUtiliser = listeSerie.Any(s => s.Id == i);
                    if (IndexNonUtiliser == false)
                    {
                        index = i;
                        i = 10000 - 1;
                    }
                }
                serie.Id = index;
                _context.Series.Add(serie);
                _context.SaveChanges();

                Exercice exo = new Exercice();
                exo.NomExerciceId = NomExoId;
                exo.SerieId = serie.Id;
                exo.Date = DateTime.Now.ToString("dd.MM.yyyy");
                exo.NumSerie = 1;
                List<Exercice> listeExo = _context.Exercices.ToList();
                bool IndexNonUtiliser1;
                int index1 = 0;
                for (int i = 1; i < 10000; i++)
                {
                    IndexNonUtiliser1 = listeExo.Any(s => s.Id == i);
                    if (IndexNonUtiliser1 == false)
                    {
                        index1 = i;
                        i = 10000 - 1;
                    }
                }
                exo.Id = index1;
                _context.Exercices.Add(exo);
                _context.SaveChanges();

                Seance seance = new Seance();
                seance.ExerciceId = exo.Id;
                seance.NomSeanceId = pageIndex;
                _context.Seances.Add(seance);
                _context.SaveChanges();
                return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
            }
            else 
            {
                return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
            }
        }
    }
}
