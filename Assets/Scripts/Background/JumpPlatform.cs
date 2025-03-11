using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������(Trampoline�� ���¿��� �̹� ���� �־ �ٸ� �̸��� ����Ѵ�)
/// </summary>
public class JumpPlatform : MonoBehaviour
{
    // ������
    [SerializeField] private float jumpForce; // ���� ����
    [SerializeField] private int jumpCount; // �����븦 ����� ��������Ƚ��
    [SerializeField] private float[] jumpForces = { 100f, 200f, 300f, 350f }; // ���� ���� �迭
    [SerializeField] private bool isOnJumpPad;
    [SerializeField] private float lastJumpExitTime;    // ������ �����κ��� ���� �ð�

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = 100f;   // �⺻������ 100
        jumpCount = 0;
        isOnJumpPad = false;
        lastJumpExitTime = 0f;
    }

    // �÷��̾ ������ �÷��̾ ���� ������
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �÷��̾����� Ȯ��
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // ������ ���� ����
                isOnJumpPad = true;

                // ���� ī��Ʈ ���� (�ִ� 4)
                jumpCount = Mathf.Min(jumpCount + 1, 4);
                // ���� Ƚ���� ���� �� ����
                float jumpForce = jumpForces[jumpCount - 1];


                //// ���� �ӵ� ���� + ���ο� ������ �߰�
                //Vector3 currentVelocity = rb.velocity;
                //float newJumpSpeed = Mathf.Max(currentVelocity.y, 0) + jumpForce;
                //rb.velocity = new Vector3(currentVelocity.x, newJumpSpeed, currentVelocity.z);


                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �������� �� ����
                // �̰� �÷��̾����� ��ߵǳ�
                CharacterManager.Instance.Player.controller._rigidbody = rb;
            }
        }
    }
    // ������ �����, �������� Ƚ�� �ʱ�ȭ
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �����븦 ����� �÷��׸� ���� (�ʱ�ȭ X)
            isOnJumpPad = false;
            // ������ �����뿡 ���� �ð� ���
            lastJumpExitTime = 0f; // �����뿡 �ٽ� ������Ƿ� �ð��� �ʱ�ȭ�� �ʿ� ����
        }
    }

    private void Update()
    {
        lastJumpExitTime += Time.deltaTime;
        // �����뿡�� ������ �� 5�ʰ� ������ ���� ī��Ʈ �ʱ�ȭ
        if (lastJumpExitTime >= 5f)
        {
            jumpCount = 0;
            lastJumpExitTime = 0f;
        }
    }

}
