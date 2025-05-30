using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRiseEffectHelper : MonoBehaviour
{
    public void FinishAnimationEvent()
    {
        Destroy(transform.parent.gameObject);
    }
}
