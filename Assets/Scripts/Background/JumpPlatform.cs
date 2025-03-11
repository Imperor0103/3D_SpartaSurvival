using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 점프대(Trampoline은 에셋에서 이미 쓰고 있어서 다른 이름을 써야한다)
/// </summary>
public class JumpPlatform : MonoBehaviour
{
    // 점프력
    [SerializeField] private float jumpForce; // 점프 강도
    [SerializeField] private int jumpCount; // 점프대를 사용한 연속점프횟수
    [SerializeField] private float[] jumpForces = { 100f, 200f, 300f, 350f }; // 점프 강도 배열
    [SerializeField] private bool isOnJumpPad;
    [SerializeField] private float lastJumpExitTime;    // 마지막 점프로부터 지난 시간

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = 100f;   // 기본점프력 100
        jumpCount = 0;
        isOnJumpPad = false;
        lastJumpExitTime = 0f;
    }

    // 플레이어가 닿으면 플레이어를 위로 보낸다
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어인지 확인
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 점프대 위에 있음
                isOnJumpPad = true;

                // 점프 카운트 증가 (최대 4)
                jumpCount = Mathf.Min(jumpCount + 1, 4);
                // 점프 횟수에 따라 힘 적용
                float jumpForce = jumpForces[jumpCount - 1];


                //// 기존 속도 유지 + 새로운 점프력 추가
                //Vector3 currentVelocity = rb.velocity;
                //float newJumpSpeed = Mathf.Max(currentVelocity.y, 0) + jumpForce;
                //rb.velocity = new Vector3(currentVelocity.x, newJumpSpeed, currentVelocity.z);


                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 순간적인 힘 적용
                // 이걸 플레이어한테 줘야되네
                CharacterManager.Instance.Player.controller._rigidbody = rb;
            }
        }
    }
    // 점프대 벗어나면, 연속점프 횟수 초기화
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 점프대를 벗어나면 플래그만 변경 (초기화 X)
            isOnJumpPad = false;
            // 마지막 점프대에 닿은 시간 기록
            lastJumpExitTime = 0f; // 점프대에 다시 닿았으므로 시간을 초기화할 필요 없음
        }
    }

    private void Update()
    {
        lastJumpExitTime += Time.deltaTime;
        // 점프대에서 떨어진 후 5초가 지나면 점프 카운트 초기화
        if (lastJumpExitTime >= 5f)
        {
            jumpCount = 0;
            lastJumpExitTime = 0f;
        }
    }

}
