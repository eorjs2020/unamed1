using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BarControllerUIComponent : MonoBehaviour, IUIComponent
{

    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;

    public RectTransform bar; // �̵��� ���� RectTransform
    public RectTransform area; // �ٰ� �̵��� �� �ִ� ������ RectTransform
    public Image[] segments; // 6���� Image ������Ʈ �迭 (�¿� ��Ī���� 3��)
    public Color[] colors = new Color[3]; // 3���� ���� �迭
    public float speed; // ���� �̵� �ӵ�
    [Header("[Blue, Green, Red percentage, Maximum 50]")]
    public float[] segmentPercents = new float[3]; // �� ���׸�Ʈ�� �ʺ� �����ϴ� ���� �迭 (��: 10%, 30%, 10%)

    private bool isMoving = false; // �ٰ� �̵� ������ ����


    #region BarAction2
    [SerializeField] private int buttonSize = 30; // ���� ��ư ����
    [SerializeField] private int columns = 3; // ���� ��ư ����
    [SerializeField] private int rows = 3; // ���� ��ư ����
    [SerializeField] private int specialButtonCount = 6;

    // ��ư �����հ� �׸��尡 ��ġ�� �θ� �г�
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform panel; // ��ư�� ��ġ�� �г�

    private bool gridGenerated = false; // �׸��尡 �� �� �����Ǿ����� ���θ� �����ϴ� �÷���
    #endregion

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.InputUpdatedGlobal += GlobalEvent_InputUpdatedGlobal;
        globalEvent.BarActionButtonClickedGlobal += GlobalEvent_BarActionButtonClickedGlobal;

        GenerateButtonGrid();
    }

    

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.InputUpdatedGlobal -= GlobalEvent_InputUpdatedGlobal;
        globalEvent.BarActionButtonClickedGlobal -= GlobalEvent_BarActionButtonClickedGlobal;
    }
    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {
        switch (args.battleStateType)
        {
            case BattleStateType.bar:

                SetRandomSpecialButtons();
                SetupSegments();
                InitBarAction();
                break;
        }
    }

    private void InitBarAction()
    {
        bar.anchoredPosition = Vector2.zero;
        isMoving = true;
    }

    void Update()
    {
        if (battleGameMgr.BattleState == BattleStateType.bar)
        {
            MoveBar();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopBarAndCalculateDistance();
            }
        }
    }

    private void SetupSegments()
    {
        float totalWidth = area.rect.width;
        float centerX = totalWidth / 2f;

        for (int i = 0; i < segmentPercents.Length; i++)
        {
            float segmentWidth = totalWidth * (segmentPercents[i] / 100f);

            float offsetX = centerX - (centerX - GetSegmentOffset(i));

            // Left segments
            RectTransform leftSegmentRect = segments[i * 2].GetComponent<RectTransform>();
            leftSegmentRect.sizeDelta = new Vector2(segmentWidth, area.rect.height);
            leftSegmentRect.pivot = new Vector2(0, 0.5f);
            leftSegmentRect.anchorMin = new Vector2(0, 0.5f);
            leftSegmentRect.anchorMax = new Vector2(0, 0.5f);
            leftSegmentRect.anchoredPosition = new Vector2(offsetX, 0);
            segments[i * 2].color = colors[i];

            leftSegmentRect.GetComponent<BoxCollider2D>().size = leftSegmentRect.rect.size;
            leftSegmentRect.GetComponent<BoxCollider2D>().offset = leftSegmentRect.rect.center;

            // Right segments
            RectTransform rightSegmentRect = segments[i * 2 + 1].GetComponent<RectTransform>();
            rightSegmentRect.sizeDelta = new Vector2(segmentWidth, area.rect.height);
            rightSegmentRect.pivot = new Vector2(1, 0.5f);
            rightSegmentRect.anchorMin = new Vector2(1, 0.5f);
            rightSegmentRect.anchorMax = new Vector2(1, 0.5f);
            rightSegmentRect.anchoredPosition = new Vector2(-offsetX, 0);
            segments[i * 2 + 1].color = colors[i];

            rightSegmentRect.GetComponent<BoxCollider2D>().size = leftSegmentRect.rect.size;
            rightSegmentRect.GetComponent<BoxCollider2D>().offset = new Vector2(-leftSegmentRect.rect.center.x, leftSegmentRect.rect.center.y);
            
        }
    }

    private float GetSegmentOffset(int index)
    {
        float offset = 0f;
        for (int i = 0; i < index; i++)
        {
            offset += area.rect.width * (segmentPercents[i] / 100f);
        }
        return offset;
    }

    private void MoveBar()
    {
        if (isMoving)
        {
            bar.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);

            //Debug.Log(bar.anchoredPosition.x);
            // ������ ����� �ʵ��� ����
            if (bar.anchoredPosition.x > area.rect.width)
            {
                //bar.anchoredPosition = new Vector2(area.rect.width, bar.anchoredPosition.y);
                // stop when bar is on end of area
                StopBarAndCalculateDistance();
            }
        }
    }

    private void StopBarAndCalculateDistance()
    {
        isMoving = false;

        // �߾ӿ����� �Ÿ� ���
        //float centerX = area.rect.width / 2;
        //float barCenterX = bar.anchoredPosition.x + bar.rect.width / 2;
        //float distanceFromCenter = Mathf.Abs(barCenterX - centerX);
        float centerX = area.transform.position.x;
        float barCenterX = bar.transform.position.x;
        float distanceFromCenter = Mathf.Abs(centerX - barCenterX);
        float percentageFromCenter = 1 - (distanceFromCenter / (area.rect.width / 2));

        SegmentOverlaped overlapled = SegmentOverlaped.GREEN;
        var segTag = GetCurrentSegment().tag;

        if (segTag.CompareTo("BattleBar1") == 0)
            overlapled = SegmentOverlaped.RED;
        else if (segTag.CompareTo("BattleBar2") == 0)
            overlapled = SegmentOverlaped.GREEN;
        else if (segTag.CompareTo("BattleBar3") == 0)
            overlapled = SegmentOverlaped.BLUE;
        
        Debug.Log("�ٰ� �߾ӿ��� " + percentageFromCenter + "% ������ �ֽ��ϴ�." + GetCurrentSegment().name);
        globalEvent.RaiseBarPercentageUpdateGlobal(battleGameMgr, new BarPercentageUpdateEventArgs(percentageFromCenter, overlapled));
        //battleGameMgr.SetBattleState(BattleStateType.playerAttackAnimation);
        //globalEvent.RaiseBattleStateUpdatedGlobal(battleGameMgr, new BattleStateEventArgs(BattleStateType.playerAttack));
    }

    private GameObject GetCurrentSegment()
    {
        GameObject segment = null;
        
        for (int i = 0; i < segments.Length; i++)
        {
            var bounds = segments[i].GetComponent<BoxCollider2D>().bounds;
            if (bounds.Contains(bar.transform.position))
                return segments[i].gameObject;
        }

        if (segment == null)
        {
            // if it doesn'g find segment just return first one
            segment = segments.FirstOrDefault().gameObject;
        }


        return segment;
    }

    private bool IsBarOnTheBarActionButton(BarActionButton btn)
    {
        bool isTouching = btn.GetComponent<BoxCollider2D>().IsTouching(bar.GetComponent<BoxCollider2D>());
        return isTouching;
    }

    private void GlobalEvent_InputUpdatedGlobal(IGameManager sender, InputEventArgs args)
    {
        if (battleGameMgr.BattleState == BattleStateType.bar)
        {
            //StopBarAndCalculateDistance();
        }
    }

    private void GlobalEvent_BarActionButtonClickedGlobal(IGameManager sender, BarActionButtonEventArgs args)
    {
        Debug.Log(IsBarOnTheBarActionButton(args.ClickedBtn));
        BarActionButton barActionButton = args.ClickedBtn;

        float decimalPer = 0f;
        // when click on bar
        if (barActionButton.IsSpecial && IsBarOnTheBarActionButton(barActionButton))
        {
            // show percentage UI
            decimalPer = battleGameMgr.GetService<IWeaponService>().GetDefaultAttackChanceIncrementValue();
        }
        else
        {
            decimalPer = battleGameMgr.GetService<IWeaponService>().GetDefaultAttackChanceDecrementValue();
        }
        var effectDuration = battleGameMgr.GetService<IBattleVFXService>()
                .PercentagePopEffect(args.ClickedBtn.gameObject, CustomUtility.DecimalToPercentage(decimalPer));
        // event to WeaponService for updating percentage.
        globalEvent.RaiseAttackChanceIncrementUpdateGlobal(battleGameMgr, new AttackChanceIncrementEventArgs(decimalPer));
    }

    #region BarAction2
    private void GenerateButtonGrid()
    {
        // �г��� ũ�⸦ ������ ��ư ũ�⸦ ���
        float panelWidth = panel.rect.width;
        float panelHeight = panel.rect.height;

        // ��ư�� ����, ���� ũ�� ���
        //float buttonWidth = panelWidth / columns;
        //float buttonHeight = panelHeight / rows;
        float buttonWidth = buttonSize;
        float buttonHeight = buttonSize;

        float pannelWidth = buttonSize * columns;
        float pannelHeight = buttonSize * rows;
        float xSpacing = (float)buttonSize / 3;

        // �׸��� ���̾ƿ� �׷��� ������ ���� ������ ��������, ������ �߰�
        GridLayoutGroup gridLayoutGroup = panel.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = panel.gameObject.AddComponent<GridLayoutGroup>();
        }


        // �׸��� ���̾ƿ� �׷� ���� (�� ���� ũ��� ������ ����)
        gridLayoutGroup.cellSize = new Vector2(buttonWidth, buttonHeight);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
        gridLayoutGroup.spacing = new Vector2(xSpacing, 0);
        gridLayoutGroup.padding = new RectOffset(0, 0, 0, 0); // �е� ����
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(pannelWidth + (xSpacing * (columns -1)), pannelHeight);

        // ������ �� ��ư�� �̹� ������ BarActionButton ��ũ��Ʈ�� Ȱ��
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, panel);
            newButton.name = "Button " + (i + 1); // �� ��ư�� �̸� �ο�

            // BarActionButton ��ũ��Ʈ�� prefab���� �̹� �����Ǿ� ����
            BarActionButton barActionButton = newButton.GetComponent<BarActionButton>();
            barActionButton.Init(battleGameMgr);
            barActionButton.GetComponent<BoxCollider2D>().size = new Vector2(buttonWidth, buttonHeight);
            barActionButton.IsSpecial = false; // �ʱ�ȭ �� isSpecial ���� false�� ����
        }

        // �������� ������ ��ư ����

    }
    private void SetRandomSpecialButtons()
    {
        // Ư�� ��ư ������ ���� �������� ������ �ȵ�
        int maxSpecialButtons = Mathf.Min(this.specialButtonCount, columns);

        // ��� ��ư�� isSpecial ���� �ʱ�ȭ
        foreach (Transform child in panel)
        {
            child.GetComponent<BarActionButton>().ResetButton();
        }

        int specialButtonCount = 0;
        List<int> availableIndices = new List<int>();

        // ������ ��� ��ư�� �ε����� ����Ʈ�� �߰�
        for (int i = 0; i < rows * columns; i++)
        {
            availableIndices.Add(i);
        }

        while (specialButtonCount < maxSpecialButtons)
        {
            // �������� �ε��� ����
            int randomIndex = UnityEngine.Random.Range(0, availableIndices.Count);
            int buttonIndex = availableIndices[randomIndex];

            int col = buttonIndex % columns;

            bool isThereSpecialButton = false;
            for (int i = 0; i < rows; i++)
            {
                int checkIndex = i * columns + col;
                if (panel.GetChild(checkIndex).GetComponent<BarActionButton>().IsSpecial)
                {
                    isThereSpecialButton = true;
                    break;
                }
            }

            // �ش� ���� Ư�� ��ư�� ������ ����
            if (!isThereSpecialButton)
            {
                Transform buttonTransform = panel.GetChild(buttonIndex);
                BarActionButton barActionButton = buttonTransform.GetComponent<BarActionButton>();

                barActionButton.SetAsSpecial();
                specialButtonCount++;
            }

            // ������ �ε����� ������ �ε������� ����
            availableIndices.RemoveAt(randomIndex);
        }
    }

    #endregion

}
