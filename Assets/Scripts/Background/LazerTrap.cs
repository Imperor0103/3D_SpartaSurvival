using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class LazerTrap : MonoBehaviour, ITrap
{
    public bool IsActive { get; private set; }  // 기본적으로 활성화
    public float Damage { get; private set; } = 10f;  // 기본 데미지

    public Transform startPos;       // 레이저가 시작될 위치
    public float maxDistance = 10f;  // 레이저 최대 거리
    public LayerMask targetLayer;    // 충돌 감지할 타겟 레이어 (플레이어 등)


    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        IsActive = true;
        startPos = gameObject.transform;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        lineRenderer.positionCount = 2;

        targetLayer = LayerMask.GetMask("Player", "Monster");  // 타겟은 플레이어, 몬스터
    }


    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            ShootLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void ActiveTrap()
    {
        IsActive = true;
    }

    public void DeActiveTrap()
    {
        IsActive = false;
    }

    public void Trigger(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerCondition>()?.TakePhysicalDamage(10);
            Debug.Log("플레이어가 레이저 트랩에 맞았습니다");
        }
    }

    private void ShootLaser()
    {
        lineRenderer.enabled = true;
        Vector3 start = startPos.position;
        Vector3 direction = GetDirection(); // 레이저 방향 사용

        RaycastHit hit;

        if (Physics.Raycast(start, direction, out hit, maxDistance, targetLayer))
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, hit.point);

            Trigger(hit.collider.gameObject);
        }
        else
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, start + direction * maxDistance);
        }
    }

    public Vector3 GetDirection()
    {
        return startPos.forward; // 레이저 진행 방향 반환
    }
}
