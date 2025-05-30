using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If service needs intializing priority, Inherit this interface
/// </summary>
public interface IGamePriorityService
{
    int ServicePriority { get; }
}
