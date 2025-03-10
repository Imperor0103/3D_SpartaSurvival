using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoard : MonoBehaviour
{
    [SerializeField] private Vector3 originPos, destPos;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        originPos = new Vector3(0, 0.5f, -22f);
        destPos = new Vector3(40f, 11f, -22f);
        transform.position = originPos; // 시작위치 설정
        targetPos = destPos; ;
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        // 도착하면 바꾸기
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            targetPos = (targetPos == destPos) ? originPos : destPos;
        }
    }
}
