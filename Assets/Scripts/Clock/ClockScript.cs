using System;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

public class ClockScript : MonoBehaviour
{
    public Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _damage = 50f;
    
    [SerializeField] private LayerMask _playerLayer;

    private void Awake()
    {
        _agent.speed = _speed;
    }

    private void FixedUpdate()
    {
        if(_player is null) return;
        _agent.destination = _player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask mask = 1 << other.gameObject.layer;
        if (mask != _playerLayer) return;
        PlayerHP health = other.GetComponent<PlayerHP>();
        if (health is null)  return;
        health.Health -= _damage;
        Destroy(gameObject);
    }
}
