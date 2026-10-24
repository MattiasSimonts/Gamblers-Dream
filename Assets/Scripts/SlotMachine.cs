using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlotMachine : MonoBehaviour
{
    [Header("Barrels")]
    [SerializeField] private Transform[] _barrelsTransforms;
    [SerializeField] private float _barrelSpinSpeed = 200.0f;
    [SerializeField] private float _barrelSpinTime = 5.0f;
    [SerializeField] private float _delayBetweenBarrels = 1f;
    private int _spinningBarrels = 0;
    private float _barrelSpinTimePast = 0f;
    
    [Header("Lever")]
    [SerializeField] private Transform _leverTransform;
    [SerializeField] private float _leverSpeed = 1f;
    //[SerializeField] private float 
    
    
    [Header("coin spewing")]
    [SerializeField] private Transform[] _coinSpewTransforms;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _coinPower  = 10;
    [SerializeField] private float _coinSpewAngleSpread = 5.0f;
    [SerializeField] private float _coinInterval = 0.02f;
    [SerializeField] private float _coinSpinSpeed = 1f;
    
    private float _timer = 0.0f;
    
    private bool _isSpinning = false;
    private void FixedUpdate()
    {
        if (_isSpinning)
        {
            _barrelSpinTimePast += Time.fixedDeltaTime;
            if (_barrelSpinTimePast >= _barrelSpinTime)
            {
                _spinningBarrels--;
                if(_spinningBarrels > 0)
                    _barrelsTransforms[_spinningBarrels].rotation = Quaternion.Euler(0,0,0);
                
                if(_spinningBarrels >= 0)
                    _barrelSpinTimePast -= _delayBetweenBarrels;
                
                else
                {
                    _isSpinning = false;
                    StartCoroutine(SpewCoins(20));
                    
                }
            }
            for (int i = 0; i < _spinningBarrels; i++)
            {
                _barrelsTransforms[i].Rotate(_barrelSpinSpeed*Time.deltaTime,0,0);
            }
        }
        
        
        
        
        _timer += Time.fixedDeltaTime;
        if (_timer >= 10f)
        {
            _timer -= 10f;
            Spin();
        }
    }

    private void Start()
    {
        
    }

    private IEnumerator  SpewCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform coinSpewPosition = _coinSpewTransforms[Random.Range(0, _coinSpewTransforms.Length)];
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
            yield return new WaitForSeconds(_coinInterval);
        }
    }
    public void Spin()
    {
        if(_isSpinning)
            return;
        _spinningBarrels = _barrelsTransforms.Length;
        _isSpinning = true;
    }
}
