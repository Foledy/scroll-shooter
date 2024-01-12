using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpOffset;
    [SerializeField] private LayerMask _groundMask;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float direction)
    {
        _rigidbody.velocity = new Vector2(_speed * direction, _rigidbody.velocity.y);
    }

    public void Jump()
    {
        if (IsGrounded() == true)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        }
    }

    private bool IsGrounded() => Physics2D.Raycast(transform.position, Vector2.down, _jumpOffset, _groundMask);
}
