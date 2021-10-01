using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WarriorUIPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _idText;
    [SerializeField]
    TextMeshProUGUI _descriptionText;
    [SerializeField]
    Image _img;
    public void SetWarrior(int id, string description, Color color)
    {
        _idText.text = id.ToString();
        _descriptionText.text = description;
        _img.color = color;
    }
}
