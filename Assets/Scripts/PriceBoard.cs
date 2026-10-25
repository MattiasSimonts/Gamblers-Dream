using TMPro;
using UnityEngine;

public class PriceBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;

    public void SetText(string title, string description)
    {
        _title.text = title;
        _description.text = description;
    }
}
