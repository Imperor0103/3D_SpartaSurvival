using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� delegate �� ����
/// 2025.03.06.ImSeonggyun
/// </summary>
namespace Define
{
    // delegate ����Ģ: ��ȯ��, D, �Ű����� ������ �ۼ�
    // D:delegate, v: void, f: float, i: int ��
    public delegate void vDv();
    public delegate void vDi(int a);
    public delegate void vDii(int a, int b);
    public delegate float fDf(float a);
    public delegate float fDv();
}
