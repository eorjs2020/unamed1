using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputService : MonoBehaviour, IInputService
{
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;

   
    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();
    }


    // This function is called from NewInputSystem InputAction
    public void OnClick(InputAction.CallbackContext context)
    {
        // ignore event when touching UI
        if (IsPointerOverUIObject() && gameMgr.State != GameStateType.battle)
            return;
        
        if (!context.started)
            return;
        //Debug.Log(context.ReadValue<Vector2>());
        globalEvent.RaiseInputUpdatedGlobal(gameMgr, new InputEventArgs(context.ReadValue<Vector2>()));
        
        // Generates Audio event Temp
        //globalEvent.RaiseAudioUpdatedGlobal(gameMgr, new AudioEventArgs(AudioType.BGM, "Audio/BGM/bgm_test"));
        //globalEvent.RaiseAudioUpdatedGlobal(gameMgr, new AudioEventArgs(AudioType.EFFECT, "Audio/EFFECT/effect_test"));
    }
    private bool IsPointerOverUIObject()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            return results.Count > 0;
        }
        else if (Mouse.current != null)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        return false;
    }
}
