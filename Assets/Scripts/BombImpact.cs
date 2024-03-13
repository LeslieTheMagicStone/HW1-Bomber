using UnityEngine;

public class BombImpact : MonoBehaviour
{
    const float MAX_RANGE = 15f;
    const float MAX_FORCE = 10000f;
    const float DURATION = 0.5f;

    private float timer = 0f;
    private float force = MAX_FORCE;

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / DURATION;
        transform.localScale = MAX_RANGE * t * Vector3.one;

        force = (DURATION - timer) / DURATION * MAX_FORCE;

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
            rb.AddForce(force * direction);
        }
    }
}
