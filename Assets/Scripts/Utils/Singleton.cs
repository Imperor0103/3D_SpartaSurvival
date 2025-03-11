using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 매니저를 싱글턴으로 선언하기 위한 제네릭 클래스
/// 2025.03.06.ImSeonggyun
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static List<GameObject> persistentObjects = new List<GameObject>(); // 유지할 오브젝트 리스트

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 해당 컴포넌트를 가지고 있는 게임 오브젝트를 찾아서 리턴
                instance = (T)FindAnyObjectByType(typeof(T));

                // 인스턴스를 찾지 못한 경우
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
        // 싱글턴 인스턴스가 없다면
        if (instance == null)
        {
            instance = this as T;   // 현재 객체를 싱글턴 인스턴스로 설정

            // DontDestroyOnLoad는 루트 오브젝트에만 사용가능하므로 
            // 루트 오브젝트가 아니면 최상위로 이동
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            DontDestroyOnLoad(gameObject);   // 씬 변경 시에도 유지
            SceneManager.sceneLoaded += OnSceneLoaded;  // 씬 변경 감지 추가        
        }
        // 이미 싱글턴 인스턴스가 존재하면, 현재 객체를 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 씬이 로드될 때 실행되는 메서드(각 매니저에서 오버라이드)
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /// 매니저에서 참조하고있는 모든 오브젝트는 일단 DontDestroyOnLoad 선언을 한다
        /// 중복되는 오브젝트 제거한다
        for (int i = persistentObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = persistentObjects[i];

            // 씬이 변경되었을 때 같은 이름과 위치의 새로운 오브젝트가 생성되었는지 검사
            foreach (GameObject newObj in FindObjectsOfType<GameObject>())
            {
                if (newObj.name == obj.name && newObj.transform.position == obj.transform.position)
                {
                    Destroy(newObj); // 중복된 새 오브젝트 삭제
                }
            }
        }
        // 그 외 부분은 각각 클래스에서 정의해 사용한다
    }
    /// <summary>
    /// 특정 오브젝트를 `DontDestroyOnLoad`에 등록하는 메서드
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

        /// 씬에서 필요없는건 따로 삭제한다
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