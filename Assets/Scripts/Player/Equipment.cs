using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �������
public class Equipment : MonoBehaviour
{
    public Equip curEquip;      // ���� �����ϰ� �ִ� �������� ����
    public Transform equipParent;   // ��� �޾��� ��ġ: ������

    // 
    //private PlayerController controller;
    public PlayerController controller;
    //private PlayerCondition condition;
    public PlayerCondition condition;

    void Start()
    {
        // ĳ��
        //controller = CharacterManager.Instance.Player.controller;
        //condition = CharacterManager.Instance.Player.condition;
        //// ������ġ�� ã�Ƽ� �����Ѵ�
        //equipParent = Helper.FindChild(gameObject.transform, "thumb_r_3");
    }

    // ����
    public void EquipNew(ItemData data)
    {
        UnEquip();  // ������ ��� �ִ� ��� ����
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }
    // ����
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }


    /// <summary>
    /// ���콺 ���� ������ ȣ��ȴ�
    /// 
    /// ���� �� ��ü�� �ִϸ��̼��� ���� ������� �ٲپ���
    /// �ִϸ��̼��� �÷��̾ ����Ѵ�
    /// </summary>
    /// <param name="context"></param>
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // InputActionPhase.Performed: ������ �ִ� ����
        // curEquip != null: ���� �������� ���Ⱑ �ִ�
        // controller.canLock: �κ��丮â�� ��Ȱ��ȭ�� ����
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLock)
        {
            // ���� �ִϸ��̼��� �÷��̾��� �ִϸ��̼�
            // ���⼭ SetTrigger
            CharacterManager.Instance.Player.controller.playerAnimator.SetBool("AttackFinishing", false);   // ���� �����Ҷ��� true, ���� ���� �� false
            CharacterManager.Instance.Player.controller.playerAnimator.SetTrigger("Attack");    // ���� �ִϸ��̼� ���
            curEquip.OnAttackInput();
        }
    }
}
