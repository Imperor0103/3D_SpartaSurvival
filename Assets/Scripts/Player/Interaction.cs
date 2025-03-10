using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// ī�޶󿡼� ray�� ���
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ray�� ��� �ð�����
    private float lastCheckTime;    // ������ ray�� �� �ð�
    public float maxCheckDistance;  // �󸶳� �ָ����� üũ�Ұ��ΰ�
    public LayerMask layerMask;     // � layer�� �޷��ִ� ���ӿ�����Ʈ�� �����Ұ��ΰ�

    /// <summary>
    /// ray�� ����� ���ӿ�����Ʈ�� ������ ��´�
    /// </summary>
    public GameObject curInteractGameObject;    // ���� �����ߴٸ�, interaction�ϴ� ���ӿ�����Ʈ ������ ����
    private IInteractable curInteractable;      /// �ڰ���� ������ �������̽��� ĳ���Ѵ� 

    // ������ ������ ������ promptText�� ����
    public TextMeshProUGUI promptText;  /// �ϴ� �и������� ������, ���ΰ����Ҷ��� UI�� �и��ؼ� drag and drop ���ϰ� ����ϴ� ����� ã�Ƽ� �����丵 �غ���
    private Camera camera;  // ī�޶�

    void Start()
    {
        camera = Camera.main;
        // ĳ��
        // promptText�� Player�� ���� ���� �ƴ϶� UI�� ������ �ִ�
        // ���̷�Ű���� ���� ����θ�� Scene ���ε�, �׷��� Scene�� �����ͼ� �� �ڽ��߿��� ã�ƾ��ϳ�?
        promptText = Helper.FindObjectInScene("PromptText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log($"��ũ�� ��ǥ: {mousePosition}");

        // 1��Ī�϶��� ray�� �����
        // checkRate �������� ray�� �������Ѵ�
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            if (!Helper.isThirdPerson)
            {
                CheckFirstPersonInteraction();  // 1��Ī ī�޶�
            }
            else
            {
                CheckThirdPersonInteraction();  // 3��Ī ī�޶�
            }
        }
    }
    // 1��Ī�϶��� ray�� ���� ray�� ������ ��ȣ�ۿ�
    private void CheckFirstPersonInteraction()
    {
        // ī�޶󿡼� ȭ�� �߾ӿ� ray �߻�
        // ī�޶� ��� �ִ� ������ �ֱ� ������, �������� �����ָ� �ȴ�
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        ProcessRaycast(ray);
    }
    // 3��Ī�϶��� ���콺�� Ŭ���� ������ ī�޶� ray�� ���� ������ ��ȣ�ۿ�
    private void CheckThirdPersonInteraction()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        ProcessRaycast(ray);
    }
    // 1��Ī, 3��Ī ��� ray�� ��� ����� ó���ϴ� ���� �޼���
    private void ProcessRaycast(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            // �浹�� ������ ���� ��ǥ
            Vector3 worldPosition = hit.point;
            Debug.Log($"Interaction���� ȣ��, ���콺 Ŭ�� ��ġ (���� ��ǥ): {worldPosition}");

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



    // promptText�� ������ ����
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // text�� Ȱ��ȭ
        promptText.text = curInteractable.GetInteractPrompt();  /// ���������̽����� ������ ������ ������ ���
    }
    // EŰ ������ �� ��ȣ�ۿ�
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        /// E�� ������ ��, aim�� �������� �ٶ󺸰� ���� ��(�������̽��� ĳ���ϰ� �ִ� ������ ���� ��)
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();   // ��ȣ�ۿ� ������ �κ��丮�� �̵��� �������� Destroy���� ���ش�
            // ��ȣ�ۿ��� �������� ��� null, ��Ȱ��ȭ
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
