using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] GameObject attackArea;
    [SerializeField] GameObject slashObject;
    private Transform playerTransform;

    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpSpeed = 8f;       
    [SerializeField] float jumpInterval = 3f;    
    [SerializeField] float attackInterval = 4f;  
    [SerializeField] float slashSpeed = 8f;      

    private bool isDead = false;
    private CancellationTokenSource cts;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cts = new CancellationTokenSource();

        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        _ = JumpRoutine(cts.Token);
        _ = AttackRoutine(cts.Token);
    }

    void Update()
    {
        if (isDead || playerTransform == null) return;

        
        MoveAndLookAtPlayer();

    }

    
    private void MoveAndLookAtPlayer()
    {

        float directionX = playerTransform.position.x - transform.position.x;

        if (directionX != 0f)
        {
            float moveDir = directionX > 0 ? 1f : -1f;
            rb.linearVelocityX = moveDir * moveSpeed;

            Vector3 localScale = transform.localScale;

            if (directionX < 0)
            {
                localScale.x = 1f;
            }
            else
            {
                localScale.x = -1f;
            }
            if (spriteRenderer != null) spriteRenderer.flipX = true;

            transform.localScale = localScale;
        }
        else
        {
            rb.linearVelocityX = 0f;
        }
    }

    void OnDestroy()
    {
        
        cts?.Cancel();
        cts?.Dispose();
    }

   
    private async UniTask JumpRoutine(CancellationToken token)
    {
        while (!isDead)
        {
         
            await UniTask.Delay(TimeSpan.FromSeconds(jumpInterval), cancellationToken: token);

            if (isDead) break;

            
            rb.linearVelocityY = jumpSpeed;
        }
    }

    private async UniTask AttackRoutine(CancellationToken token)
    {
        while (!isDead)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(attackInterval), cancellationToken: token);

            if (isDead) break;

            await EnemySlash(token);
        }
    }

    
    private async UniTask EnemySlash(CancellationToken token)
    {
        if (attackArea != null) attackArea.SetActive(true);
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);
        if (isDead) return;

        GameObject projectile = Instantiate(slashObject, transform.position, Quaternion.identity);

        Vector3 projScale = projectile.transform.localScale;
        projScale.x = transform.localScale.x;
        projectile.transform.localScale = projScale;

        
        if (projectile.TryGetComponent<Rigidbody2D>(out var slashRb))
        {
            float direction = transform.localScale.x < 0 ? 1f : -1f;
            slashRb.linearVelocityX = direction * slashSpeed;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);

        if (attackArea != null) attackArea.SetActive(false);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || !collision.CompareTag("PlayerAttack")) return;

        Die();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || !collision.gameObject.CompareTag("PlayerAttack")) return;

        Die();
    }

    private void Die()
    {
        isDead = true;
        cts?.Cancel(); 

       
        if (TryGetComponent<Collider2D>(out var col))
        {
            col.enabled = false;
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;


        
        _ = FlashAndDestroy();
    }

    
    private async UniTask FlashAndDestroy()
    {
        if (spriteRenderer != null)
        {
            
            for (int i = 0; i < 4; i++)
            {
                spriteRenderer.enabled = false;
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

                spriteRenderer.enabled = true;
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }
        else
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        
        Destroy(gameObject);
    }
}