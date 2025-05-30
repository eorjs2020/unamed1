
public interface ICraftingUIHandler : IGameService
{
    public void InitCraftRecipePanel(RecipeInfo recipe);
    public void OnClickedCraftRecipeIcon(RecipeInfo recipeInfo);
    public void CraftItem(RecipeInfo itemInfo);
}
