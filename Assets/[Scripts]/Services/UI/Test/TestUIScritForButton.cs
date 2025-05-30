using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestUIScritForButton : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void LoadBattle()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }
}
