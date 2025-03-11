using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 아직은 쓰지 않지만, 나중에 클래스 다시 나누다보면 사용할 듯
/// <summary>
/// 게임매니저를 너무 늦게 만들었다
/// 게임오버 관련 로직을 다룬다
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public bool isGameOver;
    public UIManager uiManager;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // 부모 클래스 로그 출력 유지

        // 씬 변경될 때 실행할 코드 추가
        // 초기화 코드
        isGameOver = false;        
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void GameOver()
    {
        isGameOver=true;
        // 정지 후 UI 띄운다
        Time.timeScale = 0f;
        uiManager.uiGameOver.gameObject.SetActive(true);    
    }
    public void Restart()
    {
        // 현재 씬을 다시 로드
        Time.timeScale = 1f;        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
