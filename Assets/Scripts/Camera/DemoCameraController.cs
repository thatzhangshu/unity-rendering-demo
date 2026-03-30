using UnityEngine;

public class DemoCameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float lookSpeed = 120f;
    [SerializeField] private float boostMultiplier = 2f;

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        Vector3 euler = transform.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;
    }

    private void Update()
    {
        HandleMove();
        HandleLook();
    }

    private void HandleMove()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * boostMultiplier : moveSpeed;

        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) input += transform.forward;
        if (Input.GetKey(KeyCode.S)) input -= transform.forward;
        if (Input.GetKey(KeyCode.A)) input -= transform.right;
        if (Input.GetKey(KeyCode.D)) input += transform.right;
        if (Input.GetKey(KeyCode.E)) input += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) input -= Vector3.up;

        input.Normalize();
        transform.position += input * speed * Time.deltaTime;
    }

    private void HandleLook()
    {
        if (!Input.GetMouseButton(1))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * lookSpeed * Time.deltaTime;
        _pitch -= mouseY * lookSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }
}