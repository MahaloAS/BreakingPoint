using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private float groundCheckRadius = 0.2f;
    private Vector3 moveDirection;
    public Transform mainCameraTransform;
    private Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckRadius, Color.green);
        float moveInputHorizontal = Input.GetAxis("Horizontal");
        float moveInputVertical = Input.GetAxis("Vertical");
        anim.SetFloat("MoveX", moveInputHorizontal);
        anim.SetFloat("MoveZ", moveInputVertical);
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        moveDirection = (forward.normalized * moveInputVertical + right.normalized * moveInputHorizontal).normalized;
        moveDirection *= moveSpeed;
        moveDirection.y = rb.velocity.y;
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15f);
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            moveDirection.y = jumpForce;
            anim.SetBool("Jump", true);
        }
        rb.velocity = moveDirection;
    }
}