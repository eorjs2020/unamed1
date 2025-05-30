using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RecipeInfo;

public static class ObjectFactory
{

    public static T CreateObject<T>(string resourcePath) where T : UnityEngine.Object
    {
        T prefab = Resources.Load<T>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found at: " + resourcePath);
            return null;
        }
        return Object.Instantiate(prefab);
    }

    public static CollectableItem CreateCollectableItem(ItemInfo item)
    {
        // getting CollectableItemPrefab and instantiating
        CollectableItem itemPrefab =  CreateObject<CollectableItem>(ResourcePath.CollectableItemPrefab);
        // set item you wanna make
        itemPrefab.ItemInfo = item;
        //itemPrefab.ScriptableItem = Resources.Load<ScriptableItem>(item.Path);


        return itemPrefab;
    }

    public static CollectableBox CreateCollectableBox(ItemLevel boxLevel)
    {
        // getting CollectableItemPrefab and instantiating
        CollectableBox boxPrefab = CreateObject<CollectableBox>(ResourcePath.CollectableBoxPrefab);
        // set item you wanna make

        ItemInfo item = ItemInfo.ItemBox10; ;
        switch (boxLevel)
        {
            case ItemLevel._10:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._20:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._30:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._40:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._50:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._60:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._70:
                item = ItemInfo.ItemBox10;
                break;
            case ItemLevel._80:
                item = ItemInfo.ItemBox10;
                break;
        }
        boxPrefab.ItemInfo = item;
        //itemPrefab.ScriptableItem = Resources.Load<ScriptableItem>(item.Path);


        return boxPrefab;
    }

    public static ItemRiseEffect CreateItemRiseEffect(ItemInfo item)
    {
        ItemRiseEffect itemRiseEffect = CreateObject<ItemRiseEffect>(ResourcePath.ItemRiseEffect);
        itemRiseEffect.ItemInfo = item;
        return itemRiseEffect;
    }

    public static InvenItem CreateInvenItem(ItemInfo item)
    {
        // get InvenItem
        InvenItem invenItem = CreateObject<InvenItem>(ResourcePath.InvenItemPrefab);
        invenItem.ItemInfo = item;
        // set InvenItem by using ScriptableObject by ItemInfo
        //invenItem.ScriptableItem = Resources.Load<ScriptableItem>(item.Path);

        return invenItem;
    }

    public static CraftingRecipe CreateCratfingUIBtn(RecipeInfo recipeInfo)
    {
        CraftingRecipe craftingRecipe = CreateObject<CraftingRecipe>(ResourcePath.CraftingRecipeUIBtnPrefab);
        craftingRecipe.ItemInfo = recipeInfo;

        return craftingRecipe;
    }

    public static Enemy CreateFieldEnemy(EnemyInfo enemy)
    {
        Enemy enemyPrefab = CreateObject<Enemy>(ResourcePath.EnemyFieldPrefab);
        enemyPrefab.EnemyInfo = enemy;
        return enemyPrefab;
    }

    public static Enemy CreateFieldEnemyByMapLevel(EnemyLevel level)
    {
        var enemyList = EnemyInfo.EnemyList.FindAll( enemy => enemy.ScriptableEnemy.Level == level);
        EnemyInfo enemy;
        

        // todo enemy list should be full of every level later
        if(enemyList.Count <= 0)
        {
            enemy = EnemyInfo.Enemy1;
        }
        else
        {
            enemy = enemyList[Random.Range(0, enemyList.Count)];
        }
        return CreateFieldEnemy(enemy);
    }
    public static WeaponButton CreateWeaponBtn(ItemInfo item)
    {
        WeaponButton weaponButton = CreateObject<WeaponButton>(ResourcePath.WeaponButton);
        weaponButton.ItemInfo = item;

        return weaponButton;
    }

}
