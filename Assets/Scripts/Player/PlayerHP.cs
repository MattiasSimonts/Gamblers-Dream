using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerHP : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private float _maxHealth;

        [SerializeField] private AudioSource _audioDamageSource;
        [SerializeField] private AudioResource _audioDamageResource;
        public event EventHandler OnHealthChanged;
        private float _health;
        

        public float Health
        {
            get => _health;
            set
            {
                if (SceneManager.GetActiveScene().buildIndex != 0) return;
                if (_health == value)
                    return;
                _health = value;
                OnHealthChanged?.Invoke(this, null);

                UpdateHealthBar();

                if (_health <= 0)
                    WakeUp();
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                Health = value - _maxHealth;
                _maxHealth = value;
            }
        }

        private Tween _healthbarTween;

        private void UpdateHealthBar()
        {
            _healthBar.fillAmount = (_maxHealth-Health) / _maxHealth;

            if (_healthbarTween == null)
            {
                _healthbarTween = _healthBar.transform.parent.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1.0f, 1, 0f);
                _healthbarTween.onComplete = DeleteTween;
            }
        }
        public void TakeClockDamage()
        {
            _audioDamageSource.resource = _audioDamageResource;
            _audioDamageSource.Play();
        }

        private void DeleteTween()
        {
            _healthbarTween = null;
        }

        private void WakeUp()
        {
            Debug.Log("You woke up");
            SceneManager.LoadScene(1);
        }


        private void Start()
        {
            _health = _maxHealth;
        }

        private static PlayerHP Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // a player already exists, remove this duplicate
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}