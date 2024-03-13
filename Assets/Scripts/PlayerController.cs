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
    }

    private void Move()
    {
        characterController.Move(SPEED * Time.deltaTime * InputDir);
    }

}
