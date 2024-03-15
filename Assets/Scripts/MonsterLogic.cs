using Unity.Mathematics;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    private enum MonsterState
    {
        CHASE,
        ELEVATE
    }
    private MonsterState monsterState = MonsterState.CHASE;

    private bool isElevating = false;

    [SerializeField]
    private Detector detector, buffer;
    [SerializeField]
    private Transform body;
    private PlayerLogic player;

    private Vector3 velocity;
    private new Rigidbody rigidbody;

    const float SPEED = 3.0f;
    const float ELEVATE_SPEED = 1.0f;
    const float FLOAT_AMPLITUDE = 0.6f;
    const float FALL_SPEED = 2f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerLogic>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isElevating)
        {
            velocity.y = ELEVATE_SPEED;
            if (!detector.detected) isElevating = false;
        }
        else
        {
            if (!buffer.detected) velocity.y = -FALL_SPEED;
            else velocity.y = 0;
            if (detector.detected) isElevating = true;
        }

        var pos = player.transform.position;
        pos.y = transform.position.y;
        transform.LookAt(pos);

        var distance = player.transform.position - transform.position;
        var direction = distance.normalized;
        velocity.x = direction.x * SPEED;
        velocity.z = direction.z * SPEED;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = velocity;

        var bodyVelocity = Vector3.zero;
        bodyVelocity.y = math.sin(Time.time) * FLOAT_AMPLITUDE;
        body.Translate(bodyVelocity * Time.deltaTime);
    }
}
