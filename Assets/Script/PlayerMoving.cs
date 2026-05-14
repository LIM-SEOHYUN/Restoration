using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded = true;

    [Header ("AnimationController")]
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController runController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController hitController;
    public RuntimeAnimatorController ultimateController; //±Ã±Ø±â
    public RuntimeAnimatorController deadController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 vec = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Movement()
    {
        float moveInput = 0f;

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(moveInput != 0 && spriteRenderer != null )
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        if (isGrounded )
        {
            if(moveInput != 0)
            {
                SetAnimatorController(runController);
            }
            else
            {
                SetAnimatorController(idleController);
            }
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;

        SetAnimatorController(jumpController);
    }

    void SetAnimatorController(RuntimeAnimatorController controller)
    {
        //5°­ 44ÆäÀÌÁö
    }
}
