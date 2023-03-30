using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private const float Speed = 10f;
    [SerializeField] LayerMask _groundLayer;
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private bool _grounded = false;
    private float _move = 0; // {-1, 0, 1}

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _body.velocity = new Vector2(_move * Speed, _body.velocity.y);
    }

/*    public bool canAttack()
    {

    }
*/
    public void MoveRight(InputAction.CallbackContext context)
    {
        bool run = !context.canceled;
        _move = (run ? 1 : 0);
        transform.localScale = new Vector2(1, 1);
        _animator.SetBool("run", run);
    }


    public void MoveLeft(InputAction.CallbackContext context)
    {
        bool run = !context.canceled;
        _move = (run ? -1 : 0);
        transform.localScale = new Vector2(-1, 1);
        _animator.SetBool("run", run);
    }

    public bool canAttack()
    {
        return _move == 0 && isGrounded();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            Debug.Log("Jumped!");
            _body.velocity = new Vector2(_body.velocity.x, Speed);

            _animator.SetBool("grounded", isGrounded());
            _animator.SetTrigger("jump");
        }
    }



    private bool isGrounded()
    {
        RaycastHit2D rayHit2D = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, _groundLayer);
        if (rayHit2D.collider != null)
        {
            Debug.Log("Player is standing on the " + rayHit2D.collider.name);
        }
        return rayHit2D.collider != null;
    }
}
