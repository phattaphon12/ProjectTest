using RecipeMicroservice.Models;
using RecipeMicroservice.Repositoties;

namespace RecipeMicroservice.Services
{
    public class RecipeService
    {
        private readonly RecipeRepository _recipeRepo;

        public RecipeService(RecipeRepository recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _recipeRepo.GetAllRecipesAsync();
        }

        public async Task<Recipe?> GetRecipeByLotIdAsync(FormGetRecipeByLotID req)
        {
            return await _recipeRepo.GetRecipeByLotIdAsync(req);
        }

        public async Task<int> InsertRecipeAsync(FormInsertRecipe req)
        {
            return await _recipeRepo.InsertRecipeAsync(req);
        }

        public async Task<int> UpdateRecipeAsync(FormUpdateRecipe req)
        {
            return await _recipeRepo.UpdateRecipeAsync(req);
        }

        public async Task<int> DeleteRecipeAsync(UnflagRecipe req)
        {
            return await _recipeRepo.DeleteRecipeAsync(req);
        }
    }
}
