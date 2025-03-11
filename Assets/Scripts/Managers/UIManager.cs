using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI�Ŵ����� �ʹ� �ʰ� �������
/// ���ӿ���UI��ŭ�� �Ŵ����� ���� �����غ���
/// </summary>
public class UIManager : Singleton<UIManager>   
{
    public UIGameOver uiGameOver;
    public GameManager gameManager;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // �θ� Ŭ���� �α� ��� ����

        // �� ����� �� ������ �ڵ� �߰�
        // �ʱ�ȭ �ڵ�

        // ���ӸŴ����� ���� �����Ǿ���Ѵ�
        if (gameManager == null)
        {
            gameManager = GameManager.Instance; 
            gameManager.uiManager = this;   
        }

        uiGameOver = Helper.FindChild(gameObject.transform, "GameOver").GetComponent<UIGameOver>();
        uiGameOver.gameObject.SetActive(false); // ó������ ��Ȱ��ȭ
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // ���ӿ��� UI 
    
    public void OnClickRestart()
    {
        gameManager.Restart();
    }
    public void OnClickExit()
    {
        Application.Quit(); 
    }

}
