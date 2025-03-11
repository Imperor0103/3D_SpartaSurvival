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

    public bool isInvincible = false;   // ��������

    public float noHungerHealthDecay;   // hunger�� 0�� �Ǹ� ü�� ���Ұ� ����

    public event Action onTakeDamage;   // hp ���ҽ� ȭ�� �������� ���� delegate
    // DamageIndicator���� PlayerCondition�� �����Ͽ� onTakeDamage�� ���



    // hunger�� ���������� ������
    void Update()
    {
        // Time.deltaTime: ����� ���� ���̸� �����Ѵ�
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
        // time��ŭ �ڿ� �ٽ� �������
        yield return new WaitForSeconds(time);
        CharacterManager.Instance.Player.controller.moveSpeed /= amount;
    }


    public void Invincible(float time)
    {
        StartCoroutine(InvincibleCoroutine(time));
    }
    private IEnumerator InvincibleCoroutine(float time)
    {
        hunger.Add(hunger.passiveValue * Time.deltaTime);   // hunger�� �پ���� �ʴ´�
        isInvincible = true;
        // time��ŭ �ڿ� �ٽ� �������
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("�׾���");
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
            onTakeDamage?.Invoke(); // delegate�� �Լ��� ������ ȣ��
        }
    }
    // ��� �ֵθ��� ���׹̳� �پ���
    // ��� ����ϴ� �ʿ��� UseStamina�� ȣ��
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)  // �پ�� ���¹̳��� 0���� ������, �� �ൿ�� �� �� ����
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

}
