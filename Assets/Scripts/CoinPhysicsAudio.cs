using System;
using UnityEngine;
using UnityEngine.Audio;

public class CoinPhysicsAudio : MonoBehaviour
{
    [SerializeField] private float _minImpactVelocity;
    [SerializeField] private float _impactCooldown;
    [SerializeField] private float _maxImpactVelocity;
    [SerializeField] private AudioSource _audioSource;

    private float _impactCooldownTimer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        float impactSpeed = Mathf.Abs(Vector2.Dot(collision.relativeVelocity, contact.normal));

        PlayImpact(impactSpeed);
    }
    private void Update()
    {
        if (_impactCooldownTimer > 0)
        {
            _impactCooldownTimer -= Time.deltaTime;
        }
    }
    private void PlayImpact(float velocity)
    {
        Debug.Log(velocity);

        if (velocity < _minImpactVelocity || _impactCooldownTimer > 0f)
        {
            return;
        }

        _impactCooldownTimer = _impactCooldown;

        float normalizedImpact = Mathf.Clamp01(velocity / _maxImpactVelocity);

        _audioSource.volume = normalizedImpact;
        _audioSource.Play();
    }
}
