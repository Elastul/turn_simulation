using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Army_SO", menuName = "unfrozen_tz/Army_SO", order = 0)]
public class Army_SO : ScriptableObject 
{
    [SerializeField]
    string _armyColor;
    public string ArmyColor => _armyColor;

    [SerializeField]
    List<Warrior_SO> _armyList;
    public List<Warrior_SO> ArmyList => _armyList;
}
