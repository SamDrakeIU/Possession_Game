using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxMoveSpeed = 4.5f;      // Speed at 100%
    public float rotationSpeed = 10f;      // Turn speed
    public bool cameraRelative = true;     // Move relative to camera forward

    [Header("Blend Tree / Speed Control")]
    [Range(0f, 1f)]
    public float inputMultiplier = 1.0f;   // 0..1 (0%..100%)
    public float speedDampTime = 0.08f;    // Smooth blend changes

    [Header("Animation")]
    public float animationSpeed = 1.0f;

    private Animator animator;
    private CharacterController controller;

    private Vector3 moveInput;
    private float speedParam;

    private float verticalVelocity;
    private const float Gravity = -9.81f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator.SetTrigger("PlayAnim");

        // Read input (old input system axes)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(h, 0f, v);
        float inputMagnitude = Mathf.Clamp01(moveInput.magnitude);

        // Blend tree speed (0..1)
        speedParam = Mathf.Clamp01(inputMagnitude * inputMultiplier);

        // Push to Animator (parameter name must match your blend tree)
        
        

        // Convert to world direction (optionally camera-relative)
        Vector3 moveDir = GetMoveDirection(moveInput);

        // Rotate toward movement direction (based on input, not speed)
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        // Movement speed scales with speedParam
        float currentSpeed = maxMoveSpeed * speedParam;
        Vector3 horizontalMove = moveDir * currentSpeed;

        // Simple gravity for CharacterController
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // keep grounded

        verticalVelocity += Gravity * Time.deltaTime;

        Vector3 velocity = horizontalMove + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetMoveDirection(Vector3 rawInput)
    {
        if (rawInput.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        Vector3 dir = rawInput.normalized;

        if (!cameraRelative || Camera.main == null)
            return dir;

        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return (camForward * rawInput.z + camRight * rawInput.x).normalized;
    }
}