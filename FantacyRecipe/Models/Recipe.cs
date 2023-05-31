
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FantacyRecipe.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "The Recipe Name is the most important information!")]
    public string RecipeName { get; set; }
    public string RecipeIngredient { get; set; }
    public string RecipeInstructions {get;set;}

    [Required(ErrorMessage = "The Recipe Rate is required!")]
    public string RecipeRate { get; set; }

    public List<RecipeTag> JoinEntities { get; set; }

    public ApplicationUser User { get; set; } 
  }
}