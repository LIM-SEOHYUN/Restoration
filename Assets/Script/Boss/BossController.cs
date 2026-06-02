using UnityEngine;

public class BossController : MonoBehaviour
{
    // 이동 및 행동 관련 변수
    public float moveSpeed = 2f;          // 보스 이동 속도
    public float wanderRadius = 80f;      // 배회할 수 있는 범위
    public float detectRange = 12f;       // 플레이어 탐지 범위
    public float attackRange = 10f;       // 공격 범위

    // 궁극기(스킬) 관련 변수
    public float castCooldown = 20f;      // 스킬 시전 쿨타임
    private float lastCastTime;           // 마지막 스킬 시전 시간 기록

    // 컴포넌트 및 오브젝트 참조
    public Animator animator;             // 애니메이터 (애니메이션 제어)
    public Transform player;              // 플레이어 위치 참조
    public GameObject spellPrefab;        // 생성할 스킬 프리팹

    private Rigidbody2D rb;               // 물리 이동 제어
    private SpriteRenderer sr;            // 스프라이트 방향 제어
    private Vector2 walkDirection;        // 걷는 방향
    private Vector3 wanderTarget;         // 배회 목표 위치

    // 스킬 생성 위치 관련 변수
    public float spellSpawnRangeX = 50f;  // 스킬 생성 X 범위
    public float spellSpawnHeight = 10f;  // 스킬 생성 Y 높이

    [Header("Audio")]
    public AudioSource audioSource;       // 오디오 소스
    public AudioClip statueDisappearClip; // 석상 사라질 때 소리
    public AudioClip spellCastClip;       // 스킬 시전 소리
    public AudioClip attackClip;          // 일반 공격 소리
    public AudioClip hurtClip;            // 보스 피격 소리
    public AudioClip bossAttackClip;      // 보스 공격 소리
    public AudioClip bossCastClip;        // 보스 시전 소리

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // 초기 배회 목표 설정
        ChooseNewWanderTarget();
        walkDirection = new Vector2(Random.value > 0.5f ? 1f : -1f, 0f);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // 궁극기 쿨타임 체크 → 일정 시간마다 시전 애니메이션 실행
        if (Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("isCast");
            lastCastTime = Time.time;
        }

        if (dist <= detectRange) // 플레이어 탐지 범위 안에 있으면 추적
        {
            Vector2 targetPos = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
            rb.MovePosition(targetPos);

            animator.SetBool("isWalking", true);
            sr.flipX = player.position.x < transform.position.x;

            if (dist <= attackRange) // 공격 범위 안에 있으면 공격
            {
                animator.SetTrigger("isAttacking");
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            // 탐지 범위 밖이면 배회
            Wander();
        }
    }

    void Wander()
    {
        // 목표 지점에 거의 도달하면 새로운 목표 설정
        if (Vector2.Distance(transform.position, wanderTarget) < 1f)
        {
            ChooseNewWanderTarget();
        }

        // 목표 지점으로 이동
        Vector2 targetPos = Vector2.MoveTowards(rb.position, wanderTarget, moveSpeed * Time.deltaTime);
        rb.MovePosition(targetPos);

        animator.SetBool("isWalking", true);
        sr.flipX = wanderTarget.x < transform.position.x;
    }

    void ChooseNewWanderTarget()
    {
        // 새로운 배회 목표 위치 랜덤 설정
        wanderTarget = new Vector2(
            Random.Range(-wanderRadius, wanderRadius),
            transform.position.y
        );
    }

    // 공격 애니메이션 종료 시 호출
    public void EndAttack()
    {
        animator.ResetTrigger("isAttacking");
        animator.SetBool("isWalking", true);
    }

    // 시전 애니메이션 종료 시 호출
    public void EndCast()
    {
        animator.ResetTrigger("isCast");
        animator.SetBool("isWalking", true);
    }

    // 스킬 시전 → 여러 개의 스펠 생성 후 3초 뒤 파괴
    public void CastSpell()
    {
        PlaySound(spellCastClip);
        for (int i = 0; i < 15; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-spellSpawnRangeX, spellSpawnRangeX),
                spellSpawnHeight,
                0
            );
            GameObject spell = Instantiate(spellPrefab, pos, Quaternion.identity);
            Destroy(spell, 3f);
        }
    }

    // 공격 데미지 처리
    public void DealDamage()
    {
        PlaySound(attackClip);
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(100);
            animator.SetBool("isWalking", true);
        }
    }

    // 보스가 데미지를 입었을 때
    public void TakeDamage(int dmg) => PlaySound(hurtClip);

    // 애니메이션 이벤트에서 호출할 사운드 함수들
    public void PlayBossCastSound() => PlaySound(bossCastClip);
    public void PlayBossAttackSound() => PlaySound(bossAttackClip);
    public void PlayStatueDisappearSound() => PlaySound(statueDisappearClip);

    // 사운드 재생 공통 함수
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
