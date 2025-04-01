using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 장착기능
public class Equipment : MonoBehaviour
{
    public Equip curEquip;      // 현재 장착하고 있는 아이템의 정보
    public Transform equipParent;   // 장비를 달아줄 위치: 오른팔

    // 
    //private PlayerController controller;
    public PlayerController controller;
    //private PlayerCondition condition;
    public PlayerCondition condition;

    void Start()
    {
        // 캐싱
        //controller = CharacterManager.Instance.Player.controller;
        //condition = CharacterManager.Instance.Player.condition;
        //// 장착위치를 찾아서 저장한다
        //equipParent = Helper.FindChild(gameObject.transform, "thumb_r_3");
    }

    // 장착
    public void EquipNew(ItemData data)
    {
        UnEquip();  // 기존에 들고 있는 장비 해제
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }
    // 해제
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }


    /// <summary>
    /// 마우스 왼쪽 누르면 호출된다
    /// 
    /// 무기 그 자체의 애니메이션이 없는 방식으로 바꾸었다
    /// 애니메이션은 플레이어가 재생한다
    /// </summary>
    /// <param name="context"></param>
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // InputActionPhase.Performed: 눌려져 있는 동안
        // curEquip != null: 현재 장착중인 무기가 있다
        // controller.canLock: 인벤토리창이 비활성화된 상태
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLock)
        {
            // 공격 애니메이션은 플레이어의 애니메이션
            // 여기서 SetTrigger
            CharacterManager.Instance.Player.controller.playerAnimator.SetBool("AttackFinishing", false);   // 공격 시작할때는 true, 공격 끝날 때 false
            CharacterManager.Instance.Player.controller.playerAnimator.SetTrigger("Attack");    // 공격 애니메이션 재생
            curEquip.OnAttackInput();
        }
    }
}
