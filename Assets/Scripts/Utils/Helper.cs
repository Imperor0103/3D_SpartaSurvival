using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� Ŭ����(�� ������Ʈ)
/// static �޼���� ���� ��� ������ �� �� �ֵ��� �Ѵ�
/// </summary>
public class Helper
{
    // �ڽ��� ������Ʈ�� ��Ͱ˻��Ѵ�
    public static Transform FindChild(Transform parent, string findname)
    {
        // Transform�� ���������̹Ƿ� ���� ��� null�� ��ȯ�ȴ�
        // findname �˻��ϴ� ���ӿ�����Ʈ�� �̸��̴�
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
    // �ڽ��� �ƴ� ������Ʈ�� �˻��Ѵ�
    public static Transform FindObjectInScene(string findname)
    {
        // �ֻ��� ��Ʈ������Ʈ���� ã�´�
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // ��Ʈ ������Ʈ�� �˻��Ͽ� �ڽ��� ã�´�
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
    /// DontDestroyOnLoad ���� �ִ� ������Ʈ���� �����Ͽ� ��� �˻��Ѵ�
    /// </summary>
    /// <param name="findname"></param>
    /// <returns></returns>
    public static Transform FindObjAnywhere(string findname)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // ��Ȱ��ȭ�� ������Ʈ ���� �˻�
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
