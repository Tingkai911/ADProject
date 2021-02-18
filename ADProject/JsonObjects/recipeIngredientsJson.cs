using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.JsonObjects
{
    public class recipeIngredientsJson
    {
        public recipeIngredientsJson() { }
        public List<string> ingredients { get; set; }

        public List<string> corpus_allergen { get; set; }

        public List<string> corpus_ingredient { get; set; }
    }
}
