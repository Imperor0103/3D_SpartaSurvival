using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용할 delegate 등 선언
/// 2025.03.06.ImSeonggyun
/// </summary>
namespace Define
{
    // delegate 명명규칙: 반환형, D, 매개변수 순으로 작성
    // D:delegate, v: void, f: float, i: int 등
    public delegate void vDv();
    public delegate void vDi(int a);
    public delegate void vDii(int a, int b);
    public delegate float fDf(float a);
    public delegate float fDv();
}
