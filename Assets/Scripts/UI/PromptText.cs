using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptText : MonoBehaviour
{
    // 1��Ī�� ��쿡 ��ġ�� �����Ѵ�
    [SerializeField] private RectTransform promptRect;

    // 3��Ī�� ��쿡�� ���콺 Ŀ�� ��ġ�� �̵��Ѵ�
    [SerializeField] private Vector3 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        promptRect = GetComponent<RectTransform>();
        // UI ����� �ʱ� ��ġ�� ���� (���÷� 1��Ī ����� ��)
        //SetRectPosition();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
