using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnOrderUpdate : UnityEvent<List<Warrior_SO>, int, List<int>, int>
{
}
public class TurnManager : MonoBehaviour
{
    [SerializeField]
    OnOrderUpdate onOrderUpdate;
    [SerializeField]
    List<Army_SO> armyList;
    [SerializeField]
    GameObject killButton;
    List<Warrior_SO> warriorsList = new List<Warrior_SO>();
    List<Warrior_SO> warriorsRoundTurnOrder = new List<Warrior_SO>();
    List<Warrior_SO> _evenOrderList;
    List<Warrior_SO> _oddOrderList;
    List<int> roundIndexes;
    int currentRound = 1;
    int turnCounter = 0;
    int globalTurnCounter = 1;


    private void Start() 
    {
        foreach (Army_SO army in armyList)
        {
            foreach (Warrior_SO warrior in army.ArmyList)
            {
                warriorsList.Add(warrior);
            }
        }
        
        FillMoveOrderList(true, currentRound % 2 == 0, turnCounter);
        onOrderUpdate.Invoke(warriorsRoundTurnOrder, globalTurnCounter, roundIndexes, currentRound);
    }

    public void KillNextWarrior()
    {
        globalTurnCounter++;
        warriorsRoundTurnOrder.RemoveAt(0);
        warriorsList.Remove(warriorsRoundTurnOrder[0]);
        if(warriorsList.Count <= 1)
        {
            killButton.SetActive(false);
        }
        if(turnCounter >= warriorsList.Count-1)
        {
            turnCounter = 0;
            currentRound++;
        }
        else
        {
            turnCounter++;
        }
        FillMoveOrderList(true, currentRound % 2 == 0, turnCounter);
        onOrderUpdate.Invoke(warriorsRoundTurnOrder, globalTurnCounter, roundIndexes, currentRound);
    }

    public void SkipTurn()
    {
        globalTurnCounter++;
        if(turnCounter >= warriorsList.Count-1)
        {
            turnCounter = 0;
            currentRound++;
        }
        else
        {
            turnCounter++;
        }
        FillMoveOrderList(false, currentRound % 2 == 0, turnCounter);
        onOrderUpdate.Invoke(warriorsRoundTurnOrder, globalTurnCounter, roundIndexes, currentRound);
    }


    private void FillMoveOrderList(bool needCalculation, bool isRoundEven, int turn)
    {
        if(needCalculation)
        {
            CalculateMoveOrder(true);
            CalculateMoveOrder(false);
        }
        warriorsRoundTurnOrder.Clear();
        roundIndexes = new List<int>();
        while(warriorsRoundTurnOrder.Count < 20)
        {
            for ( ; turn < warriorsList.Count; turn++)
            {
                if(isRoundEven)
                {
                    warriorsRoundTurnOrder.Add(_evenOrderList[turn]);
                }
                else
                {
                    warriorsRoundTurnOrder.Add(_oddOrderList[turn]);
                }
            }
            roundIndexes.Add(warriorsRoundTurnOrder.Count-1);
            isRoundEven = isRoundEven ? false : true;
            turn = 0;
        }
    }


    private void CalculateMoveOrder(bool even)
    {
        for (int i = warriorsList.Count - 1; i >= 1; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if(warriorsList[j].Init > warriorsList[j + 1].Init)
                {
                    ToSwap(j, j + 1);
                }
                else if(warriorsList[j].Init == warriorsList[j + 1].Init)
                {
                    if(warriorsList[j].Speed > warriorsList[j + 1].Speed)
                    {
                        ToSwap(j, j + 1);
                    }
                    else if(warriorsList[j].Speed == warriorsList[j + 1].Speed)
                    {
                        if((warriorsList[j].IsRed && warriorsList[j + 1].IsRed) || (!warriorsList[j].IsRed && !warriorsList[j + 1].IsRed))
                        {
                            if(warriorsList[j].Id < warriorsList[j + 1].Id)
                            {
                                ToSwap(j, j + 1);
                            }
                        }
                        else
                        {
                            if(even)
                            {
                                if(!warriorsList[j].IsRed)
                                {
                                    ToSwap(j, j + 1);
                                }
                            }
                            else
                            {
                                if(warriorsList[j].IsRed)
                                {
                                    ToSwap(j, j + 1);
                                }
                            }
                        }
                    }
                }
            }
        }
        warriorsList.Reverse();
        if(even)
        {
            _evenOrderList = new List<Warrior_SO>(warriorsList);
        }
        else
        {
            _oddOrderList = new List<Warrior_SO>(warriorsList);
        }
    }
    private void ToSwap(int first, int second)
    {
        Warrior_SO temp = warriorsList[first];
        warriorsList[first] = warriorsList[second];
        warriorsList[second] = temp;
    }
}