using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MessageType { info, warning, error };

public class MessageEventArgs : EventArgs
{
    public MessageType type { get; }
    public string message { get; }
    public bool customDurationEnabled { get; }
    public float customDuration { get; }

    public MessageEventArgs(MessageType type, string message, bool customDurationEnabled = false, float customDuration = 0.0f)
    {
        this.type = type;
        this.message = message;
        this.customDurationEnabled = customDurationEnabled;
        this.customDuration = customDuration;
    }
}
