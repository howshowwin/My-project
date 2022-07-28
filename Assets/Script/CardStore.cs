using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset cardData;
    public List<Card> cardList = new List<Card>();
    void Start()
    {
        // loadCardData();
        // TestLoad();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void loadCardData()
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "element")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int level = int.Parse(rowArray[3]);
                int attack = int.Parse(rowArray[4]);
                ElementCard elementCard = new ElementCard(id, name, attack, level);
                cardList.Add(elementCard);
                // Debug.Log("讀取到卡:" + elementCard.cardName);
            }
            else if (rowArray[0] == "special")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect = rowArray[3];
                SpecialCard specialCard = new SpecialCard(id, name, effect);
                cardList.Add(specialCard);

            }
        }
    }


    public void TestLoad()
    {
        foreach (var item in cardList)
        {
            Debug.Log("卡:" + item.id.ToString()+ item.cardName);
        }
    }

    public Card RandomCard()
    {
        Card card = cardList[Random.Range(0,cardList.Count)];
        return card;
    }
}
