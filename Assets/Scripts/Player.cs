using UnityEngine;
using UnityEngine.InputSystem; // 1. ADD THIS LINE AT THE TOP

/*
    This script provides jumping and movement in Unity 3D - Gatsby
    Updated for the New Input System Package
*/

public class Player : MonoBehaviour
{
    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraTransform = Camera.main.transform;

        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 2. MODIFIED: Reading movement inputs via the New Input System
        if (Keyboard.current != null)
        {
            moveHorizontal = 0f;
            moveForward = 0f;

            if (Keyboard.current.wKey.isPressed) moveForward += 1f;
            if (Keyboard.current.sKey.isPressed) moveForward -= 1f;
            if (Keyboard.current.dKey.isPressed) moveHorizontal += 1f;
            if (Keyboard.current.aKey.isPressed) moveHorizontal -= 1f;
        }

        RotateCamera();

        // 3. MODIFIED: Reading jump inputs via the New Input System
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
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
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }


    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }
    
 void RotateCamera()
{
    if (Mouse.current != null)
    {
        // Vector2 reading is much more stable than raw frame delta values
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // We use Time.deltaTime to smooth out frame rate spikes
        float horizontalRotation = mouseDelta.x * mouseSensitivity * 5f * Time.deltaTime;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= mouseDelta.y * mouseSensitivity * 5f * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}


}

