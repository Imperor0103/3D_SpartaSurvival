using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;   // Input Action���� �Է��� �� �޾ƿ���
    public LayerMask groundLayerMask;   // ground�� layer
    public Animator playerAnimator;
    // SŰ�� ������ ��� 180�� ȸ���� �ϰ� �������� �Ѵ�
    Vector2 moveInput;  // Ű���� �Է¹��� ����
    public bool isMovingBackward;   // S ������ ������ �ٲ���Ѵ�
    public bool isClimbing;     // Rock�� ������ ���� �� �ִ�
    Vector3 rockNormal;     // Rock�� �������� 

    // �ִϸ����͸� ���⼭ �����ϹǷ�, �÷��̾��� ���ݾִϸ��̼� ���ӽð��� ���ؾ��Ѵ�
    public float clipLength;


    [Header("Look")]
    public Transform cameraContainer;   // ī�޶� ȸ��
    public float minXLook;  // ȸ������ �ּ�
    public float maxXLook;  // ȸ������ �ִ�
    private float camCurXRot;   // Input Action���� �޾ƿ��� ���콺�� delta��
    public float lookSensitivity;  // ȸ���� �ΰ���
    private Vector2 mouseDelta;     // ���콺�� delta��

    [Header("Camera Control")]
    public Camera firstPersonCamera; // 1��Ī ī�޶�
    public Camera thirdPersonCamera; // 3��Ī ī�޶�
    public float thirdPersonDistance = 10f; // 3��Ī ī�޶� �÷��̾� �ڿ� ��ġ�� �Ÿ�
    private bool isThirdPerson;     // ���� ī�޶� 3��Ī���� ����


    // UI ����
    public bool canLock = true;
    // ó������ �κ��丮 â�� ��Ȱ��ȭ�� ���·� �����Ѵ�
    // ��, CursorMode.Locked ����(Ŀ���� ȭ�� �߾ӿ� ����)���� ����
    // �̶� canLock�� true�� �Ѵ�

    public Action inventory;    // �κ��丮 ���� ������ Toggle �޼��带 ��Ƽ� ����

    public Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;   // ���� ���� �߿� ���콺 Ŀ�� �Ⱥ��̰�

        isClimbing = false;
        isMovingBackward = false;
        isThirdPerson = false;
        // ĳ��
        cameraContainer = Helper.FindChild(gameObject.transform, "CameraController");
        clipLength = Helper.GetAnimationClipLength(playerAnimator, "PlayerMeleeAttack");
        firstPersonCamera = Helper.FindChild(cameraContainer.transform, "MainCamera").GetComponent<Camera>();
        thirdPersonCamera = Helper.FindChild(cameraContainer.transform, "ThirdPersonCamera").GetComponent<Camera>();
        firstPersonCamera.gameObject.SetActive(true);   // 1��Ī ����
        thirdPersonCamera.gameObject.SetActive(false);
    }
    // ���������� FixedUpdate���� ȣ��
    void FixedUpdate()
    {
        Move();
    }
    // ī�޶󿬻��� LateUpdate���� ȣ��
    private void LateUpdate()
    {
        if (canLock)
        {
            CameraLook();
        }
    }
    // ���� �̵�
    void Move()
    {
        if (isClimbing)
        {
            // ���� ������ ���� (ǥ�� ����)
            Vector3 wallRight = Vector3.Cross(rockNormal, Vector3.up).normalized;

            // ���� ���� ���� (ǥ�� ����)
            Vector3 wallUp = Vector3.Cross(wallRight, rockNormal).normalized;

            // �Է� ���� (A/D = �¿�, W/S = ����)
            Vector3 moveDirection = (wallRight * curMovementInput.x) + (wallUp * curMovementInput.y);
            moveDirection *= moveSpeed;

            _rigidbody.velocity = moveDirection;  // Ŭ���̹� �� �̵� ����
        }
        else // �Ϲ� �̵�
        {
            Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
            dir *= moveSpeed;
            dir.y = _rigidbody.velocity.y; // ������ ���� ������ ���Ʒ��� �������� �ϹǷ� y���� �ӵ� �ʱ�ȭ

            _rigidbody.velocity = dir;
        }
    }
    // ī�޶� ȸ��
    void CameraLook()
    {
        // ī�޶� ���� ȸ��
        camCurXRot += mouseDelta.y * lookSensitivity;   /// ����ȸ���� �ϱ� ���ؼ��� y���� x�� �ִ´�
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        // ī�޶� ȸ������ ������ǥ�� ����ϴ� ������ �÷��̾ ������ �Ǳ� ����
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  /// -�� �ϴ� ����: ���콺�� ������ �Ʒ��� ȸ���ϰ� ����� ����

        // ī�޶� �¿� ȸ��
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); /// �¿�ȸ���� �ϱ� ���ؼ��� x���� y�� �ִ´�
    }
    // �Է� �̺�Ʈ ó��
    public void OnMove(InputAction.CallbackContext context)
    {
        
        // SŰ�� ���ȴٸ� �ڷ� �ٶ󺻴�
        Vector2 moveInput = context.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (context.phase == InputActionPhase.Performed)  // Ű�� ��� ������ ����
        {
            // �̵� �ִϸ��̼�
            playerAnimator.SetBool("Moving", true);
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)    // Ű�� ���� ��
        {
            // ���� �ִϸ��̼�
            playerAnimator.SetBool("Moving", false);
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();  // ���콺 ���� �Է����� �ʾƵ� ��� �����ȴ�
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())  // Ű ������ �����Ҷ�
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);  // ������ ���������� �ö󰡾��ϹǷ� impurse 
        }
    }
    // �Ʒ��� ray�� ��� ���� ��Ҵ��� Ȯ��(�ߺ������� ����)
    bool IsGrounded()
    {
        // �÷��̾� ���� å��ٸ� 4�� �����
        Ray[] rays = new Ray[4]
        {
            // �÷��̾�� (transform.up * 0.01f) ������ ��� ����:
            // �÷��̾�� ���, �÷��̾ ���� �ε��� ��� ground���� �� ���� �־, ground�� ���� ���ϴ� ��찡 �߻�

            // z��(forward) ��,�� �ణ ������ ������ �Ʒ��������� �߻�
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // x��(right) ��,�� �ణ ������ ������ �Ʒ��������� �߻�
            new Ray(transform.right + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.right + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        // ���� ��� ray�� ����
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                // 4���� ray�߿��� �ϳ��� ground�� layer ����Ǿ��ٸ�
                return true;
            }
        }
        return false;
    }
    // tabŰ ������ ������
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            /// UIInventory�� Toggle �޼��带 ����ϱ� ���� delegate�� ���
            inventory?.Invoke();    // delegate�� Toggle �޼��尡 ������ ȣ��
            ToggleCursor();
        }
    }
    /// <summary>
    /// ���������� Cursor�� Toggle���ִ� ���
    /// �κ��丮�� ���� ���� Ŀ���� ȭ�� �߾ӿ� �����Ǹ�, ������ �ʴ´�
    /// �κ��丮�� �������� ȭ���� �����ϰ�, �κ��丮�� Ŭ������ Ŀ���� ���ͼ� ȭ�� ��ü�� ������ �� �ִ�
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    /// Locked: �κ��丮â�� ���� ������ ���� ����(Ŀ���� ȭ�� �߾ӿ� �����Ǿ��ִ�)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // toggle�� true: �κ��丮â�� ������ �ʾƼ� Ŀ���� ȭ�� �߾ӿ� ���� -> None���� ����, ȭ�鿡�� ������ �� �ְ� �����
        // toggle�� false: �κ��丮â�� �����ִٸ� Ŀ���� ȭ�鿡�� ������ �� �ִ� -> Locked�� ���� ȭ�� �߾ӿ� Ŀ���� �����Ѵ�

        canLock = !toggle;
        // toggle�� true: ������ Ŀ���� ȭ�鿡�� ������ �� �ְ� ��������Ƿ� canLock�� false
        // toggle�� false: ������ ȭ�� �߾ӿ� Ŀ���� ���������Ƿ� canLock�� true
    }
    // v Ű ������ ī�޶� ��ü
    public void OnCamChange(InputAction.CallbackContext context)
    {
        if (isThirdPerson)
        {
            // 1��Ī ī�޶� Ȱ��ȭ
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
        }
        else
        {
            // 3��Ī ī�޶� Ȱ��ȭ
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
        }

        // ���� ���
        isThirdPerson = !isThirdPerson;
    }
    // Rock�� ������ �ִϸ��̼� �ٲٰ� ��Ÿ�� ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            isClimbing = true;
            playerAnimator.SetBool("Climbing", true);
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero; // ���� ��� ����
        }
    }
    // ��Ÿ�� ������
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock") && isClimbing)
        {
            // �浹�� ǥ���� ���� ���͸� ��´�
            rockNormal = collision.contacts[0].normal;

        }
    }
    // ��Ÿ�� ��
    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Rock"))
        {
            isClimbing = false;
            playerAnimator.SetBool("Climbing", false);
            _rigidbody.useGravity = true;
        }
    }
}
