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
    public static bool isThirdPerson;     // ���� ī�޶� 3��Ī���� ����

    // UI�� RectTransform ��ǥ�� ���� ��ǥ�� ��ȯ�ϴ� �޼���    
    public static Vector3 ConvertUIToWorldPosition(RectTransform rectTransform, Camera uiCamera)
    {
        return uiCamera.transform.TransformPoint(rectTransform.localPosition);
    }
    // ���� ��ǥ�� UI ��ǥ(Vector2)�� ��ȯ�ϴ� �޼���
    public static Vector2 ConvertWorldToUIPosition(Vector3 worldPosition, RectTransform canvasRectTransform, Camera uiCamera)
    {
        Vector3 screenPos = uiCamera.WorldToScreenPoint(worldPosition);
        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, uiCamera, out uiPosition);
        return uiPosition;
    }
    // UI ��ǥ�� Vector3 ���� ��ǥ�� ��ȯ
    public static Vector3 ConvertUICoordinateToWorld(Vector2 uiPosition, Camera uiCamera, RectTransform canvas)
    {
        Vector3 screenPos = new Vector3(uiPosition.x, uiPosition.y, uiCamera.nearClipPlane);
        return uiCamera.ScreenToWorldPoint(screenPos);
    }

    // Vector3 ���� ��ǥ�� UI ��ǥ�� ��ȯ
    public static Vector2 ConvertWorldCoordinateToUI(Vector3 worldPosition, RectTransform canvas, Camera uiCamera)
    {
        Vector3 screenPos = uiCamera.WorldToScreenPoint(worldPosition);
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, uiCamera, out uiPos);
        return uiPos;
    }






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
    // �ִϸ��̼� Ŭ���� ���̸� ����
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


}
