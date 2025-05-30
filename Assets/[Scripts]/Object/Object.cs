using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour, IObject
{

    protected bool isInit = false;

    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;

    public virtual void Init(IGameManager gameMgr)
    {
        this.gameMgr = gameMgr;
        isInit = true;
        globalEvent = this.gameMgr.GetService<IGlobalEventService>();
    }

    protected void Start()
    {
        Invoke("CheckInitialization", 1.0f);
    }

    private void CheckInitialization()
    {
        if (!isInit)
        {
            Debug.LogWarning($"{this.gameObject.name} is not initialized!!!");
        }
    }
}
