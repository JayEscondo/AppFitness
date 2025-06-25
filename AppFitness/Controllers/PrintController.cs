using AppFitness.Helpers;
using EntitiesFitness;
using EntityFitness.MyEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace AppFitness.Controllers
{
    public class PrintController : Controller
    {
        private readonly SqliteContext _context;

        public PrintController(SqliteContext context)
        {
            _context = context;
        }

        public IActionResult PrintListeSeance(int NomSeanceId) 
        {
            List<Seance> listeS = _context.Seances
                .Include(n => n.NomSeance)
                .Include(e => e.Exercice)
                .Include(en => en.Exercice.NomExercice)
                .Include(s => s.Exercice.Serie)
                .Where(i => i.NomSeanceId == NomSeanceId)
                .ToList();
            string physicalPath = ImpressionSeance.CreateDocument(listeS, @"G:\\Github\\AppFitness\\Seance_Template.docx");
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "Seance/pdf");
        }
    }
}
