using System;   // ����ȭ�� ����ϱ� ���� �ʿ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // MyData Ŭ������ ����ȭ �����ϰ� ����
public class MyData
{
    public string name;
    public int age;

    [SerializeField] private int hp;
}
