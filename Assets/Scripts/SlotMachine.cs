using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private bool _spinAutomaticly = true;
    
    [Header("Barrels")]
    [SerializeField] private Transform[] _barrelsTransforms;
    [SerializeField] private float _barrelSpinSpeed = 200.0f;
    [SerializeField] private float _barrelSpinTime = 5.0f;
    [SerializeField] private float _delayBetweenBarrels = 1f;
    private int _spinningBarrels = 0;
    private float _barrelSpinTimePast = 0f;
    private bool _isSpinning = false;


    [Header("Lever")] 
    [SerializeField] private Transform _leverTransform;
    [SerializeField] private float _leverSpeed = 1f;
    //[SerializeField] private float 


    [Header("coin spewing")] [SerializeField]
    private Transform[] _coinSpewTransforms;

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _coinPower = 10;
    [SerializeField] private float _coinSpewAngleSpread = 5.0f;
    [SerializeField] private float _coinInterval = 0.02f;
    [SerializeField] private float _coinSpinSpeed = 1f;

    [Header("Weights")] 
    [SerializeField] private int _baseAmountOfCoins = 1;
    [SerializeField] private float _generalWeight = 2f;
    [SerializeField] private float _0DegreeCoinsValue = 1f;
    [SerializeField] private float _45DegreeCoinsValue = 1f;
    [SerializeField] private float _90DegreeCoinsValue = 1f;
    [SerializeField] private float _135DegreeCoinsValue = 1f;
    [SerializeField] private float _180DegreeCoinsValue = 1f;
    [SerializeField] private float _225DegreeCoinsValue = 1f;
    [SerializeField] private float _270DegreeCoinsValue = 1f;
    [SerializeField] private float _315DegreeCoinsValue = 1f;
    private readonly string[] _results = new string[3];
    private float _amountOfCoins;

    private float _timer = 0.0f;


    private Dictionary<string, (float, float)> _rotationMap;

    private void FixedUpdate()
    {
        if (_isSpinning)
        {
            _barrelSpinTimePast += Time.fixedDeltaTime;
            if (_barrelSpinTimePast >= _barrelSpinTime)
            {
                _spinningBarrels--;

                if (_spinningBarrels >= 0)
                {
                    _barrelsTransforms[_spinningBarrels].rotation = Quaternion.Euler(_rotationMap[_results[_spinningBarrels]].Item1, 0, 0);
                    _barrelSpinTimePast -= _delayBetweenBarrels;
                }
                else
                {
                    _barrelSpinTimePast = 0f;
                    _isSpinning = false;
                    StartCoroutine(SpewCoins(Mathf.RoundToInt(_amountOfCoins)));
                }
            }

            if (_spinningBarrels > 0)
            {
                for (int i = 0; i < _spinningBarrels; i++)
                {
                    _barrelsTransforms[i].Rotate(_barrelSpinSpeed * Time.deltaTime, 0, 0);
                }
            }
        }


        if (_spinAutomaticly)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= 10f)
            {
                _timer -= 10f;
                Spin();
            }
        }
    }

    private void Awake()
    {
        _rotationMap = new Dictionary<string, (float, float )>()
        {
            { "Bell", (0f, _0DegreeCoinsValue) },
            { "Clover", (45f, _45DegreeCoinsValue) },
            { "Apple", (90f, _90DegreeCoinsValue) },
            { "Druif", (135f, _135DegreeCoinsValue) },
            { "7", (180f, _180DegreeCoinsValue) },
            { "sinaasapple", (225f, _225DegreeCoinsValue) },
            { "kers", (270f, _270DegreeCoinsValue) },
            { "Bar", (315f, _315DegreeCoinsValue) }
        };
    }

    private void Start()
    {
        for (int i = 0; i < _barrelsTransforms.Length; i++)
        {
            _results[i] = _rotationMap.Keys.ElementAt(Random.Range(0, _rotationMap.Count));
            _barrelsTransforms[i].rotation = Quaternion.Euler(_rotationMap[_results[i]].Item1, 0, 0);
        }
    }

    private IEnumerator SpewCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform coinSpewPosition = _coinSpewTransforms[Random.Range(0, _coinSpewTransforms.Length)];
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 180), 0, Random.Range(0, 180));
            GameObject coin = Instantiate(_coinPrefab, coinSpewPosition.position, rotation);
            Vector3 angle = coinSpewPosition.rotation * Quaternion.Euler(
                Random.Range(-_coinSpewAngleSpread, _coinSpewAngleSpread),
                Random.Range(-_coinSpewAngleSpread * 2, _coinSpewAngleSpread * 2),
                0f) * Vector3.forward;

            Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();
            if (coinRigidbody)
            {
                coinRigidbody.AddTorque(Random.insideUnitSphere * _coinSpinSpeed, ForceMode.Impulse);
                coinRigidbody.AddForce(angle * _coinPower, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(_coinInterval);
        }
    }

    public void Spin()
    {
        if (_isSpinning)
            return;
        _spinningBarrels = _barrelsTransforms.Length;
        _isSpinning = true;
        GetSpinResults();
    }

    private void GetSpinResults()
    {
        _amountOfCoins =_baseAmountOfCoins;
        if (Random.Range(0, 1) == 1)
        {
            string randomResult = _rotationMap.Keys.ElementAt(Random.Range(0, _rotationMap.Count));
            for (int i = 0; i < _barrelsTransforms.Length; i++)
            {
                _results[i] = randomResult;
                _amountOfCoins *= (_rotationMap[randomResult].Item2 + _generalWeight);
            }
        }
        else
        {
            string randomResult = _rotationMap.Keys.ElementAt(Random.Range(0, _rotationMap.Count));
            for (int i = 0; i < _barrelsTransforms.Length; i++) _results[i] = randomResult;
            _amountOfCoins *= (_rotationMap[randomResult].Item2 + _generalWeight);
            _amountOfCoins *= (_rotationMap[randomResult].Item2 + _generalWeight);


            _results[Random.Range(0, _barrelsTransforms.Length) - 1] = _rotationMap.Keys.ElementAt(Random.Range(0, _rotationMap.Count));
        }
    }
}