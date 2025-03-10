using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptText : MonoBehaviour
{
    // 1인칭인 경우에 위치를 고정한다
    [SerializeField] private RectTransform promptRect;

    // 3인칭인 경우에는 마우스 커서 위치로 이동한다
    [SerializeField] private Vector3 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        promptRect = GetComponent<RectTransform>();
        // UI 요소의 초기 위치를 설정 (예시로 1인칭 모드일 때)
        //SetRectPosition();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
