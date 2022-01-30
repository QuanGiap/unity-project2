using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    [SerializeField] private float MovementSpeed = 4f;
    [SerializeField] private float HorizontalInput = 0;
    [SerializeField] private float VerticalInput = 0;
    [SerializeField] bool isInvincible;
    [SerializeField] float invincibleTimer;
    [SerializeField] float timeInvincible = 2.0f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] ParticleSystem particle;
    [SerializeField] private UIHealthBar uIHealthBar;
    Rigidbody2D rigidbody2D;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Pressed");
            Launch();
        }
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }
    private void FixedUpdate()
    {
        Vector2 move = new Vector2(HorizontalInput, VerticalInput);
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        Vector2 position = rigidbody2D.position;
        position.x += MovementSpeed * HorizontalInput * Time.deltaTime;
        position.y += MovementSpeed * VerticalInput * Time.deltaTime;
        rigidbody2D.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            particle.Play();
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        uIHealthBar.ChangeSlideValue(amount);
    }
    void Launch()
    {
        print(lookDirection);
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Destroy(projectileObject, 3);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300f);
        animator.SetTrigger("Launch");
    }
}
