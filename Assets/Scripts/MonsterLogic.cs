using Unity.Mathematics;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    private enum MonsterState
    {
        IDLE,
        CHASE,
        ELEVATE
    }

    private MonsterState monsterState = MonsterState.IDLE;

    [SerializeField]
    private Detector detector, buffer;
    private new Rigidbody rigidbody;

    private Vector3 velocity = Vector3.zero;

    const float ELEVATE_SPEED = 1.0f;
    const float FLOAT_AMPLITUDE = 0.6f;
    const float FALL_SPEED = 0.2f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        velocity.y = math.sin(Time.time) * FLOAT_AMPLITUDE;

        switch (monsterState)
        {
            case MonsterState.IDLE:
                if (!buffer.detected) transform.parent.Translate(FALL_SPEED * Time.deltaTime * Vector3.down);
                if (detector.detected) monsterState = MonsterState.ELEVATE;
                break;
            case MonsterState.ELEVATE:
                transform.parent.Translate(ELEVATE_SPEED * Time.deltaTime * Vector3.up);
                if (!detector.detected) monsterState = MonsterState.IDLE;
                break;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = velocity;
        velocity = rigidbody.velocity;
    }
}
