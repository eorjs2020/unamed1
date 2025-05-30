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
        // 겹치는 콜라이더를 담을 배열 생성
        Collider2D[] results = new Collider2D[1];

        // 첫 번째 BoxCollider2D의 충돌 체크
        int count = box1.OverlapCollider(new ContactFilter2D().NoFilter(), results);
        Debug.Log(count);

        // 결과 배열에 두 번째 콜라이더가 있는지 확인
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
