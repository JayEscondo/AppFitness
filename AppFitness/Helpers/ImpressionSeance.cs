using EntitiesFitness;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Reflection.Metadata;

namespace AppFitness.Helpers
{
    public class ImpressionSeance
    {
        public static string CreateDocument(List<Seance> s, string template) 
        {
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(template);



            string save = @"G:\Github\AppFitness\Seance_Template.docx";
            document.SaveToFile(save, FileFormat.PDF);
            return  save;
        }
    }
}
