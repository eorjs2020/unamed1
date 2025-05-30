using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PlayerMessageHandlerBase : MonoBehaviour
{
    [SerializeField, Tooltip("Panel for player message")]
    private GameObject messagePanel = null;
    [SerializeField, Tooltip("UI Text responsible for displaying the message.")]
    private TextMeshProUGUI messageText = null;
    [SerializeField, Tooltip("Default Message display time")]
    private float defaultMessageDuration = 3.0f;

    protected IGameManager gameMgr;

    protected void InitBase(IGameManager gameManager)
    {
        gameMgr = gameManager;

        
        //Action<object, string> TmpCheckNull = (obj, message) => { if (obj == null) Debug.LogWarning(message); };

        //// todo checking
        //TmpCheckNull(messagePanel, "Assign MessagePanel!");
        //TmpCheckNull(messageText, "Assign MessageText!");
        if (messagePanel == null)
            Debug.LogWarning($"[{GetType().Name}] MessagePanel is not assigned!!!");
        if (messageText == null)
            Debug.LogWarning($"[{GetType().Name}] MessageText is not assigned!!!");

    }


    protected void DisplayMessage(MessageEventArgs args)
    {
        messagePanel.SetActive(true);
        messageText.text = args.message;

        StartCoroutine(HideMessage(args.customDurationEnabled ? args.customDuration : defaultMessageDuration));
    }

    private IEnumerator HideMessage(float duration)
    {
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }
}
