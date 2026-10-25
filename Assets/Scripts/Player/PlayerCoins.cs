using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerCoins : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsDisplayText;
        private int _coins;

        public int Coins
        {
            get { return _coins; }
            set
            {
                if (value == _coins)
                    return;
                _coins = value;

                UpdateCoinVisual();
            }
        }

        private void UpdateCoinVisual()
        {
            _coinsDisplayText.text = Coins.ToString();
        }

        [SerializeField] private int _startingCoins;
        private void Start()
        {
            Coins = _startingCoins;
        }
    }
}