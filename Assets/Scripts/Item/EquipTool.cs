using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, �ڿ�ä��
public class EquipTool : Equip
{
    public float attackRate;    // �����ֱ�(���� �ִϸ��̼� ����ð�)
    private bool attacking;     // �������ΰ�?
    public float attackDistance;    // �ִ� ���� ������ �Ÿ�

    public float useStamina;    // 1ȸ �ൿ�Ҷ� ���׹̳� �Ҹ�


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
    /// ���� ���⿡ �ִϸ��̼��� ��������, �÷��̾ �ִϸ��̼��� ����ϴµ�
    /// �÷��̾��� �ִϸ��̼��� ������ �� �޼��带 ȣ���ϰ� �Ѵ�
    /// 
    /// </summary>
    public void OnHit()
    {
        // 1��Ī, 3��Ī�� �� ������ �޶�� �Ѵ�
        Ray ray = GetAttackRay();
        ProcessHit(ray);
    }
    // 1��Ī, 3��Ī�� ���� Ray ����
    private Ray GetAttackRay()
    {
        if (!Helper.isThirdPerson) // 1��Ī
            return camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        else // 3��Ī
            return camera.ScreenPointToRay(Input.mousePosition);
    }
    // Raycast �� ������ ��ȣ�ۿ� ó��
    private void ProcessHit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            // �浹�� ������ ���� ��ǥ
            Vector3 worldPosition = hit.point;
            Debug.Log($"EquipTool���� ȣ��, ���콺 Ŭ�� ��ġ (���� ��ǥ): {worldPosition}");

            HandleResourceGathering(hit);
            HandleCombat(hit);
        }
    }
    // �ڿ� ä��
    private void HandleResourceGathering(RaycastHit hit)
    {
        if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            resource.Gather(hit.point, hit.normal);
    }
    // ���� 
    private void HandleCombat(RaycastHit hit)
    {
        if (doesDealDamage && hit.collider.TryGetComponent(out NPC monster))
            monster.TakePhysicalDamage(damage);
    }
}
