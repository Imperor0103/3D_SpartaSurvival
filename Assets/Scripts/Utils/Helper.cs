using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 헬퍼 클래스(비 컴포넌트)
/// static 메서드로 만들어서 모든 곳에서 쓸 수 있도록 한다
/// </summary>
public class Helper
{
    // 자식의 오브젝트를 재귀검색한다
    public static Transform FindChild(Transform parent, string findname)
    {
        // Transform도 참조형식이므로 없는 경우 null이 반환된다
        // findname 검색하는 게임오브젝트의 이름이다
        if (parent.name == findname)
            return parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform childTr = parent.GetChild(i);
            Transform findTr = FindChild(childTr, findname);
            if (findTr != null)
                return findTr;
        }
        return null;
    }
    // 자식이 아닌 오브젝트를 검색한다
    public static Transform FindObjectInScene(string findname)
    {
        // 최상위 루트오브젝트들을 찾는다
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // 루트 오브젝트를 검색하여 자식을 찾는다
        if (rootObjects.Length != 0)
        {
            Transform foundTr;
            for (int i = 0; i < rootObjects.Length; i++)
            {
                foundTr = FindChild(rootObjects[i].transform, findname);
                if (foundTr)
                    return foundTr;
            }
        }
        return null;
    }
    /// <summary>
    /// DontDestroyOnLoad 씬에 있는 오브젝트까지 포함하여 모두 검색한다
    /// </summary>
    /// <param name="findname"></param>
    /// <returns></returns>
    public static Transform FindObjAnywhere(string findname)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // 비활성화된 오브젝트 포함 검색
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == findname)
            {
                return obj.transform;
            }
        }
        return null;
    }



    public static float GetAnimationClipLength(Animator playerAnimator, string clipName)
    {
        if (playerAnimator != null)
        {
            foreach (AnimationClip clip in playerAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        return 0f;
    }
    // Raycast
    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask layerMask)
    {
        return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
    }

}
