using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FantacyRecipe.Models
{
  
  public class Tag
  {
    public int TagId { get; set; }
    [Required(ErrorMessage = "The tag's name can't be empty!")]
    public string TagName { get; set; }

    public List<RecipeTag> JoinEntities { get;}
    
    public ApplicationUser User { get; set; }
  }
}