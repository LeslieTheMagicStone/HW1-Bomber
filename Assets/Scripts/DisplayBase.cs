using Unity.VisualScripting;
using UnityEngine;

public class DisplayBase : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    Bomb displayingBomb;
    public float rotateSpeed = 25f;

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
