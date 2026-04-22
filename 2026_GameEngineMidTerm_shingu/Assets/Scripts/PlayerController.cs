using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;
    private float moveInput;

    // =====================
    // Item: Jump Boost
    // =====================
    [Header("Item: Jump Boost")]
    public float jumpBoostMultiplier = 1.5f;
    public float jumpBoostTime = 5f;
    private float originalJumpForce;

    // =====================
    // Item: Speed Boost
    // =====================
    [Header("Item: Speed Boost")]
    public float speedBoostMultiplier = 2f;
    public float speedBoostTime = 5f;
    private float originalMoveSpeed;
    private bool isMakingGhost = false;

    // =====================
    // Item: Invincibility
    // =====================
    [Header("Item: Invincibility")]
    public bool isInvincible = false;
    public float invincibilityTime = 3f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        originalJumpForce = jumpForce;
        originalMoveSpeed = moveSpeed;
    }

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
    }


    public void OnJump(InputValue value)
    {

        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if(isInvincible == false)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }

        if (collision.CompareTag("Finish"))
        {

            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (isInvincible) return;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // =====================
        // Item 처리
        // =====================
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);

            isInvincible = true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

            Invoke(nameof(ResetInvincibility), invincibilityTime);
            return;
        }

        if (collision.CompareTag("SpeedItem"))
        {
            Destroy(collision.gameObject);

            moveSpeed = originalMoveSpeed * speedBoostMultiplier;
            isMakingGhost = true;

            Invoke(nameof(ResetSpeed), speedBoostTime);
            return;
        }

        if (collision.CompareTag("JumpItem"))
        {
            Destroy(collision.gameObject);

            jumpForce = originalJumpForce * jumpBoostMultiplier;

            // 색 변경
            spriteRenderer.color = new Color(0.5f, 1f, 0.5f, 1f);

            Invoke(nameof(ResetJump), jumpBoostTime);
            return;
        }
    }

    void ResetInvincibility()
    {
        isInvincible = false;
        spriteRenderer.color = Color.white;
    }

    void ResetSpeed()
    {
        moveSpeed = originalMoveSpeed;
        isMakingGhost = false;
    }

    void ResetJump()
    {
        jumpForce = originalJumpForce;
        spriteRenderer.color = Color.white;
    }
}