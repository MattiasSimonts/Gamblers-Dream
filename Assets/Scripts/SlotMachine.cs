using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlotMachine : MonoBehaviour
{
    
    
    [FormerlySerializedAs("_coinSpewPosition")]
    [Header("coin spewing")]
    [SerializeField] private Transform[] _coinSpewPositions;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _coinPower  = 10;
    [SerializeField] private float _coinSpewAngleSpread = 5.0f;
    [SerializeField] private float _coinInterval = 0.02f;
    [SerializeField] private float _coinSpinSpeed = 1f;
    private float _timer = 0.0f;

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= 2f)
        {
            _timer -= 2f;
            StartCoroutine(SpewCoins(20));
        }
    }

    private IEnumerator  SpewCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpewCoin();
            yield return new WaitForSeconds(_coinInterval);
        }
    }

    private void SpewCoin()
    {
        Transform coinSpewPosition = _coinSpewPositions[Random.Range(0, _coinSpewPositions.Length)];
        Quaternion rotation = Quaternion.Euler(Random.Range(0,180), 0, Random.Range(0,180));
        GameObject coin = Instantiate(_coinPrefab, coinSpewPosition.position, rotation);
        Vector3 angle = coinSpewPosition.rotation * Quaternion.Euler(
            Random.Range(-_coinSpewAngleSpread, _coinSpewAngleSpread),  
            Random.Range(-_coinSpewAngleSpread*2, _coinSpewAngleSpread*2),
            0f) * Vector3.forward;
            
        Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();
        if(coinRigidbody)
        {
            coinRigidbody.AddTorque(Random.insideUnitSphere * _coinSpinSpeed, ForceMode.Impulse);
            coinRigidbody.AddForce(angle * _coinPower, ForceMode.Impulse);
        }
    }
}
