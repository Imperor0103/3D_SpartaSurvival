using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, �ڿ�ä��, �ɷ����
public class EquipTool : Equip
{
    public float attackRate;    // �����ֱ�(���� �ִϸ��̼� ����ð�)
    private bool attacking;     // �������ΰ�?
    public float attackDistance;    // �ִ� ���� ������ �Ÿ�

    public float useStamina;    // 1ȸ �ൿ�Ҷ� ���׹̳� �Ҹ�

    //
    public float speed;
    public float jumpCount; // ����Ƚ��


    [Header("Resource Gathering")]
    public bool doesGatherResources;    /// �ɼ�1.�ڿ� ä�� �����Ѱ�?(Gather�� �����Ѱ�?)

    [Header("Combat")]
    public bool doesDealDamage; /// �ɼ�2.�������� �� �� �ִ°�?(������ �ϴ°ǰ�?
    public int damage;     // ������ ��


    private Camera camera;  // ray�� ��� ī�޶�(���� ī�޶�)


    private void Start()
    {
        camera = Camera.main;

        attackRate = CharacterManager.Instance.Player.controller.clipLength; ;    // PlayerMeleeAttack�� ����ð�
    }
    public override void OnAttackInput()
    {
        // isAttacking�� false�϶��� ���η��� ����
        if (!attacking)
        {
            // ���׹̳��� ������ ���� ��밡��
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                Invoke("OnCanAttack", attackRate);      /// �ð���(�����ֱ⸶�� �ѹ� ȣ��)
            }
        }
    }
    // ���� �ִϸ��̼� ��
    void OnCanAttack()
    {
        attacking = false;
        CharacterManager.Instance.Player.controller.playerAnimator.SetBool("AttackFinishing", true);   // ���� �����Ҷ��� true, ���� ���� �� false
        // �ִϸ��̼� ����� ����Ǿ����Ƿ� OnHit�� ���� ȣ���Ѵ�
        OnHit();
    }

    // �̺�Ʈ �Լ�
    // aim �������� ������ �� EquipTool���� attackDistance �̳��� ������ ȣ��
    /// <summary>
    /// �ڿ� ä�� �ִϸ��̼ǿ��� �����ϴ� �� key frame���� OnHit�� ȣ���ؾ��Ѵ�
    /// 
    /// ���� ���⿡ �ִϸ��̼��� ��������, �÷��̾ �ִϸ��̼��� ����ϴµ�
    /// �÷��̾��� �ִϸ��̼��� ������ �� �޼��带 ȣ���ϰ� �Ѵٸ�...?
    /// 
    /// </summary>
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            /// doesGatherResources: �ڿ� ä�� �����ؾ��Ѵ�(Gather�� ����)
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                /// ���� Resource ������Ʈ�� ������ �ִٸ� resource�� Gather�� ȣ���Ѵ�
                resource.Gather(hit.point, hit.normal);
            }

            /// Ÿ�� �����ؾ��Ѵ�
            if (doesDealDamage && hit.collider.TryGetComponent(out NPC monster))
            {
                /// ���� NPC ������Ʈ�� ������ �ִٸ� monster�� 
                monster.TakePhysicalDamage(damage);
            }
        }
    }
}
