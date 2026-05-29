using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Sword : MonoBehaviour
{

    PlayerInput playerInput;
    Rigidbody2D rb;

    [SerializeField]
    Collider2D attackArea;

    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

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
        }

        var move = playerInput.actions["Move"].ReadValue<Vector2>();
        if (move.x != 0f)
        {
            rb.linearVelocityX = move.x * speed;

            var localScale = transform.localScale;
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

        }
    }
}
