using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Pawn : Object
{
    [field: SerializeField] public float MaxHp { get; set; } = 100;
    [field: SerializeField] public float CurrentHp { get; set; } = 100;
    [field: SerializeField] public float MoveSpeed { get; set; } = 5;

    [SerializeField] private bool _isLeft = false;
    [SerializeField] private bool _isMoving = false;

    [SerializeField] private SpriteRenderer _pawnSpriteRenderer;
    [SerializeField] private Animator _pawnAnimator;
    
    public bool IsLeft
    {
        set
        {
            if (_isLeft != value)
            {
                _isLeft = value;
                _pawnSpriteRenderer.flipX = value;
            }
        }
    }

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
            _pawnAnimator.SetBool("IsMoving", value);
        }
    }

    public void Damaged(float val)
    {
        CurrentHp -= val;
        if (CurrentHp < 0)
            CurrentHp = 0;
    }
}
