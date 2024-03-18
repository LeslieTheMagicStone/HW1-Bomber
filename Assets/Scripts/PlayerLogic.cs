using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public bool isMovable => unmovableTimer <= 0;
    private float unmovableTimer = 0f;

    private CharacterController characterController;

    [SerializeField] private Detector foot, buffer, jumpDetector;
    [SerializeField] private ParticleSystem smoke;

    public Vector3 velocity;

    private float speed = 5.0f;
    const float GRAVITY = 50.0f;
    const float JUMP_HEIGHT = 2.0f;
    // v0^2/2g = h => v0 = sqrt(2gh)
    readonly float JUMP_SPEED = Mathf.Sqrt(2 * GRAVITY * JUMP_HEIGHT);

    const float INPUT_ROTATION_DEGREE = 45f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Upgrade"))
        {
            var upgrade = other.GetComponent<Upgrade>();
            switch (upgrade.effect)
            {
                case UpgradeEffect.SPEED_UP:
                    speed *= 1.2f;
                    break;
                case UpgradeEffect.REDUCE_CD:
                    GetComponentInChildren<BombShooter>().maxShootCooldown *= 0.8f;
                    break;
                case UpgradeEffect.CURE:
                    var dam = GetComponent<Damageable>();
                    dam.health = Mathf.Max(dam.maxHealth, dam.health + 50);
                    break;
            }
            Instantiate(upgrade.upgradeParticle, upgrade.transform.position, upgrade.transform.rotation);
            AudioManager.instance.Play(upgrade.upgradeSound);
            Destroy(upgrade.gameObject);
        }
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
            velocity.x = Input.GetAxis("Horizontal") * speed;
            velocity.z = Input.GetAxis("Vertical") * speed;
            velocity = Quaternion.Euler(0f, INPUT_ROTATION_DEGREE, 0f) * velocity;

            if (!foot.detected && !buffer.detected) velocity.y -= GRAVITY * Time.deltaTime;
            if (buffer.detected) velocity.y = 0;
            if (foot.detected) velocity.y = 1;

            if (foot.detected && velocity.x != 0f)
            {
                if (!smoke.isPlaying) smoke.Play();
            }
            else
            {
                if (smoke.isPlaying) smoke.Stop();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpDetector.detected)
                    velocity.y = JUMP_SPEED;
            }
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        var pos = hit.point;
        pos.y = transform.position.y;
        transform.LookAt(pos);

        /* Legacy */
        // Vector3 mousePos = Input.mousePosition;
        // Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        // Vector3 distance = mousePos - playerPos;

        // float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, INPUT_ROTATION_DEGREE + 90 - degree, 0);
    }

    private void Move()
    {
        characterController.Move(velocity * Time.deltaTime);
        velocity = characterController.velocity;
    }


    public void SetUnmovable(float time)
    {
        if (time > unmovableTimer)
            unmovableTimer = time;
    }
}
