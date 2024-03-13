using UnityEngine;

public class BombImpact : MonoBehaviour
{
    const float MAX_RANGE = 15f;
    const float MAX_FORCE = 10000f;
    const float MAX_VELOCITY = 30f;
    const float MAX_DIZZY_TIME = 1.0f;
    const float DURATION = 0.5f;

    private float timer = 0f;
    private float impactScaler => (DURATION - timer) / DURATION;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0, 0, 1f, 0.4f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / DURATION;
        transform.localScale = MAX_RANGE * t * Vector3.one;

        if (timer > DURATION)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            var distance = other.transform.position - transform.position;
            var direction = distance.normalized;
            rb.AddForce(MAX_FORCE * impactScaler * direction);
        }

        if (other.TryGetComponent(out PlayerLogic playerLogic))
        {
            playerLogic.SetUnmovable(MAX_DIZZY_TIME * impactScaler);
            var distance = other.transform.position - transform.position;
            var direction = distance.normalized;
            playerLogic.velocity = impactScaler * MAX_VELOCITY * direction;
        }
    }
}
