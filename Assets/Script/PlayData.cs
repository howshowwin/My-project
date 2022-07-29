using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayData : MonoBehaviour
{
    // Start is called before the first frame update
    public CardStore CardStore;
    public int playerCoin;
    public int[] playerCards;
    public int[] playerDeck;

    public TextAsset playData;
    void Start()
    {
        CardStore.loadCardData();
        LoadPlayData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadPlayData()
    {
        playerCards = new int[CardStore.cardList.Count];
        playerDeck = new int[CardStore.cardList.Count];

        string[] dataRow = playData.text.Split("\n");
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "coins")
            {
                playerCoin = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerCards[id] = num;

            }
            else if (rowArray[0] == "deck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerDeck[id] = num;
            }
        }
    }
    public void SavePlayData()
    {
        string path = Application.dataPath + "/store/playerdata.csv";

        List<string> datas = new List<string>();
        datas.Add("coins," + playerCoin.ToString());
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != 0)
            {
                datas.Add("card," + i.ToString() + "," + playerCards[i].ToString());
            }
        }
        for (int i = 0; i < playerDeck.Length; i++)
        {
            if(playerDeck[i]!=0){
                datas.Add("deck," + i.ToString() + "," + playerDeck[i].ToString());

            }
        }
        File.WriteAllLines(path, datas);
    }
}
