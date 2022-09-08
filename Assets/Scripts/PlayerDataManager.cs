using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class PlayerDataManager : MonoBehaviour
{
    public TextAsset playerData;

    public Text coinsText;
    public Text cardsText;

    public int totalCoins;

    private CardData cardData;
    public int[] playerCards;
    public int[] playerDeck;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        cardData = GetComponent<CardData>();
        cardData.LordCardList();
        LordPlayerData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LordPlayerData()
    {
        playerCards = new int[cardData.CardList.Count];
        playerDeck = new int[cardData.CardList.Count];
        string[] dataArray = playerData.text.Split('\n');
        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "coins")
            {
                totalCoins = int.Parse(rowArray[1]);
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
        updateText();
    }
    public void updateText()
    {
        coinsText.text = "Total Coins:" + totalCoins.ToString();
        cardsText.text = "Cards Number:" + Sum(playerCards).ToString();
    }

    public int Sum(int[] _cards)
    {
        int sum = 0;
        foreach (int item in _cards)
        {
            sum += item;
        }
        return sum;
    }

    public void SavePlayerData()
    {
        List<string> datas = new List<string>();
        string path = Application.dataPath + "/Datas/playerdata.csv";
        datas.Add("coins," + totalCoins.ToString());
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != 0)
            {
                datas.Add("card," + i.ToString() + "," + playerCards[i].ToString());
            }
        }
        for (int i = 0; i < playerDeck.Length; i++)
        {
            if (playerDeck[i] != 0)
            {
                datas.Add("deck," + i.ToString() + "," + playerDeck[i].ToString());
            }
        }

        File.WriteAllLines(path, datas);
    }

}
