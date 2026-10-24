using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerHP : MonoBehaviour
    {
		[SerializeField] private Image _healthBar;
		public event EventHandler OnHealthChanged;
		private float _health;

		public float Health
		{
			get { return _health; }
			set {
				if (_health == value)
					return;
				_health = value;
				OnHealthChanged?.Invoke(this, null);

				UpdateHealthBar();

                if (_health <= 0)
					WakeUp();
            }
		}
        private Tween _healthbarTween;
        private void UpdateHealthBar()
        {
            _healthBar.fillAmount = Health / _maxHealth;

            if (_healthbarTween == null)
            {
                _healthbarTween = _healthBar.transform.parent.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1.0f, 1, 0f);
                _healthbarTween.onComplete = DeleteTween;
            }
        }
        private void DeleteTween()
        {
            _healthbarTween = null;
        }
        private void WakeUp()
        {
			Debug.Log("You woke up");
			SceneManager.LoadScene(0);
        }

        [SerializeField] private int _maxHealth;

        private void Start()
        {
			_health = _maxHealth;
        }

    }
}