using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarActionButton : MonoBehaviour, IUIComponent
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;

    //public bool isSpecial = false;
    public bool IsSpecial { get; set; }
    private bool isClicked = false;

    private Button button;

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();
        button = GetComponent<Button>();


    }
    public void SetAsSpecial()
    {
        IsSpecial = true;
        button.image.color = Color.red; // 예시로 빨간색을 설정
    }

    public void ResetButton()
    {
        IsSpecial = false;
        isClicked = false;
        button.image.color = Color.gray; // 기본 색상으로 리셋

    }

    public void OnClickActionButton()
    {
        if (isClicked)
            return;
        globalEvent.RaiseBarActionButtonClickedGlobal(battleGameMgr, new BarActionButtonEventArgs(this));
        ExcuteClickSequence();
    }

    private void ExcuteClickSequence()
    {
        isClicked = true;
        button.image.color = Color.gray;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isClicked && IsSpecial && collision.tag == "BattleMovingBar")
        {
            Debug.Log(collision.gameObject.name);
            OnClickActionButton();
        }
    }
}
