using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages
{
    [Authorize]
    public class AddEditRecipeModel : PageModel
    {
        private readonly IRecipesService recipesService;

        [FromRoute]
        public long? Id { get; set; }

        public bool IsNewRecipe
        {
            get { return Recipe == null; }
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        public async Task OnGet()
        {
            Recipe = await recipesService.FindAsync(Id.GetValueOrDefault());
            ViewData["Title"] = IsNewRecipe ? "New Recipe" : "Edit Recipe";
        }

        public async Task<IActionResult> OnPost()
        {
            var recipe = await recipesService.FindAsync(Id.GetValueOrDefault());

            if(await TryUpdateModelAsync(recipe, nameof(Recipe),
                _ => _.DisplayName != "Image"))
            {
                await recipesService.SaveAsync(recipe);
                return RedirectToPage("/Recipe", new { id = recipe.Id });
            }

            return Page();
        }
    }
}