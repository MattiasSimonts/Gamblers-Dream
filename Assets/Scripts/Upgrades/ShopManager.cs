using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _shopItems;
    [SerializeField] private int _amount = 3;
    [SerializeField] private bool _isEnabable = true;

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_isEnabable) return;
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        List<Upgrade> upgrades = _shopItems.Select(shopItem => shopItem.GetComponent<Upgrade>()).ToList();
        List<Upgrade> availeble = upgrades.Where(upgrade => upgrade.Price < player.Coins).ToList();
        HashSet<Upgrade> final = new HashSet<Upgrade>();
        if (availeble.Count > 0)
            final.Add(availeble[Random.Range(0, availeble.Count)]);

        for (int i = final.Count; i < _amount; i++)
            final.Add(upgrades[Random.Range(0, upgrades.Count)]);

        for (int i = final.Count; final.Count < _amount; i++)
            final.Add(upgrades[Random.Range(0, upgrades.Count)]);
        foreach (Upgrade upgrade in upgrades)
            upgrade.gameObject.SetActive(false);
        foreach (Upgrade upgrade in final)
            upgrade.gameObject.SetActive(true);
    }
}