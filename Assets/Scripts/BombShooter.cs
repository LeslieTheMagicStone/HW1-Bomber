using UnityEngine;

public class BombShooter : MonoBehaviour
{
    [SerializeField]
    private Bomb bombPrefab;
    [SerializeField]
    private Transform spawnPoint;

    const float SHOOT_SPEED = 10f;
    const float MAX_SHOOT_COOLDOWN = 0.5f;
    private float shootCooldown = 0f;

    private bool readyToShoot = false;
    private Bomb bomb;
    private Rigidbody bombRigidbody;


    private void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        else
        {
            if (!readyToShoot)
            {
                bomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);
                bombRigidbody = bomb.GetComponent<Rigidbody>();
                bombRigidbody.isKinematic = true;
                bomb.transform.SetParent(spawnPoint);
                readyToShoot = true;
            }

            if (!Input.GetMouseButtonDown(0)) return;

            shootCooldown = MAX_SHOOT_COOLDOWN;
            readyToShoot = false;

            bombRigidbody.isKinematic = false;
            bomb.transform.SetParent(null);
            bombRigidbody.velocity = bomb.transform.up * SHOOT_SPEED;
            bomb.Fire();
        }


    }

}
