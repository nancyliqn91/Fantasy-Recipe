using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FantacyRecipe.Models
{
  public class FantacyRecipeContext : IdentityDbContext<ApplicationUser>
  {

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<RecipeTag> RecipeTags { get; set; }
    
    public FantacyRecipeContext(DbContextOptions options) : base(options) { }
  }
}
