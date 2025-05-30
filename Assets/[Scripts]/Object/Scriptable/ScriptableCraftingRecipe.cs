using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Bandibul/CraftingRecipe")]

public class ScriptableCraftingRecipe : ScriptableObject
{
    public Defines.RecipeType RecipeType = Defines.RecipeType.None;
    public int RecipeId = Defines.ScriptableCraftingRecipeId++;
    public string ItemName;
    public string Description;
    public Sprite Sprite;
    public ScriptableItem CratedItem;
    public List<ItemInfo> Materials;
}
