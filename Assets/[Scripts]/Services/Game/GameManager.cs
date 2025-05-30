using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour, IGameManager
{
    public GameStateType State { get; private set; }
    public virtual BattleStateType BattleState { get; protected set; }


    private IGlobalEventService globalEvent;


    #region Services Publisher
    private IReadOnlyDictionary<Type, IGameService> services = null;
   
    T IServicePublisher<IGameService>.GetService<T>()
    {
        Type type = typeof(T);
        T service = default;

        if (services.TryGetValue(type, out IGameService value))
        {
            service = (T)value;
        }
        else
        {
            Debug.LogWarning($"There's no Service named {typeof(T)}!!!!!!!!!!");
        }
        return service;
    }
    #endregion

    protected void Awake()
    {
        var registeredServices = new Dictionary<Type, IGameService>();
        
        services = GetComponentsInChildren<IGameService>()
            .ToDictionary(service =>
            {
                Type serviceType = service.GetType().GetInterfaces().Where(nextInterface => !nextInterface.Equals(typeof(IGameService)))
                .FirstOrDefault(nextInterface => nextInterface.GetInterfaces().ToList().Contains(typeof(IGameService)));
                //Type serviceType = service.GetType();

                if (registeredServices.TryGetValue(serviceType, out var serviceObject))
                {
                    // there's already instance existed.
                    return null;
                }

                registeredServices.Add(serviceType, service);
                return serviceType;
            },
            service => service);


        // Initialize services.
        foreach (IGameService service in services
            .Values
            .OrderBy(service => service is IGamePriorityService ? (service as IGamePriorityService).ServicePriority : Mathf.Infinity))
        {
            service.Init(this);
            Debug.Log(service.ToString() + " initialzied.");
        }

        globalEvent = GetComponentInChildren<IGlobalEventService>();

        SetState(GameStateType.playing);
        
    }

    public void SetState(GameStateType newState)
    {
        State = newState;

        if (globalEvent != null)
            globalEvent.RaiseGameStateUpdatedGlobal(this, new GameStateEventArgs(State));
    }

    public virtual IGameManager GetMainGameManager()
    {
        return this;
    }

    public virtual void SetBattleState(BattleStateType newState, float duration = 0, BattleResult result = BattleResult.Win)
    {
       
    }
}
