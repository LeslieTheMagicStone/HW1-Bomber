using UnityEngine;

public class BombImpact : MonoBehaviour
{
    const float MAX_RADIUS = 8f;
    const float MAX_FORCE = 10000f;
    const float MAX_VELOCITY = 50f;
    const float MAX_DIZZY_TIME = 1.0f;
    const float DURATION = 0.5f;

    private float timer = 0f;
    private float impactScaler => (DURATION - timer) / DURATION;

    private new Renderer renderer;
    private Material originMaterial;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0, 0, 1f, 0.4f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x / 2f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / DURATION;
        transform.localScale = MAX_RADIUS * 2 * t * Vector3.one;

        SetTransparency(originMaterial.color.a * impactScaler);

        if (timer > DURATION)
        {
            Destroy(gameObject);
        }
    }

    public void SetTransparency(float a)
    {
        Material newMaterial = new(originMaterial);

        Color color = newMaterial.color;
        color.a = a;
        newMaterial.color = color;

        renderer.material = newMaterial;
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
