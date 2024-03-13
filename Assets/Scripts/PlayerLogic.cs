using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public bool isMovable => unmovableTimer <= 0;
    private float unmovableTimer = 0f;

    private CharacterController characterController;

    public Vector3 velocity;

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
        UpdateTimers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateTimers()
    {
        if (unmovableTimer > 0)
            unmovableTimer -= Time.deltaTime;
        else
            unmovableTimer = 0;
    }

    private void UpdateInput()
    {
        if (isMovable)
        {
            velocity.x = Input.GetAxis("Horizontal") * SPEED;
            velocity.z = Input.GetAxis("Vertical") * SPEED;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (characterController.isGrounded)
                    velocity.y = JUMP_SPEED;
            }
        }

        velocity.y -= GRAVITY * Time.deltaTime;

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


    public void SetUnmovable(float time)
    {
        unmovableTimer = time;
    }

}
