using UnityEngine;

public class CameraMovement : MonoBehaviour

{

    public Transform target;

    public float distance = 6f;

    public float sensitivity = 3f;

    public float minPitch = 0f;

    public float maxPitch = 60f;

    public float overhead;

    private Vector3 look;
    float yaw;

    float pitch;

    void Start()

    {

        if (!target) return;

        look = new Vector3 (0, overhead, 0);

        Vector3 dir = (transform.position - target.position).normalized;

        //distance = Vector3.Distance(transform.position, target.position);

        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;

    }

    void LateUpdate()

    {

        if (!target) return;

        yaw   += Input.GetAxis("Mouse X") * sensitivity;

        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

        // Place camera exactly distance away

        Vector3 offset = rot * new Vector3(0f, 0f, -distance);

        transform.position = target.position + offset;

        // Look at target

        transform.LookAt(target.position + look);

    }

}