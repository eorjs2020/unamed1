using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarActionControllerUIComponent : MonoBehaviour
{
    // �ν����Ϳ��� ������ ��ư�� ���� �� ���� ����
    [SerializeField] private int columns = 3; // ���� ��ư ����
    [SerializeField] private int rows = 3; // ���� ��ư ����

    // ��ư �����հ� �׸��尡 ��ġ�� �θ� �г�
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform panel; // ��ư�� ��ġ�� �г�

    private bool gridGenerated = false; // �׸��尡 �� �� �����Ǿ����� ���θ� �����ϴ� �÷���

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
        // �г��� ũ�⸦ ������ ��ư ũ�⸦ ���
        float panelWidth = panel.rect.width;
        float panelHeight = panel.rect.height;

        // ��ư�� ����, ���� ũ�� ���
        float buttonWidth = panelWidth / columns;
        float buttonHeight = panelHeight / rows;

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
        gridLayoutGroup.spacing = new Vector2(0, 0); // ������ 0���� �����Ͽ� �� �´� �׸��� ����
        gridLayoutGroup.padding = new RectOffset(0, 0, 0, 0); // �е� ����

        // ��ư�� ���� x ���� ������ŭ ����
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, panel);
            newButton.name = "Button " + (i + 1); // �� ��ư�� �̸� �ο�
        }
    }
    private void SetRandomRedButtons()
    {
        List<int> availableIndices = new List<int>();

        // �ʱ� ������ ��ư �ε��� ����
        for (int i = 0; i < rows * columns; i++)
        {
            availableIndices.Add(i);
        }

        int redButtonsCount = 0;

        while (redButtonsCount < 6)
        {
            // �������� �ε��� ����
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

            // �ش� ���� ���� ��ư�� ������ ���� ����
            if (!columnAlreadyHasRedButton)
            {
                Transform buttonTransform = panel.GetChild(buttonIndex);
                Button button = buttonTransform.GetComponent<Button>();
                button.image.color = Color.red;
                redButtonsCount++;
            }

            // ������ �ε����� ������ �ε������� ����
            availableIndices.RemoveAt(randomIndex);
        }
    }



}
