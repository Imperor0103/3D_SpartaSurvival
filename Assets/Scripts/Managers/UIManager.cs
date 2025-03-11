using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI매니저를 너무 늦게 만들었다
/// 게임오버UI만큼은 매니저를 통해 관리해보자
/// </summary>
public class UIManager : Singleton<UIManager>
{
    public UIGameOver uiGameOver;
    public GameManager gameManager;
    public UIInventory inventory;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // 부모 클래스 로그 출력 유지

        // 씬 변경될 때 실행할 코드 추가
        // 초기화 코드

        // 게임매니저가 먼저 생성되어야한다
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            gameManager.uiManager = this;
        }

        uiGameOver = Helper.FindChild(gameObject.transform, "GameOver").GetComponent<UIGameOver>();
        uiGameOver.gameObject.SetActive(false); // 처음에는 비활성화

        // 캐싱
        try
        {
            inventory = Helper.FindChild(gameObject.transform, "UIInventory").GetComponent<UIInventory>();
        }
        catch (Exception e)    
        {
            // 인벤토리 연결이 안됐다
            Debug.LogException(e);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // 게임오버 UI 

    public void OnClickRestart()
    {
        gameManager.Restart();
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

}
