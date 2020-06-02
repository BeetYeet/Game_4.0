using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    [SerializeField]
    private List<ZombieSpawner> spawners;

    [SerializeField]
    private List<Wave> waveTypes;

    [SerializeField] private GameObject basicZombiePrefab = null, healingZombiePrefab = null, toughZombiePrefab = null, fastZombiePrefab = null;

    [SerializeField]
    private Dictionary<ZombieType, GameObject> prefabs;

    [SerializeField]
    private AnimationCurve difficultyOverHealth = AnimationCurve.Linear(0f, .5f, 1f, 1f);

    private int currentWave = 0;

    public static Transform enemyHolder;

    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private Animator waveTextAnimator;

    private void Awake()
    {
        SettingsHandler.ReadToCache();
        prefabs = new Dictionary<ZombieType, GameObject>();
        prefabs.Add(ZombieType.Basic, basicZombiePrefab);
        prefabs.Add(ZombieType.Regen, healingZombiePrefab);
        prefabs.Add(ZombieType.Tough, toughZombiePrefab);
        prefabs.Add(ZombieType.Fast, fastZombiePrefab);
        enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder").transform;
    }

    private void Update()
    {
        bool done = enemyHolder.childCount == 0;

        spawners.ForEach(x => { if (!x.IsDone()) done = false; });

        if (done)
        {
            SetupNextWave();
            // TODO: show its a new wave
        }
    }

    public List<SpawnInfo> GetWave(Wave wave)
    {
        List<SpawnInfo> info = new List<SpawnInfo>();
        if (wave.hoards.Count > spawners.Count)
        {
            wave.hoards.RemoveRange(spawners.Count, wave.hoards.Count - spawners.Count);
        }

        foreach (Hoard hoard in wave.hoards)
        {
            info.Add(new SpawnInfo(hoard.type,
                                    Mathf.CeilToInt(UnityEngine.Random.Range(hoard.minCount, hoard.maxCount) * GetCurrentSpawnRate()),
                                    hoard.spawnRate,
                                    hoard.spawnDelay));
        }
        return info;
    }

    private void SetupNextWave()
    {
        currentWave++;
        waveText.text = $"WAVE {currentWave}";
        waveTextAnimator.SetTrigger("Trigger");
        List<Wave> wavePool = new List<Wave>();
        waveTypes.ForEach(x => { if (x.minWave <= currentWave && x.hoards.Count > 0) wavePool.Add(x); });

        Wave wave = wavePool[UnityEngine.Random.Range(0, wavePool.Count - 1)];

        List<SpawnInfo> info = GetWave(wave);

        for (int i = 0; i < info.Count; i++)
        {
            spawners[i].prefab = prefabs[info[i].type];
            spawners[i].spawnCount = info[i].count;
            spawners[i].spawnRate = info[i].spawnRate;
            spawners[i].timeSinceLast = -info[i].spawnDelay;
        }
        Debug.Log($"Wave: {currentWave}");
    }

    private float GetCurrentSpawnRate()
    {
        return difficultyOverHealth.Evaluate(PlayerHealth.current.Health / PlayerHealth.current.MaxHealth) * DoMath();
    }

    private float DoMath()
    {
        return Mathf.Log(currentWave + 1, 4) + 1 + (currentWave - 1) * 0.03f;
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public ZombieType type;
    public int count;
    public float spawnRate;
    public float spawnDelay;

    public SpawnInfo(ZombieType type, int count, float spawnRate, float spawnDelay)
    {
        this.type = type;
        this.count = count;
        this.spawnRate = spawnRate;
        this.spawnDelay = spawnDelay;
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<Hoard> hoards;
    public int minWave;
}

[System.Serializable]
public struct Hoard
{
    public ZombieType type;
    public int minCount, maxCount;
    public float spawnRate;
    public float spawnDelay;
}