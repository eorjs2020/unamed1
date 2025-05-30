using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIService : MonoBehaviour, IUIService
{
    protected IGameManager gameMgr;
    
    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
    }
}
