using Microsoft.AspNetCore.Mvc;
using RecipeMicroservice.Models;
using RecipeMicroservice.Services;

namespace RecipeMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpPost("GetByLotID")]
        public async Task<IActionResult> GetRecipeByLotID([FromBody] FormGetRecipeByLotID req)
        {
            var recipe = await _recipeService.GetRecipeByLotIdAsync(req);
            return Ok(recipe);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertRecipe([FromBody] FormInsertRecipe req)
        {
            var result = await _recipeService.InsertRecipeAsync(req);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateRecipe([FromBody] FormUpdateRecipe req)
        {
            var result = await _recipeService.UpdateRecipeAsync(req);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRecipe([FromBody] UnflagRecipe req)
        {
            var result = await _recipeService.DeleteRecipeAsync(req);
            return Ok(result);
        }
    }
}
