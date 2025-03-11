using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    // 게임시작버튼, 게임종료버튼 연결하고 이벤트 메서드 연결한다
    public Button restartButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 연결(게임오버 UI가 활성화될 때 Start가 호출되면서 연결된다)
        restartButton = Helper.FindChild(gameObject.transform,"RestartButton").GetComponent<Button>();
        exitButton = Helper.FindChild(gameObject.transform, "ExitButton").GetComponent<Button>();

        // 이벤트 메서드 연결
        restartButton.onClick.AddListener(() => UIManager.Instance.OnClickRestart());
        exitButton.onClick.AddListener(() => UIManager.Instance.OnClickExit());
    }
}
