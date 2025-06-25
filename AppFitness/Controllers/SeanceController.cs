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
        public IActionResult AddExercice(int id) 
        {
            Seance seance = _context.Seances
                .Include(c => c.NomSeance)
                .Include(c => c.Exercice)
                .Include(ne => ne.Exercice.NomExercice)
                .Include(r => r.Exercice.Serie)
                .FirstOrDefault(n => n.NomSeanceId == id);
            return PartialView("_AddExercicePartial", seance);
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
            //if (s.Id == 1)
            //{
            //    Serie s1 = new Serie();
            //    s1.Poids = s.Poids;
            //    s1.Rep = s.Rep;
            //    _context.Add(s1);
            //    _context.SaveChanges();
            //    List<Serie> listeSerie = _context.Series.ToList();
            //    Serie s2 = new Serie();
            //    List<Exercice> listeExo = _context.Exercices.ToList();
            //    for (int i = 0; i < listeSerie.Count;) 
            //    {
            //        if (listeSerie[i].Poids == s1.Poids && listeSerie[i].Rep == s1.Rep)
            //        {
            //            s2 = listeSerie[i];
            //        }
            //        else 
            //        {
            //            i++;
            //        }
            //        if (s2.Id != 0) 
            //        {
            //            for (int a = 0; a < listeExo.Count; a++)
            //            {
            //                if (s2.Id == listeExo[a].SerieId)
            //                {
            //                    s2 = null;
            //                    i++;
            //                    a = listeExo.Count - 1;
            //                }
            //            }
            //        }
            //        if (s2.Id != 0) 
            //        {
            //            i = listeSerie.Count;
            //        }
            //    }
            //    Seance seance = _context.Seances
            //        .Include(c => c.NomSeance)
            //        .Include(c => c.Exercice)
            //        .Include(ne => ne.Exercice.NomExercice)
            //        .FirstOrDefault(i => i.NomSeanceId == pageIndex);
            //    Exercice newExo = new Exercice();
            //    Exercice exo1 = new Exercice();
            //    newExo.SerieId = s2.Id;
            //    newExo.NumSerie = numSerie;
            //    newExo.NomExerciceId = seance.Exercice.NomExerciceId;
            //    newExo.Date = DateTime.Now.ToString("dd/MM/yyyy");
            //    _context.Exercices.Add(newExo);
            //    _context.SaveChanges();
            //    listeExo = _context.Exercices
            //        .ToList();
            //    List<Seance> listeSeance = _context.Seances.ToList();
            //    for (int i = 0; i < listeExo.Count;) 
            //    {
            //        if (newExo.SerieId == listeExo[i].SerieId) 
            //        {
            //            exo1 = listeExo[i];
            //        }
            //        else { i++; }
            //        if (exo1.Id != 0) 
            //        {
            //            i = listeExo.Count;
            //        }
            //    }
            //    Seance newSeance = new Seance();
            //    newSeance.NomSeanceId = seance.NomSeanceId;
            //    newSeance.ExerciceId = exo1.Id;
            //    _context.Seances.Add(newSeance);
            //    _context.SaveChanges();
            //    return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
            //}
            //else 
            //{
            //    Exercice exo = _context.Exercices.FirstOrDefault(n=>n.SerieId == s.Id);
            //    exo.NumSerie = numSerie;
            //    _context.Exercices.Update(exo);
            //    _context.Series.Update(s);
            //    _context.SaveChanges();
            //    return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
            //}
            Exercice exo = _context.Exercices.FirstOrDefault(n => n.SerieId == s.Id);
            exo.NumSerie = numSerie;
            _context.Exercices.Update(exo);
            _context.Series.Update(s);
            _context.SaveChanges();
            return RedirectToAction("DetailSeance", "Seance", new { id = pageIndex });
        }
    }
}
