using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Warrior_SO", menuName = "unfrozen_tz/Warrior_SO", order = 0)]
public class Warrior_SO : ScriptableObject 
{
    [SerializeField]
    string _name;
    public string Name => _name;

    [SerializeField]
    int _id;
    public int Id => _id;

    [SerializeField]
    bool _isRed;
    public bool IsRed => _isRed;

    [SerializeField]
    int _init;
    public int Init => _init;

    [SerializeField]
    int _speed;
    public int Speed => _speed;
}
