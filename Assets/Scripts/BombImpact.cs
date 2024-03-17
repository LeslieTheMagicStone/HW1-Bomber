using UnityEngine;

public class BombImpact : MonoBehaviour
{
    public float maxRadius = 8f;
    public float maxForce = 4000f;
    public float maxVelocity = 30f;
    public float maxDizzyTime = 1.5f;
    public float duration = 0.5f;

    private float timer = 0f;
    private float impactScaler => (duration - timer) / duration;

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
        float t = timer / duration;
        transform.localScale = maxRadius * 2 * t * Vector3.one;

        SetTransparency(originMaterial.color.a * impactScaler);

        if (timer > duration)
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
            rb.AddForce(maxForce * impactScaler * direction);
        }

        if (other.TryGetComponent(out PlayerLogic playerLogic))
        {
            playerLogic.SetUnmovable(maxDizzyTime * impactScaler);
            var distance = other.transform.position - transform.position;
            var direction = distance.normalized;
            playerLogic.velocity = impactScaler * maxVelocity * direction;
        }

        if (other.TryGetComponent(out MonsterLogic monsterLogic))
        {
            monsterLogic.SetUnmovable(maxDizzyTime * impactScaler);
            var distance = other.transform.position - transform.position;
            var direction = distance.normalized;
            monsterLogic.velocity = impactScaler * maxVelocity * direction;
        }
    }
}
