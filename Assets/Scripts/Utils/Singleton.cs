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
    private static List<GameObject> persistentObjects = new List<GameObject>(); // ������ ������Ʈ ����Ʈ

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
        /// �Ŵ������� �����ϰ��ִ� ��� ������Ʈ�� �ϴ� DontDestroyOnLoad ������ �Ѵ�
        /// �ߺ��Ǵ� ������Ʈ �����Ѵ�
        for (int i = persistentObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = persistentObjects[i];

            // ���� ����Ǿ��� �� ���� �̸��� ��ġ�� ���ο� ������Ʈ�� �����Ǿ����� �˻�
            foreach (GameObject newObj in FindObjectsOfType<GameObject>())
            {
                if (newObj.name == obj.name && newObj.transform.position == obj.transform.position)
                {
                    Destroy(newObj); // �ߺ��� �� ������Ʈ ����
                }
            }
        }
        // �� �� �κ��� ���� Ŭ�������� ������ ����Ѵ�
    }
    /// <summary>
    /// Ư�� ������Ʈ�� `DontDestroyOnLoad`�� ����ϴ� �޼���
    /// </summary>
    public void RegisterPersistentObject(GameObject obj)
    {
        if (!persistentObjects.Contains(obj))
        {
            DontDestroyOnLoad(obj);
            persistentObjects.Add(obj);
        }
    }


    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        /// ������ �ʿ���°� ���� �����Ѵ�
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