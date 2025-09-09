using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Mouvement
    private Rigidbody rb;
    private float moveSpeed = 5f;
    private float moveHorizontal;
    private float moveVertical;

    // Saut
    private float jumpForce = 10f;
    private float fallMultiplier = 2.5f;
    private float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float raycastDistance = 1.2f;

    // Animations
    private Animator animator;

    // Model Rotation
    public Transform model;
    private float rotationSpeed = 15f;

    // Hp and hit logic
    private float inviTime = 1f;
    private float inviTimer;
    private int maxHp = 5;
    private int currentHp;
    public Image healthBar;

    // Attack
    private float atkTime = 0.7f;
    private float atkTimer;
    public BoxCollider swordHitBox;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
        currentHp = maxHp;
        inviTimer = inviTime;
        atkTimer = atkTime;
        swordHitBox.enabled = false;
    }

    private void Update()
    {
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("GameOver");
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        if (inviTimer > 0)
        {
            inviTimer -= Time.deltaTime;
        }

        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("Attack");
                atkTimer = atkTime;
                StartCoroutine(Attack());
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
        HandleRunAnimation();
        HandleModelRotation();
    }

    private void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;

        if (isGrounded && moveHorizontal == 0 && moveVertical == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    private void HandleRunAnimation()
    {
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        if (rb.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);
        }
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void HandleModelRotation()
    {
        Vector3 orientation = transform.forward * moveVertical + transform.right * moveHorizontal;
        if (orientation == Vector3.zero)
        {
            orientation = model.forward;
        }

        Quaternion rotation = Quaternion.LookRotation(orientation);
        Quaternion modelRotation = Quaternion.Slerp(model.rotation, rotation, rotationSpeed * Time.deltaTime);

        model.rotation = modelRotation;
    }

    public void GetHit(int damage)
    {
        if (inviTimer <= 0)
        {
            currentHp -= damage;

            inviTimer = inviTime;
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.4f);
        swordHitBox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        swordHitBox.enabled = false;
    }

    private void UpdateHealthBar()
    {
        float healthRatio = (float)currentHp / maxHp;
        healthBar.fillAmount = healthRatio;

        if (healthRatio <= 0.5f)
        {
            healthBar.color = Color.red;
        }
    }
}