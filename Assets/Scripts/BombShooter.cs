using System;
using Unity.Mathematics;
using UnityEngine;

public class BombShooter : MonoBehaviour
{
    [SerializeField]
    private Bomb bombPrefab, safeBombPrefab;
    private Bomb selectedBombPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private DisplayBase displayBase;

    const float MAX_SHOOT_COOLDOWN = 0.4f;
    private float shootCooldown = 0f;

    private bool bombInHand = false;
    private Bomb bomb;
    private Rigidbody bombRigidbody;
    // Used to get the base velocity of the bomb.
    private CharacterController parent;

    private void Start()
    {
        parent = GetComponentInParent<CharacterController>();

        selectedBombPrefab = bombPrefab;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedBombPrefab == bombPrefab) selectedBombPrefab = safeBombPrefab;
            else selectedBombPrefab = bombPrefab;
            displayBase.Display(selectedBombPrefab);
        }

        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        else
        {
            if (!bombInHand)
            {
                bomb = Instantiate(selectedBombPrefab, spawnPoint.position, spawnPoint.rotation);
                bombRigidbody = bomb.GetComponent<Rigidbody>();
                bombRigidbody.isKinematic = true;
                bomb.transform.SetParent(spawnPoint);
                bombInHand = true;
            }

            if (!Input.GetMouseButtonDown(0)) return;
            if (!bomb.isReady) return;

            shootCooldown = MAX_SHOOT_COOLDOWN;
            bombInHand = false;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            bombRigidbody.isKinematic = false;
            bomb.transform.SetParent(null);

            // t = 2 * v0 * sin(theta) / g
            // d = v0 * cos(theta) * t
            // v^2 * 2sin(theta)cos(theta) / g = d
            // v = sqrt(d * g / 2sin(theta)cos(theta))
            var s = hit.transform.position - transform.position;
            s.y = 0;
            float distance = s.magnitude;
            float theta = (90f - Vector3.Angle(bomb.transform.up, Vector3.up)) * Mathf.Deg2Rad;
            float g = math.abs(Physics.gravity.y);
            float speed = math.sqrt(distance * g / (2 * math.sin(theta) * math.cos(theta)));

            bombRigidbody.velocity = speed * bomb.transform.up - parent.velocity;
            bomb.Fire();
        }


    }

}
