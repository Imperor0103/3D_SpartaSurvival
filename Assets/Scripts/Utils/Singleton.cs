using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Ŵ����� �̱������� �����ϱ� ���� ���׸� Ŭ����
/// 2025.03.06.ImSeonggyun
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // �ش� ������Ʈ�� ������ �ִ� ���� ������Ʈ�� ã�Ƽ� ����
                instance = (T)FindAnyObjectByType(typeof(T));

                // �ν��Ͻ��� ã�� ���� ���
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        // �̱��� �ν��Ͻ��� ���ٸ�
        if (instance == null)
        {
            instance = this as T;   // ���� ��ü�� �̱��� �ν��Ͻ��� ����

            // DontDestroyOnLoad�� ��Ʈ ������Ʈ���� ��밡���ϹǷ� 
            // ��Ʈ ������Ʈ�� �ƴϸ� �ֻ����� �̵�
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            DontDestroyOnLoad(gameObject);   // �� ���� �ÿ��� ����
            SceneManager.sceneLoaded += OnSceneLoaded;  // �� ���� ���� �߰�        
        }
        // �̹� �̱��� �ν��Ͻ��� �����ϸ�, ���� ��ü�� ����
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� �ε�� �� ����Ǵ� �޼���(�� �Ŵ������� �������̵�)
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� Ŭ�������� ������ ����Ѵ�
    }
    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}



public interface ISwitchable
{
    public bool IsActive { get; }
    public void Activate();
    public void Deactivate();
}


public class Switch : MonoBehaviour
{
    public ISwitchable client;
    public void Toggle()
    {
        if (client.IsActive)
        {
            client.Deactivate();
        }
        else
        {
            client.Activate();
        }
    }
}
public class Door : MonoBehaviour, ISwitchable
{
    private bool isActive;
    public bool IsActive => isActive;
    public void Activate()
    {
        isActive = true;
        Debug.Log("The door is open.");
    }
    public void Deactivate()
    {
        isActive = false;
        Debug.Log("The door is closed.");
    }
}