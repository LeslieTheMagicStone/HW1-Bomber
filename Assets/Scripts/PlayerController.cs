using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 InputDir;

    const float SPEED = 5.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        InputDir = new(horizontalInput, 0, verticalInput);

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 distance = mousePos - playerPos;

        float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -90 - degree, 0);
    }

    private void Move()
    {
        characterController.SimpleMove(SPEED * InputDir);
    }

}
