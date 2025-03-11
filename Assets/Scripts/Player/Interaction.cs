using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// 카메라에서 ray를 쏜다
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ray를 쏘는 시간간격
    private float lastCheckTime;    // 마지막 ray를 쏜 시각
    public float maxCheckDistance;  // 얼마나 멀리까지 체크할것인가
    public LayerMask layerMask;     // 어떤 layer가 달려있는 게임오브젝트를 검출할것인가

    /// <summary>
    /// ray로 검출된 게임오브젝트의 정보를 담는다
    /// </summary>
    public GameObject curInteractGameObject;    // 검출 성공했다면, interaction하는 게임오브젝트 정보를 저장
    private IInteractable curInteractable;      /// ★검출된 정보를 인터페이스로 캐싱한다 

    // 검출한 아이템 정보를 promptText에 띄운다
    public TextMeshProUGUI promptText;  /// 일단 분리하지는 않지만, 개인과제할때는 UI를 분리해서 drag and drop 안하고 사용하는 방법을 찾아서 리팩토링 해봐라
    private Camera camera;  // 카메라

    void Start()
    {
        camera = Camera.main;
        // 캐싱
        // promptText는 Player가 가진 것이 아니라 UI가 가지고 있다
        // 하이러키에서 둘의 공통부모는 Scene 뿐인데, 그러면 Scene을 가져와서 그 자식중에서 찾아야하나?
        // 매니저에 DontDestroyOnLoad가 설정되어있어서 아래의 방식으로 찾을 수 없게 되었다
        //promptText = Helper.FindObjectInScene("PromptText").GetComponent<TextMeshProUGUI>();
        promptText = Helper.FindObjAnywhere("PromptText").GetComponent<TextMeshProUGUI>();

        /// layerMask에 추가
        layerMask = LayerMask.GetMask("Interactable", "JumpPlatform", "Door");
    }

    // Update is called once per frame
    void Update()
    {
        // checkRate 간격으로 ray를 만들어야한다
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 카메라에서 화면 중앙에 ray 발사
            // 카메라가 찍고 있는 방향이 있기 때문에, 시작점만 정해주면 된다
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; // ray에 부딪힌 게임오브젝트의 정보가 들어간다

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // ray에 검출된 오브젝트가 있다
            {
                if (hit.collider.CompareTag("JumpPlatform"))
                {
                    // JumpingPlatform에 대한 처리
                    HandleJumpingPlatform(hit.collider.gameObject); // 매개변수로 들어가는 게임오브젝트는 JumpPlatform
                }
                else
                {
                    if (hit.collider.gameObject != curInteractGameObject)   // ray에 충돌한 게임오브젝트가 현재 상호작용하는 게임오브젝트가 아니라면
                    {
                        curInteractGameObject = hit.collider.gameObject;    // 새로운 정보로 바꿔
                        curInteractable = hit.collider.GetComponent<IInteractable>();       /// ★검출된 정보를 인터페이스로 캐싱
                        SetPromptText();    // promptText에 출력해라
                    }

                }

            }
            else // 빈공간에 ray를 쏜 경우
            {
                // 모든 정보를 없애라
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    private void HandleJumpingPlatform(GameObject gameObj)
    {
        // JumpingPlatform에 대한 특별한 처리를 여기에 구현
        // 예: 메세지 출력
        //Debug.Log("JumpingPlatform detected: " + gameObj.name);

        // 필요하다면 promptText를 업데이트할 수 있습니다.
        promptText.gameObject.SetActive(true);
        

        JumpPlatform jump = gameObj.GetComponent<JumpPlatform>();
        promptText.text = jump.des;

        // curInteractGameObject와 curInteractable을 업데이트할 수도 있습니다.
        curInteractGameObject = gameObj;
        // JumpingPlatform이 IInteractable을 구현한다면:
        // curInteractable = platform.GetComponent<IInteractable>();
    }


    // promptText에 정보를 세팅
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // text를 활성화
        promptText.text = curInteractable.GetInteractPrompt();  /// ★인터페이스에서 정보를 가져와 정보를 출력
    }
    // E키 눌렀을 때 상호작용
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        /// E를 눌렀을 때, aim이 아이템을 바라보고 있을 때(인 터페이스로 캐싱하고 있는 정보가 있을 때)
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();   // 상호작용 끝나고 인벤토리로 이동한 아이템은 Destroy까지 해준다
            // 상호작용을 끝냈으니 모두 null, 비활성화
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
