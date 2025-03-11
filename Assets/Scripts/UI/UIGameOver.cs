using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    // ���ӽ��۹�ư, ���������ư �����ϰ� �̺�Ʈ �޼��� �����Ѵ�
    public Button restartButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // ��ư ����(���ӿ��� UI�� Ȱ��ȭ�� �� Start�� ȣ��Ǹ鼭 ����ȴ�)
        restartButton = Helper.FindChild(gameObject.transform,"RestartButton").GetComponent<Button>();
        exitButton = Helper.FindChild(gameObject.transform, "ExitButton").GetComponent<Button>();

        // �̺�Ʈ �޼��� ����
        restartButton.onClick.AddListener(() => UIManager.Instance.OnClickRestart());
        exitButton.onClick.AddListener(() => UIManager.Instance.OnClickExit());
    }
}
