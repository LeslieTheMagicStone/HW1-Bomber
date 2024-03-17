using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int xSize, zSize, ySize;
    [SerializeField]
    GameObject voxelPrefab, wallPrefab;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    MonsterLogic monsterPrefab;

    public bool spawnMonster = true;
    public bool spawnVoxel = true;
    const float SPAWN_MONSTER_INTERVAL_CONVERGENCE_RATE = 0.01f;
    const float MAX_SPAWN_MONSTER_INTERVAL = 3f;
    private float spawnMonsterInterval = MAX_SPAWN_MONSTER_INTERVAL;
    private float spawnMonsterTimer = MAX_SPAWN_MONSTER_INTERVAL;


    const float SPAWN_VOXEL_INTERVAL_CONVERGENCE_RATE = 0.005f;
    const float MAX_SPAWN_VOXEL_INTERVAL = 5f;
    private float spawnVoxelInterval = MAX_SPAWN_VOXEL_INTERVAL;
    private float spawnVoxelTimer = MAX_SPAWN_VOXEL_INTERVAL;

    public bool spawnUpgrade = true;
    private float spawnUpgradeInterval = 4f;
    private float spawnUpgradeTimer = 1f;
    [SerializeField] private Upgrade[] upgradePrefabs;

    private PlayerLogic player;
    private Damageable playerDam;
    [SerializeField] private DamageField crusher;
    const float CRUSHER_FALL_SPEED = 3.0f;
    private bool isGameOver = false;
    [SerializeField] private TimeLogic timeLogic;

    private void Start()
    {
        for (int x = -xSize; x <= xSize; x++)
        {
            for (int z = -zSize; z <= zSize; z++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    GameObject cube = Instantiate(voxelPrefab);
                    cube.transform.SetParent(spawnPoint);
                    cube.transform.localPosition = new(x, y + 1f, z);
                    cube.name = "Cube(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
                }
            }
        }

        var leftWall = Instantiate(wallPrefab, new Vector3(-xSize - 2.5f, 0, 0), Quaternion.identity);
        leftWall.transform.localScale = new(4f, 100f, 3 * zSize);
        var rightWall = Instantiate(wallPrefab, new Vector3(xSize + 2.5f, 0, 0), Quaternion.identity);
        rightWall.transform.localScale = new(4f, 100f, 3 * zSize);
        var forwardWall = Instantiate(wallPrefab, new Vector3(0, 0, -zSize - 2.5f), Quaternion.identity);
        forwardWall.transform.localScale = new(3 * xSize, 100f, 4f);
        var backWall = Instantiate(wallPrefab, new Vector3(0, 0, zSize + 2.5f), Quaternion.identity);
        backWall.transform.localScale = new(3 * xSize, 100f, 4f);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerLogic>();
        playerDam = player.GetComponent<Damageable>();
        playerDam.onDeath.AddListener(GameOver);
        timeLogic.onComplete.AddListener(Win);
    }

    private void Update()
    {
        if (isGameOver) return;

        if (spawnMonster)
        {
            spawnMonsterInterval = MAX_SPAWN_MONSTER_INTERVAL * Mathf.Exp(-SPAWN_MONSTER_INTERVAL_CONVERGENCE_RATE * Time.time);

            if (spawnMonsterTimer <= 0f)
            {
                var monster = Instantiate(monsterPrefab);
                Vector3 randomPos = GetRandomPos();
                monster.transform.position = randomPos;
                spawnMonsterTimer = spawnMonsterInterval;
            }
            spawnMonsterTimer -= Time.deltaTime;
        }

        if (spawnVoxel)
        {
            spawnVoxelInterval = MAX_SPAWN_VOXEL_INTERVAL * Mathf.Exp(-SPAWN_VOXEL_INTERVAL_CONVERGENCE_RATE * Time.time);

            if (spawnVoxelTimer <= 0f)
            {
                for (int x = -xSize; x <= xSize; x++)
                {
                    for (int z = -zSize; z <= zSize; z++)
                    {
                        for (int y = 0; y < 1; y++)
                        {
                            GameObject cube = Instantiate(voxelPrefab);
                            cube.transform.SetParent(spawnPoint);
                            cube.transform.localPosition = new(x, y, z);
                            cube.name = "Cube(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
                        }
                    }
                }
                spawnVoxelTimer = spawnVoxelInterval;
            }
            spawnVoxelTimer -= Time.deltaTime;
        }

        if (spawnUpgrade)
        {
            if (spawnUpgradeTimer <= 0f)
            {
                int randIndex = Random.Range(0, upgradePrefabs.Length);
                var upgrade = Instantiate(upgradePrefabs[randIndex]);
                Vector3 randomPos = GetRandomPos();
                upgrade.transform.position = randomPos;
                spawnUpgradeTimer = spawnUpgradeInterval;
            }
            spawnUpgradeTimer -= Time.deltaTime;
        }

    }

    private Vector3 GetRandomPos()
    {
        return new(Random.Range(-xSize + 1f, xSize - 1f), 10f, Random.Range(-zSize + 1f, zSize - 1f));
    }

    private void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        isGameOver = true;

        crusher.canDestroyVoxel = true;
        while (crusher.transform.position.y > 0f)
        {
            crusher.transform.Translate(0, -CRUSHER_FALL_SPEED * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.5f);

        SceneReloader.instance.Reload();
    }

    private void Win()
    {
        PlayerPrefs.SetString("HasWon", "true");
        spawnMonster = false;
    }
}
