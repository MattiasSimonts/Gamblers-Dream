
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClockManager : MonoBehaviour
{
    [SerializeField] private string _clockSpawnerTag = "ClockSpawner";
    [SerializeField] private float _minDistanceFromPlayerToSpawner = 100f;
    [SerializeField] private float _standardSpawnInterval = 15f;
    [SerializeField] private float _spawnIntervalDecreasePerCoin = 0.3f;
    [SerializeField] private float _minSpawnTime = 2f;
    [SerializeField] private PlayerMovement _playermovement;
    public float SpawnInterval
    {
        get 
        { 
            return Mathf.Clamp(_standardSpawnInterval - _spawnIntervalDecreasePerCoin * _playermovement.Coins, _minSpawnTime, 100f); 
        }
    }


    private List<ClockSpawner> _clockSpawners = new List<ClockSpawner>();
    private Transform _player;
    private float _timer = 0f;

    public bool IsSpawning = false;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player is not null)
        {
            _player = player.transform;
            _playermovement = player.GetComponent<PlayerMovement>();
        }

        var clockSpawners = GameObject.FindGameObjectsWithTag(_clockSpawnerTag);
        foreach (GameObject spawnerObject in clockSpawners)
        {
            if (spawnerObject.TryGetComponent(out ClockSpawner spawner))
                _clockSpawners.Add(spawner);
        }
    }

    void SpawnClock()
    {
        if (_clockSpawners.Count == 0 || _player is null)
            return;
        List<ClockSpawner> possibleSpawnLocations = _clockSpawners.Where(
            clockSpawner => Vector3.Distance(clockSpawner.Position, _player.position) > _minDistanceFromPlayerToSpawner).ToList() ?? new List<ClockSpawner>();
        if (possibleSpawnLocations.Count == 0) return;
        possibleSpawnLocations[Random.Range(0, possibleSpawnLocations.Count)].Spawn();
    }

    private void FixedUpdate()
    {
        if (IsSpawning)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= SpawnInterval)
            {
                _timer -= SpawnInterval;
                SpawnClock();
            }
        }
    }
}
