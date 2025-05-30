using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
   public BoxCollider2D image;
   public void OnClick()
    {
       Debug.Log(this.GetComponent<BoxCollider2D>().IsTouching(image));
    }
    bool IsOverlapping(BoxCollider2D box1, BoxCollider2D box2)
    {
        // ��ġ�� �ݶ��̴��� ���� �迭 ����
        Collider2D[] results = new Collider2D[1];

        // ù ��° BoxCollider2D�� �浹 üũ
        int count = box1.OverlapCollider(new ContactFilter2D().NoFilter(), results);
        Debug.Log(count);

        // ��� �迭�� �� ��° �ݶ��̴��� �ִ��� Ȯ��
        foreach (var result in results)
        {
            if (result == box2)
            {
                return true;
            }
        }

        return false;
    }
}
