using EntitiesFitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFitness.MyEntities
{
    public class SeanceExercice
    {
        public Seance Seance { get; set; }
        public List<int> exo { get; set; }
    }
}
