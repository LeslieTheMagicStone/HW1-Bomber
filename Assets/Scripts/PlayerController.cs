using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    Vector3 velocity;

    const float SPEED = 5.0f;
    const float GRAVITY = 30.0f;
    const float JUMP_SPEED = 15.0f;

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
        velocity.x = Input.GetAxis("Horizontal") * SPEED;
        velocity.z = Input.GetAxis("Vertical") * SPEED;

        velocity.y -= GRAVITY * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (characterController.isGrounded)
                velocity.y = JUMP_SPEED;
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 distance = mousePos - playerPos;

        float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -90 - degree, 0);
    }

    private void Move()
    {
        characterController.Move(velocity * Time.deltaTime);
        velocity = characterController.velocity;
    }

}
