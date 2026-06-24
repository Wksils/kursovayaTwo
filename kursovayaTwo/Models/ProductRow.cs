using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class ProductRow
    {
        public Product Product { get; set; }
        public List<Recipe> Recipes { get; set; }      
        public List<TechCard> TechCards { get; set; }  
        public ProductionBatch Batch { get; set; }

        public string RecipeName => Recipes?.FirstOrDefault()?.RecipeId.ToString() ?? "---";
        public string TechCardName => TechCards?.FirstOrDefault()?.CardId.ToString() ?? "---";
        public string BatchNumber => Batch?.BatchNumber ?? "---";
    }
}
