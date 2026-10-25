using UnityEngine;

public class OverLay : MonoBehaviour
{
    private static OverLay Instance { get; set; }

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject); // a player already exists, remove this duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
