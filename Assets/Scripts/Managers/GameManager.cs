using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� ������, ���߿� Ŭ���� �ٽ� �����ٺ��� ����� ��
public class GameManager : Singleton<GameManager>
{
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // �θ� Ŭ���� �α� ��� ����

        // �� ����� �� ������ �ڵ� �߰�
        // �ʱ�ȭ �ڵ�

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
