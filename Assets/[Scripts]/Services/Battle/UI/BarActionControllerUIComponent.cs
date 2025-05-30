using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarActionControllerUIComponent : MonoBehaviour
{
    // 인스펙터에서 설정할 버튼의 가로 및 세로 갯수
    [SerializeField] private int columns = 3; // 가로 버튼 갯수
    [SerializeField] private int rows = 3; // 세로 버튼 갯수

    // 버튼 프리팹과 그리드가 배치될 부모 패널
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform panel; // 버튼이 배치될 패널

    private bool gridGenerated = false; // 그리드가 한 번 생성되었는지 여부를 추적하는 플래그

    private void Start()
    {
        if (!gridGenerated)
        {
            GenerateButtonGrid();
            SetRandomRedButtons();
            gridGenerated = true;
        }
    }

    private void GenerateButtonGrid()
    {
        // 패널의 크기를 가져와 버튼 크기를 계산
        float panelWidth = panel.rect.width;
        float panelHeight = panel.rect.height;

        // 버튼의 가로, 세로 크기 계산
        float buttonWidth = panelWidth / columns;
        float buttonHeight = panelHeight / rows;

        // 그리드 레이아웃 그룹이 있으면 기존 설정을 가져오고, 없으면 추가
        GridLayoutGroup gridLayoutGroup = panel.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = panel.gameObject.AddComponent<GridLayoutGroup>();
        }

        // 그리드 레이아웃 그룹 설정 (각 셀의 크기와 여백을 설정)
        gridLayoutGroup.cellSize = new Vector2(buttonWidth, buttonHeight);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
        gridLayoutGroup.spacing = new Vector2(0, 0); // 간격을 0으로 설정하여 딱 맞는 그리드 생성
        gridLayoutGroup.padding = new RectOffset(0, 0, 0, 0); // 패딩 설정

        // 버튼을 가로 x 세로 갯수만큼 생성
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, panel);
            newButton.name = "Button " + (i + 1); // 각 버튼에 이름 부여
        }
    }
    private void SetRandomRedButtons()
    {
        List<int> availableIndices = new List<int>();

        // 초기 가능한 버튼 인덱스 설정
        for (int i = 0; i < rows * columns; i++)
        {
            availableIndices.Add(i);
        }

        int redButtonsCount = 0;

        while (redButtonsCount < 6)
        {
            // 무작위로 인덱스 선택
            int randomIndex = Random.Range(0, availableIndices.Count);
            int buttonIndex = availableIndices[randomIndex];

            int col = buttonIndex % columns;

            bool columnAlreadyHasRedButton = false;
            for (int i = 0; i < rows; i++)
            {
                int checkIndex = i * columns + col;
                if (panel.GetChild(checkIndex).GetComponent<Button>().image.color == Color.red)
                {
                    columnAlreadyHasRedButton = true;
                    break;
                }
            }

            // 해당 열에 빨간 버튼이 없으면 색상 변경
            if (!columnAlreadyHasRedButton)
            {
                Transform buttonTransform = panel.GetChild(buttonIndex);
                Button button = buttonTransform.GetComponent<Button>();
                button.image.color = Color.red;
                redButtonsCount++;
            }

            // 선택한 인덱스를 가능한 인덱스에서 제거
            availableIndices.RemoveAt(randomIndex);
        }
    }



}
