using Cysharp.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

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
        

        if (playerInput.actions["Jump"].WasPressedThisFrame())
        {
            rb.linearVelocityY = jumpSpeed;
            animator.Play("Jump");
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

    private async UniTask slash()
    {
        attackArea.SetActive(true);

        animator.Play("Attack");

        await UniTask.Yield();

        Instantiate(slashObject, new Vector3(0, 0, 0), Quaternion.identity);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        attackArea.SetActive(false);
    }

}
