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
    private Detector detector;
    private CharacterController characterController;

    private Vector3 velocity = Vector3.zero;

    const float ELEVATE_SPEED = 1.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        switch (monsterState)
        {
            case MonsterState.IDLE:
                velocity.y = math.sin(Time.time) * 1f;
                if (detector.detected) monsterState = MonsterState.ELEVATE;
                break;
            case MonsterState.ELEVATE:
                velocity.y = ELEVATE_SPEED;
                if (!detector.detected) monsterState = MonsterState.IDLE;
                break;
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(velocity * Time.deltaTime);
        velocity = characterController.velocity;
    }
}
