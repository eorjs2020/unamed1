using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Defines.RecipeType;

public class CraftingUIHandler : MonoBehaviour, ICraftingUIHandler
{
    [SerializeField] private GameObject _craftingPanel;
    [SerializeField] private RecipeInfo _recipeInfo;
    [SerializeField] private Image _craftingItem;
    [SerializeField] private List<Image> _craftingMaterialSprites = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> _categories;
    [SerializeField] private List<GameObject> _craftingPanels;

    private int prvPanelIndex = 0;

    protected IGameManager gameMgr;

    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        
        // TODO Temporary For Testing
        _categories[0].text = Consumable.ToString();
        _categories[1].text = Weapon.ToString();
        _categories[2].text = Misc.ToString();
        InitCraftRecipePanel(RecipeInfo.recPotionRed);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
        InitCraftRecipePanel(RecipeInfo.recPotionBlue);
    }

    public void InitCraftRecipePanel(RecipeInfo recipe)
    {
        Defines.RecipeType recipeType = recipe.ScriptableCraftingRecipe.RecipeType;
        switch (recipeType)
        {
            case Consumable:
                InitCraftItemIcon(recipe, _craftingPanels[0]);
                break;
            case Weapon:
                InitCraftItemIcon(recipe, _craftingPanels[1]);
                break;
            case Misc:
                InitCraftItemIcon(recipe, _craftingPanels[2]);
                break;
            default:
                break;
        }
    }

    private void InitCraftItemIcon(RecipeInfo recipe, GameObject craftingPanel)
    {
        CraftingRecipe recipeUIBtn = ObjectFactory.CreateCratfingUIBtn(recipe);
        recipeUIBtn.Init(gameMgr);
        recipeUIBtn.transform.SetParent(craftingPanel.GetComponent<ScrollRect>().content.transform, false);
    }

    public void ShowRecipes(int newIndex)
    {
        //Debug.LogWarning($"prvPanelIndex={prvPanelIndex}, newIndex={newIndex}");
        _craftingPanels[prvPanelIndex].gameObject.SetActive(false);
        _craftingPanels[newIndex].gameObject.SetActive(true);
        prvPanelIndex = newIndex;
    }

    public void OnClickedCraftRecipeIcon(RecipeInfo recipeInfo)
    {
        _recipeInfo = recipeInfo;
        _craftingItem.sprite = recipeInfo.ScriptableItem.Sprite;
        
        int requiredMaterialCount = recipeInfo.ScriptableCraftingRecipe.Materials.Count;
        
        for (int index = 0; index < _craftingMaterialSprites.Count; index++)
        {
            if (index < requiredMaterialCount)
            {
                _craftingMaterialSprites[index].sprite =
                    recipeInfo.ScriptableCraftingRecipe.Materials[index].ScriptableItem.Sprite;
            }
            else
            {
                _craftingMaterialSprites[index].sprite = null;
            }
        }
    }

    public void OnClickedCraftBtn()
    {
        if (CanBeCrafted()) CraftItem(_recipeInfo);
    }
    
    public void CraftItem(RecipeInfo recipeInfo)
    {
        gameMgr.GetService<IInventoryService>().AddItemToInventory(recipeInfo, 1);
    }

    private bool CanBeCrafted()
    {
        Dictionary<InvenItem, int> inventoryItemDic = new Dictionary<InvenItem, int>();
        List<InvenItem> invenItems = gameMgr.GetService<IInventoryUIHandler>().GetInvenItems();

        foreach (var material in _recipeInfo.ScriptableCraftingRecipe.Materials)
        {
            // Check Individual Material Availability in Inventory
            // If it returns an InvenItem, it's available to craft
            // However, it's necessary to check the availability of every material before proceeding with crafting
            // So, processing data is stored in a Dictionary.
            InvenItem invenItem = CheckIndividualMaterial(material, invenItems);
            if (invenItem != null)
            {
                // Player has a material at Inventory 
                if (inventoryItemDic.ContainsKey(invenItem))
                {
                    int value = inventoryItemDic[invenItem];
                    value++;
                    inventoryItemDic[invenItem] = value;
                }
                else
                {
                    inventoryItemDic.Add(invenItem, 1);
                }
            }
            else
            {
                // Player doesn't have a material in the inventory or doesn't have enough.
                Debug.Log($"NOT satisfied craft requirement.");
                return false;
            }
        }
        
        // Inventory materials counting process setup 
        foreach (KeyValuePair<InvenItem,int> keyValuePair in inventoryItemDic)
        {
            keyValuePair.Key.SetCount(keyValuePair.Key.GetCount() - keyValuePair.Value);
        }
        return true;
    }

    private InvenItem CheckIndividualMaterial(ItemInfo itemInfo, List<InvenItem> invenItems)
    {
        foreach (var invenItem in invenItems)
        {
            if (itemInfo.ScriptableItem.ItemId == invenItem.ItemInfo.ScriptableItem.ItemId)
            {
                /*Debug.Log("=============================");
                Debug.Log($"Material Item Name {itemInfo.ScriptableItem.ItemName}");
                Debug.Log($"Material Item ID {itemInfo.ScriptableItem.ItemId}");
                Debug.Log($"Inven Item Name {invenItem.ItemInfo.ScriptableItem.ItemName}");
                Debug.Log($"Inven Item ID {invenItem.ItemInfo.ScriptableItem.ItemId}");*/

                if (invenItem.GetCount() > 0)
                {
                    return invenItem;
                }
                else
                {
                    Debug.Log($"Not enough material.");
                    return null;
                }
            }
        }
        return null;
    }
}
