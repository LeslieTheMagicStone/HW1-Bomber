    using System;
using Unity.Mathematics;
using UnityEngine;

public class BombShooter : MonoBehaviour
{
    [SerializeField] private Bomb bombPrefab, safeBombPrefab;
    private Bomb selectedBombPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DisplayBase displayBase;
    [SerializeField] private Transform trackPrefab;
    private Transform[] tracks;

    [HideInInspector] public float maxShootCooldown = 0.4f;
    private float shootCooldown = 0f;

    private bool bombInHand = false;
    private Bomb bomb;
    private float shootSpeed;
    float theta => (90f - Vector3.Angle(spawnPoint.transform.up, Vector3.up)) * Mathf.Deg2Rad;
    private Rigidbody bombRigidbody;

    private void Start()
    {
        selectedBombPrefab = bombPrefab;
        displayBase.Display(selectedBombPrefab);

        tracks = new Transform[40];
        for (int i = 0; i < 40; i++)
        {
            tracks[i] = Instantiate(trackPrefab);
            tracks[i].name = "Track " + i.ToString();
        }
    }

    private void Update()
    {
        UpdateShootSpeed();
        DrawTracks();

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

            if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) return;
            if (!bomb.isReady) return;

            shootCooldown = maxShootCooldown;
            bombInHand = false;

            bombRigidbody.isKinematic = false;
            bomb.transform.SetParent(null);

            bombRigidbody.velocity = shootSpeed * bomb.transform.up;
            bomb.Fire();
        }
    }

    private void OnDestroy()
    {
        foreach (var track in tracks)
            if (track != null)
                Destroy(track.gameObject);
    }

    private void UpdateShootSpeed()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        // t = 2 * v0 * sin(theta) / g
        // d = v0 * cos(theta) * t
        // v0^2 * 2sin(theta)cos(theta) / g = d
        // v0 = sqrt(d * g / 2sin(theta)cos(theta))
        var s = hit.point - spawnPoint.position;
        s.y = 0;
        float distance = s.magnitude;
        float g = math.abs(Physics.gravity.y);
        shootSpeed = math.sqrt(distance * g / (2 * math.sin(theta) * math.cos(theta)));
    }

    private void DrawTracks()
    {
        for (int i = 0; i < 40; i++)
        {
            float t = i * 0.05f;
            float z = shootSpeed * t * Mathf.Cos(theta);
            float y = shootSpeed * t * Mathf.Sin(theta) + 0.5f * Physics.gravity.y * t * t;
            Vector3 pos = transform.parent.parent.rotation * new Vector3(0, y, z) + spawnPoint.position;
            tracks[i].position = pos;
        }
    }
}