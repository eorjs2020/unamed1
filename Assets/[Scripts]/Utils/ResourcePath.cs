using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public static class ResourcePath
{
    #region Mold of Item
    // InvenItem prefab
    public static readonly string InvenItemPrefab = "prefabs/Items/InvenItem";
    // CollectableItem Prefab
    public static readonly string CollectableItemPrefab = "prefabs/Items/CollectableItem";
    public static readonly string CollectableBoxPrefab = "prefabs/Items/CollectableBox";
    // CraftingUIBtn Prefab
    public static readonly string CraftingRecipeUIBtnPrefab = "prefabs/Items/CraftingRecipeUIBtn";
    // Enemy_Field Prefab
    public static readonly string EnemyFieldPrefab = "prefabs/Pawn/Enemy_Field";
    // WeaponButton for Battle
    public static readonly string WeaponButton = "prefabs/Battle/WeaponButton";

    #endregion

    #region Scritable Items path
    // Scriptable Items path

    // ItemBox for map
    public static readonly string ItemBox10 = "prefabs/Items/ScriptableObjects/ItemBox10";

    public static readonly string Key = "prefabs/Items/ScriptableObjects/Key";
    public static readonly string Blueberry = "prefabs/Items/ScriptableObjects/Blueberry";
    public static readonly string Cristal = "prefabs/Items/ScriptableObjects/Cristal";
    public static readonly string PotionBlue = "prefabs/Items/ScriptableObjects/Potion_Blue";
    public static readonly string PotionRed = "prefabs/Items/ScriptableObjects/Potion_Red";
    public static readonly string Strawberry = "prefabs/Items/ScriptableObjects/Strawberry";

    #endregion

    #region Scriptable Weapon Path
    public static readonly string Fist = "prefabs/Items/ScriptableObjects/Weapons/Fist";
    public static readonly string WoodenClub = "prefabs/Items/ScriptableObjects/Weapons/WoodenClub";
    public static readonly string YellowSword = "prefabs/Items/ScriptableObjects/Weapons/YellowSword";

    #endregion

    #region Scriptable Crafting Recipe path

    private static string CraftingRecipePath = "prefabs/Items/ScriptableObjects/CraftingRecipes/";
    public static readonly string RecipePotionRed = CraftingRecipePath + "Rec_Potion_Red";
    public static readonly string RecipePotionBlue = CraftingRecipePath + "Rec_Potion_Blue";
    #endregion

    #region    

    #endregion

    #region Pawn

    //Monster
    private static string MonsterPawnPath = "prefabs/Pawn/ScriptableObjects/Monster/";
    public static readonly string EnemyLevel1 = MonsterPawnPath + "MonsterLevel1";
    public static readonly string EnemyLevel2 = MonsterPawnPath + "MonsterLevel2";


    #endregion

    #region Monster
    //public static readonly string TestMonster = "prefabs/Items/ScriptableObjects/Monsters/TestMonster";

    #endregion

    #region Audio

    //public static readonly string BGM = "Audio/BGM/blabla...";

    #endregion

    #region Weapon Effect
    public static readonly string AttackEffect = "prefabs/Battle/AttackEffect/Effect";


    #endregion

    #region Enemy Attack Effect
    public static readonly string EnemyAttackEffect1 = "prefabs/Battle/EnemyAttackEffect/EnemyAttackEffect1";
    #endregion

    #region Effects

    public static readonly string ItemRiseEffect = "prefabs/Items/ItemRisingEffect";

    #endregion


    #region BattleEffect
    public static readonly string MissDamageEffect = "prefabs/Battle/Effect/MissDamagePrefab";
    public static readonly string DamageNumberEffect = "prefabs/Battle/Effect/DamageNumberPrefab";
    public static readonly string PercentPopEffect = "prefabs/Battle/Effect/PercentPopPrefab";
    public static readonly string MinusPercentPopEffect = "prefabs/Battle/Effect/MinusPercentPopPrefab";

    #endregion


}

[System.Serializable]
public class ItemInfo
{
    public static Dictionary<ItemLevel, List<ItemInfo>> ItemListOderedByLevel = new Dictionary<ItemLevel, List<ItemInfo>>();
    // todo need count for whole items
    public static ItemInfo ItemBox10 = new ItemInfo(ResourcePath.ItemBox10);
    public static ItemInfo Key = new ItemInfo(ResourcePath.Key);
    public static ItemInfo Blueberry = new ItemInfo(ResourcePath.Blueberry);
    public static ItemInfo Cristal = new ItemInfo(ResourcePath.Cristal);
    public static ItemInfo PotionBlue = new ItemInfo(ResourcePath.PotionBlue);
    public static ItemInfo PotionRed = new ItemInfo(ResourcePath.PotionRed);
    public static ItemInfo Strawberry = new ItemInfo(ResourcePath.Strawberry);

    

    private string path;
    [SerializeField]
    private ScriptableItem scriptableItem;

    public string Path { get => path; set => path = value; }

    public ScriptableItem ScriptableItem { get => scriptableItem; set => scriptableItem = value; }

    protected ItemInfo(string path)
    {
        Path = path;
        ScriptableItem = Resources.Load<ScriptableItem>(path);
        
        
        // 에러이유 -> 레시피인포에서 path로 ScriptableItem을 안만듬.
        // 아이탬을 타입별로 구별필요 -> 무기,소비, 기타
        // todo RecipeInfo 수정필요.
        if (ScriptableItem == null)
            return;

        ItemListOderedByLevel.TryGetValue(scriptableItem.ItemLevel, out List<ItemInfo> list);
        if (list == null)
        {
            ItemListOderedByLevel.Add(scriptableItem.ItemLevel, list = new List<ItemInfo>());
        }
        else
        {
            list.Add(this);
        }
        
    }
}

[System.Serializable]
public class WeaponInfo : ItemInfo
{
    // todo need count for whole items
    public static WeaponInfo Fist= new WeaponInfo(ResourcePath.Fist);
    public static WeaponInfo WoodenClub = new WeaponInfo(ResourcePath.WoodenClub);
    public static WeaponInfo YellowSword = new WeaponInfo(ResourcePath.YellowSword);



    protected WeaponInfo(string path) : base(path)
    {
        Path = path;
        ScriptableItem = Resources.Load<ScriptableWeapon>(path);
    }
}

[System.Serializable]
public class RecipeInfo : ItemInfo
{
    public static List<RecipeInfo> RecipeInfos = new List<RecipeInfo>();

    public static RecipeInfo recPotionRed = new RecipeInfo(ResourcePath.RecipePotionRed);
    public static RecipeInfo recPotionBlue = new RecipeInfo(ResourcePath.RecipePotionBlue);
    
    // scriptable object should inherit ScriptableItem
    public ScriptableCraftingRecipe ScriptableCraftingRecipe { get; set; }
    
    private RecipeInfo(string path) : base(path)
    {
        ScriptableItem = Resources.Load<ScriptableCraftingRecipe>(path).CratedItem;
        ScriptableCraftingRecipe = Resources.Load<ScriptableCraftingRecipe>(path);
        RecipeInfos.Add(this);
    }
}

[System.Serializable]
public class EnemyInfo
{
    public static List<EnemyInfo> EnemyList = new List<EnemyInfo>();
    public static EnemyInfo Enemy1 = new EnemyInfo(ResourcePath.EnemyLevel1);
    public static EnemyInfo Enemy2 = new EnemyInfo(ResourcePath.EnemyLevel2);

    private string path;
    public string Path { get => path; set => path = value; }
    
    [SerializeField] private ScriptableEnemy scriptableEnemy;
    public ScriptableEnemy ScriptableEnemy
    {
        get => scriptableEnemy;
        set => scriptableEnemy = value;
    }
    
    private EnemyInfo(string path)
    {
        Path = path;
        ScriptableEnemy = Resources.Load<ScriptableEnemy>(path);

        EnemyList.Add(this);
    }

}

