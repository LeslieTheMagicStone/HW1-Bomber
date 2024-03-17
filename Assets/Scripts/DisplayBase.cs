using UnityEngine;

public class DisplayBase : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private Bomb displayingBomb;
    public float rotateSpeed = 25f;

    private void Start()
    {
        var playerDam = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        playerDam.onDeath.AddListener(() => displayingBomb.Explode());
    }

    public void Display(Bomb bombPrefab)
    {
        if (displayingBomb != null)
        {
            Destroy(displayingBomb.gameObject);
        }

        displayingBomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);
        displayingBomb.transform.SetParent(transform);
        displayingBomb.transform.localScale = spawnPoint.localScale;
        displayingBomb.GetComponent<Rigidbody>().isKinematic = true;
        displayingBomb.name = "DisplayBomb";
    }

    private void Update()
    {
        if (displayingBomb == null) return;

        displayingBomb.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
