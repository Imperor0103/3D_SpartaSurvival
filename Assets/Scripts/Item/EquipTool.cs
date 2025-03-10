using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격, 자원채취
public class EquipTool : Equip
{
    public float attackRate;    // 공격주기(공격 애니메이션 재생시간)
    private bool attacking;     // 공격중인가?
    public float attackDistance;    // 최대 공격 가능한 거리

    public float useStamina;    // 1회 행동할때 스테미나 소모량


    [Header("Resource Gathering")]
    public bool doesGatherResources;    /// 옵션1.자원 채취 가능한가?(Gather가 가능한가?)

    [Header("Combat")]
    public bool doesDealDamage; /// 옵션2.데미지를 줄 수 있는가?(공격을 하는건가?
    public int damage;     // 데미지 양


    private Camera camera;  // ray를 쏘는 카메라(메인 카메라)


    private void Start()
    {
        camera = Camera.main;

        attackRate = CharacterManager.Instance.Player.controller.clipLength; ;    // PlayerMeleeAttack의 재생시간
    }
    public override void OnAttackInput()
    {
        // isAttacking이 false일때만 내부로직 실행
        if (!attacking)
        {
            // 스테미나가 남았을 때만 사용가능
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                Invoke("OnCanAttack", attackRate);      /// 시간차(공격주기마다 한번 호출)
            }
        }
    }
    // 공격 애니메이션 끝
    void OnCanAttack()
    {
        attacking = false;
        CharacterManager.Instance.Player.controller.playerAnimator.SetBool("AttackFinishing", true);   // 공격 시작할때는 true, 공격 끝날 때 false
        // 애니메이션 방식이 변경되었으므로 OnHit을 직접 호출한다
        OnHit();
    }

    // 이벤트 함수
    // aim 기준으로 때렸을 때 EquipTool마다 attackDistance 이내에 있으면 호출
    /// <summary>
    /// 이젠 무기에 애니메이션이 없어졌고, 플레이어가 애니메이션을 담당하는데
    /// 플레이어의 애니메이션이 끝나고 이 메서드를 호출하게 한다
    /// 
    /// </summary>
    public void OnHit()
    {
        // 1인칭, 3인칭일 때 로직은 달라야 한다
        Ray ray = GetAttackRay();
        ProcessHit(ray);
    }
    // 1인칭, 3인칭에 따라 Ray 생성
    private Ray GetAttackRay()
    {
        if (!Helper.isThirdPerson) // 1인칭
            return camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        else // 3인칭
            return camera.ScreenPointToRay(Input.mousePosition);
    }
    // Raycast 후 적절한 상호작용 처리
    private void ProcessHit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            // 충돌한 지점의 월드 좌표
            Vector3 worldPosition = hit.point;
            Debug.Log($"EquipTool에서 호출, 마우스 클릭 위치 (월드 좌표): {worldPosition}");

            HandleResourceGathering(hit);
            HandleCombat(hit);
        }
    }
    // 자원 채취
    private void HandleResourceGathering(RaycastHit hit)
    {
        if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            resource.Gather(hit.point, hit.normal);
    }
    // 공격 
    private void HandleCombat(RaycastHit hit)
    {
        if (doesDealDamage && hit.collider.TryGetComponent(out NPC monster))
            monster.TakePhysicalDamage(damage);
    }
}
