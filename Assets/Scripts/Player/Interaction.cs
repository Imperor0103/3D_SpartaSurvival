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
        promptText = Helper.FindObjectInScene("PromptText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log($"스크린 좌표: {mousePosition}");

        // 1인칭일때만 ray를 만든다
        // checkRate 간격으로 ray를 만들어야한다
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            if (!Helper.isThirdPerson)
            {
                CheckFirstPersonInteraction();  // 1인칭 카메라
            }
            else
            {
                CheckThirdPersonInteraction();  // 3인칭 카메라
            }
        }
    }
    // 1인칭일때는 ray를 만들어서 ray와 닿으면 상호작용
    private void CheckFirstPersonInteraction()
    {
        // 카메라에서 화면 중앙에 ray 발사
        // 카메라가 찍고 있는 방향이 있기 때문에, 시작점만 정해주면 된다
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        ProcessRaycast(ray);
    }
    // 3인칭일때는 마우스로 클릭한 곳으로 카메라에 ray를 쏴서 닿으면 상호작용
    private void CheckThirdPersonInteraction()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        ProcessRaycast(ray);
    }
    // 1인칭, 3인칭 모두 ray를 쏘고 결과를 처리하는 공통 메서드
    private void ProcessRaycast(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            // 충돌한 지점의 월드 좌표
            Vector3 worldPosition = hit.point;
            Debug.Log($"Interaction에서 호출, 마우스 클릭 위치 (월드 좌표): {worldPosition}");

            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
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
        /// E를 눌렀을 때, aim이 아이템을 바라보고 있을 때(인터페이스로 캐싱하고 있는 정보가 있을 때)
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
