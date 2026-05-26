using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    [Header("SkillPrefab")]
    public Transform attackSpawnPoint;
    public GameObject normalAttackPrefab;
    public GameObject rightClickSkillPrefab;
    public GameObject ultimatePrefab;

    public PlayerMoving playerMoving;
    private Animator animator;

    public CutsceneUI cutsceneUI;// 컷씬 UI 제어 스크립트

    [Header("SkillCooldownUI")]
    public SkillCooldownUI rightSkillUI;//스킬 쿨타임 패널
    public SkillCooldownUI ultimateSkillUI;

    private bool canUseUltimate = true; //궁극기가 실행되었는지
    private bool canUseRightSkill = true; //우클릭 스킬

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerMoving.currentState == PlayerState.Normal) //스킬 실행
        {
            if (Input.GetMouseButtonDown(0)) NormalAttack();
            if (Input.GetMouseButtonDown(1) && canUseRightSkill) NormalSkill();
            if (Input.GetKeyDown(KeyCode.E) && canUseUltimate) ActivateUltimate();
        }
        else if (playerMoving.currentState == PlayerState.Ultimate)
        {
            if (Input.GetMouseButtonDown(0)) UltimateAttack();
        }
    }

    void NormalAttack() //일반공격
    {
        animator.SetTrigger("isAttacking");
    }

    void NormalSkill() //우클릭 스킬
    {
        animator.SetTrigger("isSkill");
        canUseRightSkill = false;
        rightSkillUI.StartCooldown(5f);
        StartCoroutine(RightSkillCooldown(5f));
    }

    IEnumerator RightSkillCooldown(float cooldownTime) //우클릭 쿨타임
    {
        canUseRightSkill = false;
        yield return new WaitForSeconds(cooldownTime);
        canUseRightSkill = true;
    }

    void UltimateAttack()//궁극기 시전 후
    {
        animator.SetTrigger("ultimateAttack");
        SpawnUltimateSkill();
    }

    void ActivateUltimate()//궁극기 시전
    {
        playerMoving.currentState = PlayerState.Ultimate;
        animator.SetBool("isUltimate", true);
        cutsceneUI.ShowCutscene(); // 컷씬 실행

        ultimateSkillUI.SetGlow(true);//궁극기 시전 시 아이콘 빛남 효과
        StartCoroutine(UltimateDuration(10f));
    }

    IEnumerator UltimateDuration(float duration)//궁극기 유지
    {
        yield return new WaitForSeconds(duration);
        EndUltimate();
    }

    void EndUltimate()//다시 일반 상태로 돌아옴
    {
        animator.SetTrigger("ultimateEnd");
        playerMoving.currentState = PlayerState.Normal;
        animator.SetBool("isUltimate", false);

        ultimateSkillUI.SetGlow(false);

        canUseUltimate = false;
        ultimateSkillUI.StartCooldown(30f);
        StartCoroutine(UltimateCooldown(30f));//궁극기 시전 종료 시점에 쿨타임 다시 시작
    }

    IEnumerator UltimateCooldown(float cooldownTime)//궁극기 쿨타임
    {
        canUseUltimate = false;
        yield return new WaitForSeconds(cooldownTime);
        canUseUltimate = true;
    }

    public void SpawnNormalAttack()
    {
        GameObject effect = Instantiate(normalAttackPrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;

        Vector3 scale = effect.transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        effect.transform.localScale = scale;

        Rigidbody2D rb = effect.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = (facingLeft ? Vector2.left : Vector2.right) * 10f;
        }

        Projectile projectile = effect.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 10;
    }

    public void SpawnRightClickSkill()
    {
        GameObject skill = Instantiate(rightClickSkillPrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;

        Vector3 scale = skill.transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        skill.transform.localScale = scale;

        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = (facingLeft ? Vector2.left : Vector2.right) * 15f;
        }

        Projectile projectile = skill.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 20;
    }


    public void SpawnUltimateSkill()
    {
        GameObject ultimate = Instantiate(ultimatePrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;
        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;

        ultimate.GetComponent<UltimateProjectile>().SetDirection(dir);

        Projectile projectile = ultimate.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 50;
    }
}
