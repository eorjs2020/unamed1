using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageHandler : PlayerMessageHandlerBase, IGameMessageHandler
{

    protected IGlobalEventService globalEvent;

    public void Init(IGameManager gameManager)
    {
        base.InitBase(gameManager);
        globalEvent = gameManager.GetService<IGlobalEventService>();

        globalEvent.DisplayMessageGlobal += DisplayMessage;
        globalEvent.RaiseDisplayMessageGlobal(gameMgr, new MessageEventArgs(MessageType.warning, "---test---"));
    }

    private void OnDisable()
    {
        globalEvent.DisplayMessageGlobal -= DisplayMessage;
    }

    private void DisplayMessage(IGameManager sender, MessageEventArgs args)
    {
        base.DisplayMessage(args);
    }
}
