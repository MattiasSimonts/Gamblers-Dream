using System;
using System.Collections;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class ClockScript : MonoBehaviour
{
    public Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _damage = 50f;
    
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private float _jumpDistance;
    [SerializeField] private float _jumpHorizontalPower;
    [SerializeField] private float _jumpVerticalPower;
    [SerializeField] private float _jumpWindUpTime;
    [SerializeField] private Rigidbody _clockRB;
    [SerializeField] private GameObject _deathParticleSystem;
    [SerializeField] private Animator _clockAnimator;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioResource _clockJumpsounds;

    private Coroutine _jumpCoroutine;

    private bool _hasJumped = false;
    public float Speed
    {
        get { return _agent.speed; }
        set { 
            _agent.speed = value;
            _clockAnimator.SetFloat("Speed", value);
        }
    }

    private void Awake()
    {
        Speed = _speed;
    }

    private void FixedUpdate()
    {
        if(_player is null) return;

        if(_agent.enabled)
        {
            _agent.destination = _player.position;

            if (_agent.pathPending) return;

            if (_agent.remainingDistance <= _jumpDistance && _jumpCoroutine == null)
            {
                _jumpCoroutine = StartCoroutine(StartJump());
            }
        }
    }

    private IEnumerator StartJump()
    {
        Speed = 0;

        _agent.enabled = false;

        _audioSource.resource = _clockJumpsounds;

        _audioSource.Play();

        Vector3 playerDirection = _player.position - transform.position;

        yield return new WaitForSeconds(_jumpWindUpTime);

        playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z).normalized;

        Vector3 jumpForce = (playerDirection * _jumpHorizontalPower) + (Vector3.up * _jumpVerticalPower);

        _clockRB.AddForce(jumpForce, ForceMode.Impulse);

        _hasJumped = true;

        yield return null;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasJumped)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (_deathParticleSystem != null)
        {
            Instantiate(_deathParticleSystem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
