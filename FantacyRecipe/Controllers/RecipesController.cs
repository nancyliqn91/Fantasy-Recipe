using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FantacyRecipe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FantacyRecipe.Controllers
{
  [Authorize]

  public class RecipesController : Controller
  {
    private readonly FantacyRecipeContext _db;

    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, FantacyRecipeContext db)
    {
      _userManager = userManager;

      _db = db;
    }
    
    [AllowAnonymous]
    public ActionResult Index()
    {
      List<Recipe> allRecipes = _db.Recipes
      .OrderBy(recipe => recipe.RecipeRate).ToList();                            
      return View(allRecipes);
    }

    public async Task<ActionResult> MyRecipes()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      List<Recipe> userRecipes = _db.Recipes
      .Where(entry => entry.User.Id == currentUser.Id)
      .OrderBy(recipe => recipe.RecipeRate).ToList();                            
      return View(userRecipes);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe)
    {
      if (!ModelState.IsValid)
      {
        return View(recipe);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        recipe.User = currentUser;

        _db.Recipes.Add(recipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      
    }
   
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Recipe thisRecipe = _db.Recipes
                             .Include(recipe => recipe.JoinEntities)
                             .ThenInclude(join => join.Tag)
                             .FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(thisRecipe);
    }

    public async Task<ActionResult> Edit(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      if (thisRecipe.User == currentUser)
      {
        return View(thisRecipe);
      }
      else
      {
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public ActionResult Edit(Recipe recipe)
    {
      _db.Recipes.Update(recipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      if (thisRecipe.User == currentUser)
      {
        return View(thisRecipe);
      }
      else
      {
        return RedirectToAction("Index");
      }
      
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      _db.Recipes.Remove(thisRecipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddTag(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      List<Tag> userTags = _db.Tags
                                .Where(entry =>entry.User.Id == currentUser.Id)
                                .OrderBy(tag => tag.TagName)
                                .ToList();

      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      
      ViewBag.TagId = new SelectList(userTags, "TagId", "TagName");
      
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddTag(Recipe recipe, int tagId)
    {
      #nullable enable
      RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.TagId == tagId && join.RecipeId == recipe.RecipeId));
      #nullable disable

      if (joinEntity == null && tagId != 0)
      {
        _db.RecipeTags.Add(new RecipeTag() { TagId = tagId, RecipeId = recipe.RecipeId });
        _db.SaveChanges();
      }

      return RedirectToAction("Details", new { id = recipe.RecipeId });
    }  

    [HttpPost]
    public async Task<ActionResult> DeleteJoin(int joinId)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      RecipeTag joinEntry = _db.RecipeTags.FirstOrDefault(entry => entry.RecipeTagId == joinId);
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(entry => entry.RecipeId == joinEntry.RecipeId);
      if (thisRecipe.User == currentUser)
      {
        _db.RecipeTags.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
    }

    public ActionResult Search()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Search(Recipe input)
    {
//  List<Recipe> model = _db.Recipes.Where(recipe => recipe.Contains(recipe.Re == input.RecipeIngredient)).ToList();                                  

      string inputString = input.RecipeIngredient;
      List<Recipe> recipeList = _db.Recipes
                                  .Where(recipe => recipe.RecipeIngredient
                                  .Contains(inputString))
                                  .ToList();    
      
      if (recipeList.Count != 0)
      {
        return RedirectToAction("Result", recipeList);
      }
      
      else
      {
        return RedirectToAction("NoResult");
      }
      
    }

    public ActionResult NoResult()
    {
      return View();
    }

    public ActionResult Result(List<Recipe> recipeList)
    {
      return View(recipeList);
    }
    
  }
}
 
