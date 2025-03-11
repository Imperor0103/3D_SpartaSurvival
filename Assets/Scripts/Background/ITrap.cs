using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrap
{
    bool IsActive { get; }  // Ʈ���� Ȱ��ȭ ����
    float Damage { get; }   // Ʈ���� ���ϴ� ���ط�

    Vector3 GetDirection(); // Ʈ���� �����ϴ� ������ ��ȯ

    void ActiveTrap();  // Ʈ�� �۵��ϴ� ����
    void DeActiveTrap();    // Ʈ�� ��Ȱ��ȭ

    void Trigger(GameObject target);    // Ʈ���� Ÿ��(�÷��̾� ��)�� �������� �� ������ ����
}
