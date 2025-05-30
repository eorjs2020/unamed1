using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollsionType
{
    UPSTAIR,
    DOWNSTAIR,
    ROOM1,
    ROOM2,
    ROOM3,
    ROOM4,
    ROOM5,
    ROOM6,
    ROOM7,
    ROOM8
};

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    private CollsionType collsionType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            switch(collsionType)
            {
                case CollsionType.UPSTAIR:
                    break;
                case CollsionType.DOWNSTAIR:
                    break;
                case CollsionType.ROOM1:
                    break;
                case CollsionType.ROOM2:
                    break;
                case CollsionType.ROOM3:
                    break;
                case CollsionType.ROOM4:
                    break;
                case CollsionType.ROOM5:
                    break;
                case CollsionType.ROOM6:
                    break;
                case CollsionType.ROOM7:
                    break;
                case CollsionType.ROOM8:
                    break;

            }
        }
    }
}
