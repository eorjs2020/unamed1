using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXService : MonoBehaviour, IVFXService
{
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;
    
    [SerializeField]
    private Image screenFadeImage;

    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();
    }

    public void ScreenFadeInOut(System.Action<int> callback, int val)
    {
        screenFadeImage.gameObject.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();

       
        sequence.Append(screenFadeImage.DOFade(1.0f, 0.2f));

       
        sequence.AppendCallback(() =>
        {
            callback?.Invoke(val);
        });

        
        sequence.Append(screenFadeImage.DOFade(0.0f, 0.2f)
            .OnComplete(() => {
                screenFadeImage.gameObject.SetActive(false);
            })); 

    }

    public void StartBattleEffect(Action callback)
    {
        screenFadeImage.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        Transform cameraTransform = Camera.main.transform;

        sequence.AppendCallback(() => GetComponent<CinemachineImpulseSource>().GenerateImpulse(2)); ;


        sequence.Join(screenFadeImage.DOFade(1.0f, 1.0f));
        sequence.AppendCallback(() =>
        {
            callback.Invoke();
        });
        sequence.Append(screenFadeImage.DOFade(0.0f, 1.0f));
        sequence.AppendCallback(() =>
        {
            //screenFadeImage.DOFade(0.0f, 5.0f);
            screenFadeImage.gameObject.SetActive(false);
        });
    }
}
