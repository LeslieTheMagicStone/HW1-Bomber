using Unity.Mathematics;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    private enum MonsterState
    {
        CHASE,
        ATTACK
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
    const float ATTACK_RADIUS = 2f;
    const float ATTACK_DURATION = 1.0f;

    private float behaviorTimer = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1.0f, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, ATTACK_RADIUS);
    }

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

        switch (monsterState)
        {
            case MonsterState.CHASE:
                var distance = player.transform.position - transform.position;
                var direction = distance.normalized;
                velocity.x = direction.x * SPEED;
                velocity.z = direction.z * SPEED;

                if (distance.magnitude < ATTACK_RADIUS)
                {
                    monsterState = MonsterState.ATTACK;
                    behaviorTimer = 0f;
                }

                break;
            case MonsterState.ATTACK:
                rigidbody.isKinematic = true;
                if (behaviorTimer < ATTACK_DURATION * 0.2f)
                {
                    // Go back a bit.
                    velocity = -transform.forward * 1f;
                    behaviorTimer += Time.deltaTime;
                }
                else if (behaviorTimer < ATTACK_DURATION * 0.4f)
                {
                    // Dash forward.
                    velocity = transform.forward * 5f;
                    behaviorTimer += Time.deltaTime;
                }
                else if (behaviorTimer < ATTACK_DURATION * 0.8f)
                {
                    // Go back.
                    velocity = -transform.forward * 1f;
                    behaviorTimer += Time.deltaTime;
                }
                else if (behaviorTimer < ATTACK_DURATION * 1.0f)
                {
                    // Take a rest.
                    velocity = Vector3.zero;
                    behaviorTimer += Time.deltaTime;
                }
                else
                {
                    rigidbody.isKinematic = false;
                    monsterState = MonsterState.CHASE;
                    behaviorTimer = 0f;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!rigidbody.isKinematic)
            rigidbody.velocity = velocity;
        else
            transform.Translate(velocity*Time.deltaTime);

        var bodyVelocity = Vector3.zero;
        bodyVelocity.y = math.sin(Time.time) * FLOAT_AMPLITUDE;
        body.Translate(bodyVelocity * Time.deltaTime);
    }
}
