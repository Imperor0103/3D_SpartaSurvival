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

        /// �Ŵ����� �����ϰ� �ִ� ��� ������Ʈ�� DontDestroyOnLoad ������ �ʿ��ϴ�
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void GameOver()
    {
        isGameOver = true;

        // ���� �� UI ����
        Time.timeScale = 0f;
        uiManager.uiGameOver.gameObject.SetActive(true);
        // Ŀ�� ��� ����    
        Cursor.lockState = CursorLockMode.None;
    }
    public void Restart()
    {
        // ���� ���� �ٽ� �ε�
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // health, Hunger, stamina �ٽ� �������
        CharacterManager.Instance.Player.condition.health.curValue = CharacterManager.Instance.Player.condition.health.startValue;
        CharacterManager.Instance.Player.condition.hunger.curValue = CharacterManager.Instance.Player.condition.hunger.startValue;
        CharacterManager.Instance.Player.condition.stamina.curValue = CharacterManager.Instance.Player.condition.stamina.startValue;
        // �κ��丮 �ʱ�ȭ
        UIManager.Instance.inventory.ClearInventory();  // ����
        UIManager.Instance.inventory.gameObject.SetActive(true);    // Ȱ��ȭ
        // �κ��丮 ���ݴ� Toggle �޼��带 Action�� ����
        CharacterManager.Instance.Player.controller.inventory += UIManager.Instance.inventory.Toggle;
        UIManager.Instance.inventory.inventoryWindow.SetActive(false);


        // Ŀ�� �ٽ� ���
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
