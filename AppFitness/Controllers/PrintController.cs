using EntitiesFitness;
using Microsoft.AspNetCore.Mvc;
using Xceed.Words.NET;

namespace AppFitness.Controllers
{
    public class PrintController : Controller
    {
        private readonly SqliteContext _context;
        public PrintController(SqliteContext context) 
        {
            _context = context;
        }
        public IActionResult ExportWord(int id)
        {
            List<Seance> listeSeance = _context.Seances
                .Where(s => s.NomSeanceId == id)
                .ToList();


            string templatePath = "Seance_Template.docx";

            using (DocX document = DocX.Create(templatePath)) 
            {
                document.InsertParagraph("Liste Séancees").FontSize(16).Bold().SpacingAfter(20);

                foreach (Seance seance in listeSeance) 
                {
                    document.InsertParagraph()
                }
            }
        }
    }
}
