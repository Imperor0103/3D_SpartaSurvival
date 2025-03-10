using System;   // 직렬화를 사용하기 위해 필요
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // MyData 클래스를 직렬화 가능하게 설정
public class MyData
{
    public string name;
    public int age;

    [SerializeField] private int hp;
}
