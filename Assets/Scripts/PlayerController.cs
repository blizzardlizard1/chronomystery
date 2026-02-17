using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;

    private Vector3 input;
    private bool movementLocked = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (movementLocked)
        {
            input = Vector3.zero;
            return;
        }

        GatherInput();
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    void Look()
    {
        if (input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(input);
            var relative = transform.position + skewedInput - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        if (input == Vector3.zero)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            return;
        }

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(input);
        var desiredVelocity = skewedInput * speed;
        _rb.velocity = new Vector3(desiredVelocity.x, _rb.velocity.y, desiredVelocity.z);
    }

    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;
        // Kill velocity immediately when locking
        if (locked) _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
    }
}