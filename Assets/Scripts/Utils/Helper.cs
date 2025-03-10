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
    public static bool isThirdPerson;     // 현재 카메라가 3인칭인지 여부

    // UI의 RectTransform 좌표를 월드 좌표로 변환하는 메서드    
    public static Vector3 ConvertUIToWorldPosition(RectTransform rectTransform, Camera uiCamera)
    {
        return uiCamera.transform.TransformPoint(rectTransform.localPosition);
    }
    // 월드 좌표를 UI 좌표(Vector2)로 변환하는 메서드
    public static Vector2 ConvertWorldToUIPosition(Vector3 worldPosition, RectTransform canvasRectTransform, Camera uiCamera)
    {
        Vector3 screenPos = uiCamera.WorldToScreenPoint(worldPosition);
        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, uiCamera, out uiPosition);
        return uiPosition;
    }
    // UI 좌표를 Vector3 월드 좌표로 변환
    public static Vector3 ConvertUICoordinateToWorld(Vector2 uiPosition, Camera uiCamera, RectTransform canvas)
    {
        Vector3 screenPos = new Vector3(uiPosition.x, uiPosition.y, uiCamera.nearClipPlane);
        return uiCamera.ScreenToWorldPoint(screenPos);
    }

    // Vector3 월드 좌표를 UI 좌표로 변환
    public static Vector2 ConvertWorldCoordinateToUI(Vector3 worldPosition, RectTransform canvas, Camera uiCamera)
    {
        Vector3 screenPos = uiCamera.WorldToScreenPoint(worldPosition);
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, uiCamera, out uiPos);
        return uiPos;
    }






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
    // 애니메이션 클립의 길이를 리턴
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
