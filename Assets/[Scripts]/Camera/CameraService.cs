using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraService : ICameraService
{
    #region GameManager
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;
    #endregion


    public void Init(IGameManager gameManager)
    {
        throw new System.NotImplementedException();
    }

    
}
