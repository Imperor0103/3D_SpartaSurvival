using System;
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
        //restartButton = Helper.FindChild(gameObject.transform, "RestartButton").GetComponent<Button>();
        //exitButton = Helper.FindChild(gameObject.transform, "ExitButton").GetComponent<Button>();

        // �̺�Ʈ �޼��� ����
        //restartButton.onClick.AddListener(() =>
        //{
        //    try
        //    {
        //        UIManager.Instance.OnClickRestart();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("Exception in OnClickRestart: " + e.Message);
        //    }
        //});

        //exitButton.onClick.AddListener(() =>
        //{
        //    try
        //    {
        //        UIManager.Instance.OnClickExit();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("Exception in OnClickExit: " + e.Message);
        //    }
        //});
    }
}