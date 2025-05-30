using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUIHandler : MonoBehaviour, IStatusUIHandler
{
    private IGameManager _gameMgr;
    private IGlobalEventService _globalEventService;
    
    [SerializeField] private TextMeshProUGUI _HPText;
    [SerializeField] private TextMeshProUGUI _HungerText;
    [SerializeField] private TextMeshProUGUI _FloorText;
    [SerializeField] private TextMeshProUGUI _RoomText;

    public void Init(IGameManager gameManager)
    {
        _gameMgr = gameManager;
        _globalEventService = _gameMgr.GetService<IGlobalEventService>();
        _globalEventService.UIStatusUpdateGlobal += UpdateStatusUIText;
        _globalEventService.MapUpdatedGlobal += _globalEventService_MapUpdatedGlobal;
    }

    public void OnDisable()
    {
        _globalEventService.UIStatusUpdateGlobal -= UpdateStatusUIText;
        _globalEventService.MapUpdatedGlobal -= _globalEventService_MapUpdatedGlobal;
    }

    private void _globalEventService_MapUpdatedGlobal(IGameManager sender, MapUpadateArg args)
    {
        _FloorText.text = "Floor : " + args.MapNumber.ToString();
        _RoomText.text = "Room : " + args.RoomNumber.ToString();
    }

    public void UpdateStatusUIText(IGameManager sender, UIStatusEventArgs e)
    {
        if (e.StatusType == Defines.StatusType.HP)
        {
            _HPText.text = $"HP : {e.CurValue} / {e.MaxValue}";
        } 
        else if (e.StatusType == Defines.StatusType.Hunger)
        {
            _HungerText.text = $"Hunger : {e.CurValue} / {e.MaxValue}";
        }
    }
}
