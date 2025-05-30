using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour, IMenuUIHandler
{
    private IGameManager gameMgr;
    private IGlobalEventService globalEventService;

    [SerializeField]
    private GameObject InventoryPannel;
    [SerializeField]
    private RectTransform contents;


    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEventService = gameMgr.GetService<IGlobalEventService>();
        InitializeAllMenu();
    }

    public void InitializeAllMenu()
    {
        InventoryPannel.SetActive(false);
    }

    public void OnMenuButton(bool val)
    {
        if (!val) gameMgr.GetService<IWeaponPanelUIHandler>().SetDetailPanelInfo(null);
        InventoryPannel.SetActive(val);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contents);
    }

}
