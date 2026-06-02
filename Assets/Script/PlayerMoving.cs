using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float moveSpeed = 5f;              // 기본 이동 속도
    public float jumpForce = 7f;              // 점프 힘
    private Rigidbody2D rb;                   // 물리 이동 제어
    private SpriteRenderer spriteRenderer;    // 캐릭터 방향 전환
    private Animator animator;                // 애니메이션 제어
    private bool isGrounded = true;           // 땅에 닿아있는지 여부

    public Transform attackSpawnPoint;        // 공격 시작 위치 (칼끝 등)

    public PlayerState currentState = PlayerState.Normal; // 플레이어 상태 (Normal, Ultimate, Dead 등)

    [Header("Audio")]
    public AudioSource audioSource;           // 오디오 소스
    public AudioClip footstepSound;           // 발소리 효과음

    void Awake()
    {
        DontDestroyOnLoad(gameObject);        // 씬 전환 시에도 유지
    }

    // 발소리 효과음 재생
    public void PlayFootstepSound()
    {
        if (footstepSound != null)
            audioSource.PlayOneShot(footstepSound);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Dead 상태가 아닐 때만 이동/점프 가능
        if (currentState != PlayerState.Dead)
        {
            Movement();
            Jump();
        }
    }

    // 좌우 이동 처리
    void Movement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = (currentState == PlayerState.Ultimate) ? moveSpeed * 1.5f : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            // 방향 전환
            spriteRenderer.flipX = moveInput < 0;
            animator.SetBool("isRunning", true);

            // 발소리 루프 재생
            if (!audioSource.isPlaying)
            {
                audioSource.clip = footstepSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            UpdateSpawnPoint(); // 공격 위치 업데이트
        }
        else
        {
            animator.SetBool("isRunning", false);

            // 이동 멈추면 발소리 중지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // 점프 처리
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }

    // 땅에 닿으면 점프 상태 해제
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>() != null)
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    // 공격 위치(칼끝) 업데이트
    void UpdateSpawnPoint()
    {
        if (spriteRenderer.flipX)
            attackSpawnPoint.localPosition = new Vector3(-0.5f, 0, 0); // 왼쪽 칼끝
        else
            attackSpawnPoint.localPosition = new Vector3(+0.5f, 0, 0); // 오른쪽 칼끝
    }
}
