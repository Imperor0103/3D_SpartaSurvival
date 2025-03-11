using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrap
{
    bool IsActive { get; }  // 트랩의 활성화 여부
    float Damage { get; }   // 트랩이 가하는 피해량

    Vector3 GetDirection(); // 트랩이 공격하는 방향을 반환

    void ActiveTrap();  // 트랩 작동하는 로직
    void DeActiveTrap();    // 트랩 비활성화

    void Trigger(GameObject target);    // 트랩이 타겟(플레이어 등)을 감지했을 때 실행할 로직
}
