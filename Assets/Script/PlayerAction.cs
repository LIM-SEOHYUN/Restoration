using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    [Header("SkillPrefab")]
    public Transform attackSpawnPoint;          // 공격 시작 위치
    public GameObject normalAttackPrefab;       // 일반 공격 프리팹
    public GameObject rightClickSkillPrefab;    // 우클릭 스킬 프리팹
    public GameObject ultimatePrefab;           // 궁극기 프리팹

    public PlayerMoving playerMoving;           // 플레이어 이동 스크립트 참조
    private Animator animator;

    public CutsceneUI cutsceneUI;               // 컷씬 UI 제어 스크립트

    [Header("SkillCooldownUI")]
    public SkillCooldownUI rightSkillUI;        // 우클릭 스킬 쿨타임 UI
    public SkillCooldownUI ultimateSkillUI;     // 궁극기 쿨타임 UI

    private bool canUseUltimate = true;         // 궁극기 사용 가능 여부
    private bool canUseRightSkill = true;       // 우클릭 스킬 사용 가능 여부

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip normalAttackSound;         // 일반 공격 사운드
    public AudioClip rightSkillSound;           // 우클릭 스킬 사운드
    public AudioClip ultimateCastSound;         // 궁극기 시전 사운드
    public AudioClip ultimateAttackSound;       // 궁극기 공격 사운드

    void Awake()
    {
        DontDestroyOnLoad(gameObject);          // 씬 전환 시에도 유지
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 일반 상태일 때 스킬 실행 가능
        if (playerMoving.currentState == PlayerState.Normal)
        {
            if (Input.GetMouseButtonDown(0)) NormalAttack();
            if (Input.GetMouseButtonDown(1) && canUseRightSkill) NormalSkill();
            if (Input.GetKeyDown(KeyCode.E) && canUseUltimate) ActivateUltimate();
        }
        // 궁극기 상태일 때는 궁극기 공격만 가능
        else if (playerMoving.currentState == PlayerState.Ultimate)
        {
            if (Input.GetMouseButtonDown(0)) UltimateAttack();
        }
    }

    // 일반 공격
    void NormalAttack()
    {
        animator.SetTrigger("isAttacking");
        if (normalAttackSound != null) audioSource.PlayOneShot(normalAttackSound);
    }

    // 우클릭 스킬
    void NormalSkill()
    {
        animator.SetTrigger("isSkill");
        canUseRightSkill = false;
        rightSkillUI.StartCooldown(5f);              // 쿨타임 UI 시작
        StartCoroutine(RightSkillCooldown(5f));      // 쿨타임 코루틴 실행
        if (rightSkillSound != null) audioSource.PlayOneShot(rightSkillSound);
    }

    IEnumerator RightSkillCooldown(float cooldownTime)
    {
        canUseRightSkill = false;
        yield return new WaitForSeconds(cooldownTime);
        canUseRightSkill = true;
    }

    // 궁극기 공격 (궁극기 상태에서만 실행)
    void UltimateAttack()
    {
        animator.SetTrigger("ultimateAttack");
        SpawnUltimateSkill();
        audioSource.PlayOneShot(ultimateAttackSound);
    }

    // 궁극기 시전
    void ActivateUltimate()
    {
        playerMoving.currentState = PlayerState.Ultimate;
        animator.SetBool("isUltimate", true);

        if (cutsceneUI != null)
            cutsceneUI.ShowCutscene();               // 컷씬 연출

        ultimateSkillUI.SetGlow(true);               // 궁극기 아이콘 빛남 효과
        StartCoroutine(UltimateDuration(10f));       // 궁극기 유지 시간
        if (ultimateCastSound != null) audioSource.PlayOneShot(ultimateCastSound);
    }

    IEnumerator UltimateDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndUltimate();
    }

    // 궁극기 종료 → 일반 상태로 복귀
    void EndUltimate()
    {
        animator.SetTrigger("ultimateEnd");
        playerMoving.currentState = PlayerState.Normal;
        animator.SetBool("isUltimate", false);

        ultimateSkillUI.SetGlow(false);

        canUseUltimate = false;
        ultimateSkillUI.StartCooldown(30f);          // 궁극기 쿨타임 시작
        StartCoroutine(UltimateCooldown(30f));
    }

    IEnumerator UltimateCooldown(float cooldownTime)
    {
        canUseUltimate = false;
        yield return new WaitForSeconds(cooldownTime);
        canUseUltimate = true;
    }

    // 일반 공격 투사체 생성
    public void SpawnNormalAttack()
    {
        GameObject effect = Instantiate(normalAttackPrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;
        Vector3 scale = effect.transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        effect.transform.localScale = scale;

        Rigidbody2D rb = effect.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = (facingLeft ? Vector2.left : Vector2.right) * 10f;

        Projectile projectile = effect.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 100;
    }

    // 우클릭 스킬 투사체 생성
    public void SpawnRightClickSkill()
    {
        GameObject skill = Instantiate(rightClickSkillPrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;
        Vector3 scale = skill.transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        skill.transform.localScale = scale;

        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = (facingLeft ? Vector2.left : Vector2.right) * 15f;

        Projectile projectile = skill.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 200;
    }

    // 궁극기 투사체 생성
    public void SpawnUltimateSkill()
    {
        GameObject ultimate = Instantiate(ultimatePrefab, attackSpawnPoint.position, Quaternion.identity);

        bool facingLeft = GetComponent<SpriteRenderer>().flipX;
        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;

        ultimate.GetComponent<UltimateProjectile>().SetDirection(dir);

        Projectile projectile = ultimate.GetComponent<Projectile>();
        if (projectile != null)
            projectile.baseDamage = 300;
    }

    // 사운드 재생 함수
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    // 쿨타임 초기화 (부활 시 사용)
    public void ResetCooldowns()
    {
        canUseRightSkill = true;
        canUseUltimate = true;
    }
}
