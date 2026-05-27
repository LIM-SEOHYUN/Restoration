using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded = true;

    public Transform attackSpawnPoint;

    public PlayerState currentState = PlayerState.Normal;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip footstepSound;

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
        if (currentState != PlayerState.Dead)
        {
            Movement();
            Jump();
        }
    }

    void Movement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = (currentState == PlayerState.Ultimate) ? moveSpeed * 1.5f : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
            animator.SetBool("isRunning", true);

            UpdateSpawnPoint();
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>() != null)
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }
    void UpdateSpawnPoint()
    {
        if (spriteRenderer.flipX)
            attackSpawnPoint.localPosition = new Vector3(-0.5f, 0, 0); // 哭率 漠场
        else
            attackSpawnPoint.localPosition = new Vector3(+0.5f, 0, 0); // 坷弗率 漠场
    }



}
