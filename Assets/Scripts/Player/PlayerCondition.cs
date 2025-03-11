using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public bool isInvincible = false;   // 무적상태

    public float noHungerHealthDecay;   // hunger가 0이 되면 체력 감소가 시작

    public event Action onTakeDamage;   // hp 감소시 화면 깜빡임을 받을 delegate
    // DamageIndicator에서 PlayerCondition에 접근하여 onTakeDamage에 등록



    // hunger를 지속적으로 내린다
    void Update()
    {
        // Time.deltaTime: 기기의 성능 차이를 보정한다
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        if (!isInvincible)
        {
            hunger.Subtract(hunger.passiveValue * Time.deltaTime);
            if (hunger.curValue == 0f)
            {
                health.Subtract(noHungerHealthDecay * Time.deltaTime);
            }
            if (health.curValue == 0f)
            {
                Die();
            }
        }
    }
    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void Eat(float amount)
    {
        health.Add(amount);
    }


    public void Speed(float amount, float time)
    {
        StartCoroutine(SpeedBoostCoroutine(amount, time));
    }
    private IEnumerator SpeedBoostCoroutine(float amount, float time)
    {
        CharacterManager.Instance.Player.controller.moveSpeed *= amount;
        // time만큼 뒤에 다시 원래대로
        yield return new WaitForSeconds(time);
        CharacterManager.Instance.Player.controller.moveSpeed /= amount;
    }


    public void Invincible(float time)
    {
        StartCoroutine(InvincibleCoroutine(time));
    }
    private IEnumerator InvincibleCoroutine(float time)
    {
        hunger.Add(hunger.passiveValue * Time.deltaTime);   // hunger가 줄어들지 않는다
        isInvincible = true;
        // time만큼 뒤에 다시 원래대로
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("죽었다");
    }

    public void TakePhysicalDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        else
        {
            health.Subtract(damage);
            onTakeDamage?.Invoke(); // delegate에 함수가 있으면 호출
        }
    }
    // 장비 휘두르면 스테미나 줄어든다
    // 장비 사용하는 쪽에서 UseStamina를 호출
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)  // 줄어든 스태미나가 0보다 작으면, 그 행동을 할 수 없다
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

}
