using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� ������, ���߿� Ŭ���� �ٽ� �����ٺ��� ����� ��
/// <summary>
/// ���ӸŴ����� �ʹ� �ʰ� �������
/// ���ӿ��� ���� ������ �ٷ��
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public bool isGameOver;
    public UIManager uiManager;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // �θ� Ŭ���� �α� ��� ����

        // �� ����� �� ������ �ڵ� �߰�
        // �ʱ�ȭ �ڵ�
        isGameOver = false;        
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void GameOver()
    {
        isGameOver=true;
        // ���� �� UI ����
        Time.timeScale = 0f;
        uiManager.uiGameOver.gameObject.SetActive(true);    
    }
    public void Restart()
    {
        // ���� ���� �ٽ� �ε�
        Time.timeScale = 1f;        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
