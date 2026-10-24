using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCoins : MonoBehaviour
    {
		private int _coins;

		public int Coins
		{
			get { return _coins; }
			set { 
				_coins = value; 

			}
		}
		[SerializeField] private int _startingCoins;
        private void Start()
        {
			Coins = _startingCoins;
        }
    }
}