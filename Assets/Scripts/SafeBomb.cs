using UnityEngine;

class SafeBomb : Bomb
{
    public float maxRadius;
    protected override void OnCollisionEnter(Collision other)
    {
        
    }

    public override void Explode()
    {
        BombImpact impact = Instantiate(impactPrefab, null);
        impact.transform.position = transform.position;
        impact.maxRadius = maxRadius;

        // Boom animation.
        boom.transform.SetParent(null);
        boom.Play();
        Destroy(boom.gameObject, 10f);
        
        int randIndex = Random.Range(0, boomAudios.Length);
        AudioManager.instance.Play(boomAudios[randIndex], 0.1f);

        Destroy(gameObject);
    }
}