using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class LazerTrap : MonoBehaviour, ITrap
{
    public bool IsActive { get; private set; }  // �⺻������ Ȱ��ȭ
    public float Damage { get; private set; } = 10f;  // �⺻ ������

    public Transform startPos;       // �������� ���۵� ��ġ
    public float maxDistance = 10f;  // ������ �ִ� �Ÿ�
    public LayerMask targetLayer;    // �浹 ������ Ÿ�� ���̾� (�÷��̾� ��)


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

        targetLayer = LayerMask.GetMask("Player", "Monster");  // Ÿ���� �÷��̾�, ����
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
            Debug.Log("�÷��̾ ������ Ʈ���� �¾ҽ��ϴ�");
        }
    }

    private void ShootLaser()
    {
        lineRenderer.enabled = true;
        Vector3 start = startPos.position;
        Vector3 direction = GetDirection(); // ������ ���� ���

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
        return startPos.forward; // ������ ���� ���� ��ȯ
    }
}
