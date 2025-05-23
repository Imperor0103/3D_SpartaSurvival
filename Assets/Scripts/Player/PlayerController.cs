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
    private Vector2 curMovementInput;   // Input Action에서 입력한 값 받아오기
    public LayerMask groundLayerMask;   // ground의 layer
    public Animator playerAnimator;
    // S키가 눌렸을 경우 180도 회전을 하고 움직여야 한다
    Vector2 moveInput;  // 키보드 입력방향 저장
    public bool isMovingBackward;   // S 누르면 방향을 바꿔야한다
    public bool isClimbing;     // Rock에 닿으면 오를 수 있다
    Vector3 rockNormal;     // Rock의 법선벡터 

    // 애니메이터를 여기서 관리하므로, 플레이어의 공격애니메이션 지속시간을 구해야한다
    public float clipLength;

    [Header("Raycast")]
    public float raycastDistance;  // 플레이어 머리에서 나가는 ray(벽 감지)
    public LayerMask wallLayer; // 벽 레이어
    private bool isNearWall;    // 벽 근처인가


    [Header("Look")]
    public Transform cameraContainer;   // 카메라 회전
    public float minXLook;  // 회전범위 최소
    public float maxXLook;  // 회전범위 최대
    private float camCurXRot;   // Input Action에서 받아오는 마우스의 delta값
    public float lookSensitivity;  // 회전의 민감도
    private Vector2 mouseDelta;     // 마우스의 delta값

    [Header("Camera Control")]
    public Camera firstPersonCamera; // 1인칭 카메라
    public Camera thirdPersonCamera; // 3인칭 카메라
    public float thirdPersonDistance = 10f; // 3인칭 카메라가 플레이어 뒤에 위치할 거리
    private bool isThirdPerson;     // 현재 카메라가 3인칭인지 여부


    public GameManager gameManager;

    // UI 관련
    public bool canLock = true;
    // 처음에는 인벤토리 창을 비활성화한 상태로 시작한다
    // 즉, CursorMode.Locked 상태(커서가 화면 중앙에 고정)으로 실행
    // 이때 canLock을 true로 한다

    public Action inventoryAction;    // 인벤토리 열고 닫을때 Toggle 메서드를 담아서 실행

    public Rigidbody _rigidbody;
    private void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;   // 게임 시작 중에 마우스 커서 안보이게

        isClimbing = false;
        isMovingBackward = false;
        isThirdPerson = false;
        // 캐싱
        //cameraContainer = Helper.FindChild(gameObject.transform, "CameraController");
        clipLength = Helper.GetAnimationClipLength(playerAnimator, "PlayerMeleeAttack");
        //firstPersonCamera = Helper.FindChild(cameraContainer.transform, "MainCamera").GetComponent<Camera>();
        //thirdPersonCamera = Helper.FindChild(cameraContainer.transform, "ThirdPersonCamera").GetComponent<Camera>();
        //firstPersonCamera.gameObject.SetActive(true);   // 1인칭 시작
        //thirdPersonCamera.gameObject.SetActive(false);

        // "Rock" 레이어의 LayerMask를 가져온다
        wallLayer = LayerMask.GetMask("Rock");
        raycastDistance = 1f;
    }
    // 물리연산은 FixedUpdate에서 호출
    void FixedUpdate()
    {
        CheckWall();
        Move();

        // 벽에 붙은 채로 입력이 없는 경우 벽 타기 애니메이션 일시 정지
        if (isClimbing && curMovementInput.magnitude == 0)
        {
            playerAnimator.speed = 0; // 애니메이션 일시 정지
        }
        else
        {
            playerAnimator.speed = 1; // 애니메이션 재개
        }
    }
    // 카메라연산은 LateUpdate에서 호출
    private void LateUpdate()
    {

        if (canLock)
        {
            CameraLook();
        }
    }
    void CheckWall()
    {
        // Raycast를 사용하여 벽 감지
        RaycastHit hit;
        isNearWall = Helper.Raycast(transform.position, transform.forward, out hit, raycastDistance, wallLayer);

        if (isNearWall && hit.collider.CompareTag("Rock"))
        {
            // 벽이 감지되었을 때 추가 로직 처리
            rockNormal = hit.normal;    // Raycast로 얻은 법선 벡터 사용

            // 벽에 접근하면 벽 타기 상태로 전환
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
                playerAnimator.SetBool("Climbing", true);
                _rigidbody.useGravity = false;
            }
        }
        else // 벽에서 멀어질때
        {
            isNearWall = false;

            // 벽에서 멀어지면 벽 타기 종료
            if (isClimbing)
            {
                isClimbing = false;
                playerAnimator.SetBool("Climbing", false);
                _rigidbody.useGravity = true;
            }
        }
        //
        if (isClimbing) // 벽 타기 중
        {
            // 벽 타기 중일 때의 물리적 처리
            _rigidbody.useGravity = false;

            /// 벽 쪽으로 밀어주는 힘 추가 (벽에서 떨어지지 않도록)
            _rigidbody.AddForce(-rockNormal * 10f, ForceMode.Force);
        }
    }

    // 실제 이동
    void Move()
    {
        if (isClimbing)
        {
            // 벽의 오른쪽 방향 (표면 평행)
            Vector3 wallRight = Vector3.Cross(rockNormal, Vector3.up).normalized;
            // 벽의 위쪽 방향 (표면 평행)
            Vector3 wallUp = Vector3.Cross(wallRight, rockNormal).normalized;
            // 입력 조합 (A/D = 좌우, W/S = 상하)
            Vector3 moveDirection = (wallRight * curMovementInput.x) + (wallUp * curMovementInput.y);
            moveDirection *= moveSpeed;


            _rigidbody.velocity = moveDirection;  // 클라이밍 중 이동 적용
        }
        else // 일반 이동
        {
            Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
            dir *= moveSpeed;
            dir.y = _rigidbody.velocity.y; // 점프를 했을 때에만 위아래로 움직여야 하므로 y방향 속도 초기화

            _rigidbody.velocity = dir;
            //Debug.LogFormat($"플레이어의 위치x,y,z순: {gameObject.transform.position.x} ,{gameObject.transform.position.y},{gameObject.transform.position.z}");
        }
    }
    // 카메라 회전
    void CameraLook()
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
        {
            // 카메라 상하 회전
            camCurXRot += mouseDelta.y * lookSensitivity;   /// 상하회전을 하기 위해서는 y값을 x에 넣는다
            camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
            // 카메라 회전각에 로컬좌표를 줘야하는 이유는 플레이어가 기준이 되기 때문
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  /// -를 하는 이유: 마우스를 내리면 아래로 회전하게 만들기 위해

            // 카메라 좌우 회전
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); /// 좌우회전을 하기 위해서는 x값을 y에 넣는다
        }
    }
    // 입력 이벤트 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
        {
            // S키가 눌렸다면 뒤로 바라본다
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            if (context.phase == InputActionPhase.Performed)  // 키가 계속 눌리는 동안
            {
                // 이동 애니메이션
                playerAnimator.SetBool("Moving", true);
                curMovementInput = context.ReadValue<Vector2>();
            }
            else if (context.phase == InputActionPhase.Canceled)    // 키를 뗐을 때
            {
                // 정지 애니메이션
                playerAnimator.SetBool("Moving", false);
                curMovementInput = Vector2.zero;
            }
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
            mouseDelta = context.ReadValue<Vector2>();  // 마우스 값은 입력하지 않아도 계속 유지된다
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
        {
            if (context.phase == InputActionPhase.Started && IsGrounded())  // 키 누르기 시작할때
            {
                _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);  // 점프는 순간적으로 올라가야하므로 impurse 
            }
        }
    }
    // 아래로 ray를 쏘아 땅에 닿았는지 확인(중복점프를 방지)
    bool IsGrounded()
    {
        // 플레이어 기준 책상다리 4개 만든다
        Ray[] rays = new Ray[4]
    {
            // 플레이어보다 (transform.up * 0.01f) 위에서 쏘는 이유:
            // 플레이어에서 쏘면, 플레이어가 땅에 부딪힌 경우 ground에서 쏠 수가 있어서, ground를 인지 못하는 경우가 발생

            // z축(forward) 앞,뒤 약간 떨어진 곳에서 아래방향으로 발사
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // x축(right) 앞,뒤 약간 떨어진 곳에서 아래방향으로 발사
            new Ray(transform.right + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.right + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
    };
        // 위의 모든 ray를 검출
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                // 4개의 ray중에서 하나라도 ground의 layer 검출되었다면
                return true;
            }
        }
        return false;
    }
    // tab키 누르면 열린다
    public void OnInventory(InputAction.CallbackContext context)
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
        {
            //if (context.phase == InputActionPhase.Started && !GameManager.Instance.isGameOver)
            if (context.phase == InputActionPhase.Started && !gameManager.isGameOver)
            {
                /// UIInventory의 Toggle 메서드를 사용하기 위해 delegate를 사용
                inventoryAction?.Invoke();    // delegate에 Toggle 메서드가 있으면 호출
                ToggleCursor();
            }
        }
    }
    /// <summary>
    /// 내부적으로 Cursor를 Toggle해주는 기능
    /// 인벤토리를 껐을 때는 커서가 화면 중앙에 고정되며, 보이지 않는다
    /// 인벤토리를 켰을때는 화면을 고정하고, 인벤토리를 클릭해줄 커서가 나와서 화면 전체를 움직일 수 있다
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    /// Locked: 인벤토리창이 아직 열리지 않은 상태(커서가 화면 중앙에 고정되어있다)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // toggle이 true: 인벤토리창이 열리지 않아서 커서가 화면 중앙에 고정 -> None으로 만들어서, 화면에서 움직일 수 있게 만든다
        // toggle이 false: 인벤토리창이 열려있다면 커서가 화면에서 움직일 수 있다 -> Locked로 만들어서 화면 중앙에 커서를 고정한다

        canLock = !toggle;
        // toggle이 true: 위에서 커서를 화면에서 움직일 수 있게 만들었으므로 canLock은 false
        // toggle이 false: 위에서 화면 중앙에 커서를 고정했으므로 canLock은 true
    }
    // v 키 누르면 카메라 교체
    public void OnCamChange(InputAction.CallbackContext context)
    {
        //if (!GameManager.Instance.isGameOver)
        if (!gameManager.isGameOver)
        {
            if (isThirdPerson)
            {
                // 1인칭 카메라 활성화
                firstPersonCamera.gameObject.SetActive(true);
                thirdPersonCamera.gameObject.SetActive(false);
            }
            else
            {
                // 3인칭 카메라 활성화
                firstPersonCamera.gameObject.SetActive(false);
                thirdPersonCamera.gameObject.SetActive(true);
            }

            // 상태 토글
            isThirdPerson = !isThirdPerson;
        }
    }
}
