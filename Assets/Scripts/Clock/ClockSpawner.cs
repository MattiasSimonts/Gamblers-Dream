using System;
using UnityEngine;

public class ClockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ClockPrefab;
    private GameObject _player;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    public void Spawn()
    {
        GameObject clock = Instantiate(_ClockPrefab, transform.position, Quaternion.identity);
        ClockScript clockScript = clock.GetComponent<ClockScript>();
        if (clockScript is null) return;
        clockScript._player = _player.transform;
    }
}