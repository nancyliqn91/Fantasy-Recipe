using Microsoft.AspNetCore.Mvc;
using FantacyRecipe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FantacyRecipe.Controllers
{
  [Authorize]

  public class TagsController : Controller
  {
    private readonly FantacyRecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TagsController(UserManager<ApplicationUser> userManager, FantacyRecipeContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Tag> userTags = _db.Tags
                                .Where(entry =>entry.User.Id == currentUser.Id)
                                .OrderBy(tag => tag.TagName)
                                .ToList();
      return View(userTags);
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Tag thisTag = _db.Tags
          .Include(tag => tag.JoinEntities)
          .ThenInclude(join => join.Recipe)
          .FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Tag tag)
    {
      if (!ModelState.IsValid)
      {
        return View(tag);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        tag.User = currentUser;

        _db.Tags.Add(tag);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

    }
    
    public async Task<ActionResult> AddRecipe(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      List<Recipe> userRecipes = _db.Recipes
                                .Where(e => e.User.Id == currentUser.Id)
                                .OrderBy(recipe => recipe.RecipeRate)
                                .ToList();
                                
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      ViewBag.RecipeId = new SelectList(userRecipes, "RecipeId", "RecipeName");
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult AddRecipe(Tag tag, int recipeId)
    {
      #nullable enable
      RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.RecipeId == recipeId && join.TagId == tag.TagId));
      #nullable disable
      
      if (joinEntity == null && recipeId != 0)
      {
        _db.RecipeTags.Add(new RecipeTag() { RecipeId = recipeId, TagId = tag.TagId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tag.TagId });
    }

    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult Edit(Tag tag)
    {
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      
      if (thisTag.User == currentUser)
      {
        _db.Tags.Remove(thisTag);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      RecipeTag joinEntry = _db.RecipeTags.FirstOrDefault(entry => entry.RecipeTagId == joinId);
      _db.RecipeTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
  }
}