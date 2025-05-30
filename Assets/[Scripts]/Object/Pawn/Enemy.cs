using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : Pawn
{
    [SerializeField] private EnemyInfo _enemyInfo;

    public EnemyInfo EnemyInfo
    {
        get { return _enemyInfo; }
        set { _enemyInfo = value; }
    }
    
    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        
        InitSpriteAndCollider();
    }

    private void InitSpriteAndCollider()
    {
        if (_enemyInfo.ScriptableEnemy != null && _enemyInfo.ScriptableEnemy.EnemySprite != null)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = _enemyInfo.ScriptableEnemy.EnemySprite;
            PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
            
            polygonCollider.pathCount = GetComponentInChildren<SpriteRenderer>().sprite.GetPhysicsShapeCount();

            
            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                List<Vector2> path = new List<Vector2>();
                GetComponentInChildren<SpriteRenderer>().sprite.GetPhysicsShape(i, path);
                polygonCollider.SetPath(i, path.ToArray());
            }
            
            // todo changing aniamtion. It's just temparay this animator is not fit for field enemy. It's for battle
            GetComponentInChildren<Animator>().runtimeAnimatorController = _enemyInfo.ScriptableEnemy.Animator;
        }
        else
        {
            Debug.LogWarning($"{GetType().Name} Scriptable Object is not set!");
        }
    }
}
