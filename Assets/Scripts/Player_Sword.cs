using Cysharp.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player_Sword : MonoBehaviour
{

    PlayerInput playerInput;
    Rigidbody2D rb;

    [SerializeField]
    GameObject attackArea;

    [SerializeField]
    GameObject slashObject;

    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float slashSpeed = 10f;
    private bool isDead = false;

    private int jumpCount = 0;
    [SerializeField] int maxJumpCount = 3;

    [SerializeField]
    Animator animator;

    public Vector3 localScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (playerInput.actions["Jump"].WasPressedThisFrame())
        {
            
            if (jumpCount < maxJumpCount)
            {
                rb.linearVelocityY = jumpSpeed;
                animator.Play("Jump");

                jumpCount++; 
            }
        }

        var move = playerInput.actions["Move"].ReadValue<Vector2>();
        if (move.x != 0f)
        {
            animator.Play("Run");
            rb.linearVelocityX = move.x * speed;

            localScale = transform.localScale;
            if (move.x < 0)
            {
                localScale.x = 1f;
            }
            else
            {
                localScale.x = -1f;
            }
            transform.localScale = localScale;
        }

        if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            _ = slash();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Floor"))
        {
            jumpCount = 0;
        }
    }

    private async UniTask slash()
    {
        attackArea.SetActive(true);
        animator.Play("Attack");

        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));

        GameObject projectile = Instantiate(slashObject, transform.position, Quaternion.identity);

        Vector3 projScale = projectile.transform.localScale;
        projScale.x = transform.localScale.x;
        projectile.transform.localScale = projScale;

        
        if (projectile.TryGetComponent<Rigidbody2D>(out var slashRb))
        {
            float direction = transform.localScale.x < 0 ? 1f : -1f;
            slashRb.linearVelocityX = direction * slashSpeed;
        }
       
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (isDead || !collision.CompareTag("Enemy")) return;

        
        _ = GameOver();
    }

    
    private async UniTask GameOver()
    {
        isDead = true; 

        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; 

        
        animator.Play("Die");

        
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));

       
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
