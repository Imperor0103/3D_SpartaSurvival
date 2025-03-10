using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode); // 부모 클래스 로그 출력 유지

        // 씬 변경될 때 실행할 코드 추가
        // 초기화 코드

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
