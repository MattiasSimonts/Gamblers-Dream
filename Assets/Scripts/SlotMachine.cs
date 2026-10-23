using UnityEngine;
using UnityEngine.Serialization;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Transform _coinSpewPosition;
    [SerializeField] private GameObject _coinPrefab;
    
    [Header("coin spewing")]
    [SerializeField] private GameObject _coinSpewingPrefab;
    public void Spin()
    {
        
    }

    private void SpewCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Quaternion rotation = Quaternion.Euler(Random.Range(0,180), 0, Random.Range(0,180));
            GameObject coin = Instantiate(_coinPrefab, _coinSpewPosition.position, rotation);
        }
    }
}
