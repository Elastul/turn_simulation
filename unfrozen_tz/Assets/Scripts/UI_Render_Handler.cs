using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Render_Handler : MonoBehaviour
{
    [SerializeField]
    GameObject warriorPanel;
    [SerializeField]
    GameObject roundPanel;
    [SerializeField]
    Transform main_panel;
    List<GameObject> _tempObectsList = new List<GameObject>();

    bool isFirstIteration;

    public void RenderMoveOrderList(List<Warrior_SO> moveOrderList, int turn, List<int> roundIndexes, int round)
    {
        isFirstIteration = true;
        int i = 0;

        foreach (GameObject panel in _tempObectsList)
        {
            Destroy(panel);
        }

        while(i < 20)
        {
            foreach (Warrior_SO warrior in moveOrderList)
            {
                if(i >= 20)
                {
                    break;
                }
                GameObject _tempWarrior = Instantiate(warriorPanel, main_panel);
                _tempObectsList.Add(_tempWarrior);
                _tempWarrior.GetComponentsInChildren<TextMeshProUGUI>()[0].text = (turn++).ToString();
                _tempWarrior.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Существо " + warrior.Name + ": Инициатива - " + warrior.Init + " Скорость - " + warrior.Speed;
                _tempWarrior.GetComponent<RectTransform>().Find("Description_Panel").GetComponent<Image>().color = warrior.IsRed ? Color.red : Color.blue;
                if(i == roundIndexes[0] && roundIndexes.Count != 0)
                {
                    roundIndexes.RemoveAt(0);
                    GameObject _tempRound = Instantiate(roundPanel, main_panel);
                    _tempRound.GetComponentInChildren<TextMeshProUGUI>().text = "Раунд №" + ++round;
                    _tempObectsList.Add(_tempRound);
                }
                i++;
            }
        }
    }
}
