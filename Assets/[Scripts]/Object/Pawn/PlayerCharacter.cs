
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCharacter : Pawn
{
    [field: SerializeField] public float MaxHunger { get; set; } = 100;
    [field: SerializeField] public float CurrentHunger { get; set; } = 100;
    [field: SerializeField] public int AmountOfHpReduceOnStarved { get; set; } = 1;
    [field: SerializeField] public int AmountOfHungerReduce { get; set; } = 1;

    [field: SerializeField] public int CharacterStatStr { get; set; } = 1;
    [field: SerializeField] public int CharacterStatDex { get; set; } = 1;
    [field: SerializeField] public int CharacterStatAgi { get; set; } = 1;
    [field: SerializeField] public int CharacterStatLuck { get; set; } = 1;

    private void Start()
    {
        base.Start();

        globalEvent.BuffGlobal += UpdateCharacterStat;
    }

    private void OnDisable()
    {
        globalEvent.BuffGlobal -= UpdateCharacterStat;
    }

    private void UpdateCharacterStat(IGameManager sender, BuffEventArgs e)
    {
        switch (e.roomType)
        {
            case RoomType.Blue:
                CharacterStatStr++;
                break;
            case RoomType.Red:
                CharacterStatDex++;
                break;
            case RoomType.Green:
                CharacterStatAgi++;
                break;
            case RoomType.Yellow:
                CharacterStatLuck++;
                break;
            default:
                break;
        }
        gameMgr.GetService<IWeaponPanelUIHandler>().UpdateCharacterStatPanel();
    }


    public void UpdateHungerValue()
    {
        if (CurrentHunger > 0)
        {
            CurrentHunger -= AmountOfHungerReduce;
            if (CurrentHunger <= 0)
            {
                CurrentHunger = 0;
                OnStarved();
            }
            globalEvent.RaiseUIStatusTextUpdateGlobal(gameMgr,
                new UIStatusEventArgs(Defines.StatusType.Hunger, CurrentHunger, MaxHunger));
        }
        else
        {
            OnStarved();
        }
    }

    // Hunger value under 0
    private void OnStarved()
    {
        CurrentHp -= AmountOfHpReduceOnStarved;
        if (CurrentHp < 0) OnDead();

        globalEvent.RaiseUIStatusTextUpdateGlobal(gameMgr,
            new UIStatusEventArgs(Defines.StatusType.HP, CurrentHp, MaxHp));
        /*gameMgr.GetService<IUIService>().UpdateStatusUIText(
            Defines.StatusType.HP,
            CurrentHp, MaxHp);*/
    }

    public void OnDamaged(int damage)
    {
        CurrentHp -= damage;

        if (CurrentHp < 0) OnDead();
    }

    private void OnDead()
    {
        // TODO
        Debug.Log("Player is dead.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 collisionDirection = (other.GetComponent<Collider2D>().bounds.center - GetComponent<Collider2D>().bounds.center).normalized;

        switch (other.gameObject.tag)
        {
            #region collision with EnterDoor
            case "Room1":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM1, collisionDirection));
                break;
            case "Room2":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM2, collisionDirection));
                break;
            case "Room3":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM3, collisionDirection));
                break;
            case "Room4":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM4, collisionDirection));
                break;
            case "Room5":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM5, collisionDirection));
                break;
            case "Room6":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM6, collisionDirection));
                break;
            case "Room7":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM7, collisionDirection));
                break;
            case "Room8":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOM8, collisionDirection));
                break;
            case "Item":
                //if(other.gameObject.name)
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ITEM, collisionDirection));
                break;
            #endregion

            #region collision with ExitDoor
            case "RoomExitDoor":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.ROOMEXIT, collisionDirection));
                break;
            #endregion

            #region collision with stairs
            case "Upstairs":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.UPSTAIR, collisionDirection));
                break;
            case "Downstairs":
                globalEvent.RaiseMapCollisionGlobal(gameMgr, new MapEventArgs(MapArgType.DOWNSTAIR, collisionDirection));
                break;
            #endregion

            #region collision with enemies
            case "Enemies":
                // prevent player character keeps moving
                gameMgr.GetService<IPlayerControllerService>().TargetPosition(transform.position);
                globalEvent.RaiseBattleEventInfoGlobal(gameMgr, new BattleEventArgs(this, other.gameObject.GetComponent<Enemy>()));
                break;
                #endregion

        }
    }

    public bool IsOverlappingSomethingTilemapCollider()
    {
        LayerMask doorLayerMask = LayerMask.GetMask("DoorCollider");

        Collider2D myCollider = GetComponent<Collider2D>();

        Bounds bounds = myCollider.bounds;

        // bounds center is not same with player position in this time, I don't know why, so make bounds center artificially
        Vector3 colliderCenter = transform.position + new Vector3(myCollider.offset.x, myCollider.offset.y, 0);


        Collider2D[] hits = Physics2D.OverlapBoxAll(colliderCenter, bounds.size, 0, doorLayerMask);
        //Debug.Log(colliderCenter);
        foreach (var hit in hits)
        {
            if (hit != myCollider)
                return true;
        }
        return false;

    }
}
