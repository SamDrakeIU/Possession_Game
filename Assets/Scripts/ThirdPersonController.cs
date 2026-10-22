using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 0.1f;
    private float turnSmoothVelocity;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float distance = 5f;
    public float height = 2f;
    public float minPitch = -35f;
    public float maxPitch = 60f;

    private CharacterController controller;
    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // If camera transform isn't assigned, grab the main camera automatically
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Lock cursor to the game window for playtesting (Press Escape to release)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraInput();
        HandlePlayerMovement();
    }

    void HandleCameraInput()
    {
        // Get mouse input for looking around
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void HandlePlayerMovement()
    {
        // Get WASD / Controller inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate angle relative to the camera's facing direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed);

            // Rotate player to face movement direction
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move in the direction the camera is facing
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Apply slight gravity to keep character grounded
        controller.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Calculate camera position based on rotation angles and distance
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = transform.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

        // Update camera position and look rotation
        cameraTransform.position = targetPosition;
        cameraTransform.LookAt(transform.position + Vector3.up * 1.2f);
    }
}