using Microsoft.AspNetCore.Mvc;
using FantacyRecipe.Models;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FantacyRecipe.Controllers
{
    public class HomeController : Controller
    {

      private readonly FantacyRecipeContext _db;
      private readonly UserManager<ApplicationUser> _userManager;

      public HomeController(UserManager<ApplicationUser> userManager, FantacyRecipeContext db)
      {
        _userManager = userManager;
        _db = db;
      }

      [HttpGet("/")]
      public ActionResult Index()
      {
        Recipe[] rec = _db.Recipes.OrderByDescending(rec => rec.RecipeRate).ToArray();
        // Tag[] tag = _db.Tags.OrderBy(tag  => tag.TagName).ToArray();
        Dictionary<string,object[]> model = new Dictionary<string, object[]>();
                
        // string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        
        // Recipe[] recipes = _db.Recipes.ToArray();
        model.Add("recipes", rec);
          
        // Tag[] tags = _db.Tags
        //                 .Where(entry => entry.User.Id == currentUser.Id)
        //                 .ToArray();
        // model.Add("tags", tags);
                
        return View(model);
      }      

    }
}