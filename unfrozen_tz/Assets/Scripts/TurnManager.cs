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
    List<Warrior_SO> _evenOrderList = new List<Warrior_SO>();
    List<Warrior_SO> _oddOrderList= new List<Warrior_SO>();
    List<int> roundIndexes = new List<int>();
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
        roundIndexes.Clear();

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
        for (int i = 0; i < warriorsList.Count - 1; i++)
        {
            for (int j = warriorsList.Count - 1; j > i; j--)
            {
                Warrior_SO currentWarrior = warriorsList[j];
                Warrior_SO nextWarrior = warriorsList[j - 1];

                CompareWarriors(currentWarrior, nextWarrior, even, j);
            }
        }

        if(even)
        {
            _evenOrderList.Clear();
            _evenOrderList.AddRange(warriorsList);
        }
        else
        {
            _oddOrderList.Clear();
            _oddOrderList.AddRange(warriorsList);
        }
    }

    private void CompareWarriors(Warrior_SO currentWarrior, Warrior_SO nextWarrior, bool even, int index)
    {
        if(currentWarrior.Init > nextWarrior.Init)
        {
            ToSwap(index, index - 1);
        }
        else if(currentWarrior.Init == nextWarrior.Init)
        {
            if(currentWarrior.Speed > nextWarrior.Speed)
            {
                ToSwap(index, index - 1);
            }
            else if(currentWarrior.Speed == nextWarrior.Speed)
            {
                if((currentWarrior.IsRed && nextWarrior.IsRed) || (!currentWarrior.IsRed && !nextWarrior.IsRed))
                {
                    if(currentWarrior.Id < nextWarrior.Id)
                    {
                        ToSwap(index, index - 1);
                    }
                }
                else
                {
                    if(even)
                    {
                        if(!currentWarrior.IsRed)
                        {
                            ToSwap(index, index - 1);
                        }
                    }
                    else
                    {
                        if(currentWarrior.IsRed)
                        {
                            ToSwap(index, index - 1);
                        }
                    }
                }
            }
        }
    }

    private void ToSwap(int first, int second)
    {
        Warrior_SO temp = warriorsList[first];
        warriorsList[first] = warriorsList[second];
        warriorsList[second] = temp;
    }
}