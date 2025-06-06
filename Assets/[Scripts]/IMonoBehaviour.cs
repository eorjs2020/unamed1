using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IMonoBehaviour
{
    bool enabled { set; get; }

    Transform transform { get; }
    GameObject gameObject { get; }

    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine routine);
    void StopCoroutine(IEnumerator routine);

    T GetComponent<T>();
    T GetComponentInChildren<T>();
    Component GetComponent(Type type);
    Component GetComponentInChildren(Type type);
}
