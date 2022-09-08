using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public List<Card> CardList = new List<Card>(); 
    public TextAsset cardListData; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LordCardList()
    {
        string[] dataArray = cardListData.text.Split('\n');
        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "m")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int atk = int.Parse(rowArray[3]);
                int hp = int.Parse(rowArray[4]);
                string type = rowArray[5];
                CardList.Add(new MonsterCard(id, name, atk, hp, type));
            }
            else if (rowArray[0] == "s")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int rank = int.Parse(rowArray[3]);
                string type = rowArray[4];
                string effect = rowArray[5];
                CardList.Add(new SpellCard(id, name, rank, type, effect));
            }
            else if (rowArray[0] == "t")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string type = rowArray[3];
                string effect = rowArray[4];
                CardList.Add(new ItemCard(id, name, type, effect));
            }
        }
    }

    void TestCopy()
    {
        List<Card> copylist = new List<Card>();
        copylist = CardList;
        Card card1 = copylist[1];
        Card card2 = new Card(card1.id, card1.cardName);
        Card card3 = CopyCard(1);

        card1.cardName = "DarkKnight";
        print("test copy");
        print(card3.cardName + "," + card1.GetType());
        print(card2.cardName + "," + card2.GetType());
        print(CardList[1].cardName + "," + CardList[1].GetType());
       
    }
    public Card RandomCard()
    {
        Card randCard = CardList[Random.Range(0, CardList.Count)];
        return randCard;
    }

    public Card CopyCard(int _id) 
    {
        Card card = CardList[_id];
        Card copyCard = new Card(_id, card.cardName);
        if (card is MonsterCard)
        {
            var monstercard = card as MonsterCard;
            copyCard = new MonsterCard(_id, monstercard.cardName, monstercard.attack, monstercard.healthPointMax, monstercard.type);
        }
        else if (card is SpellCard)
        {
            var spellcard = card as SpellCard;
            copyCard = new SpellCard(_id, spellcard.cardName, spellcard.rank, spellcard.type, spellcard.effect);
        }
        else if (card is ItemCard)
        {
            var itemcard = card as ItemCard;
            copyCard = new ItemCard(_id, itemcard.cardName, itemcard.type, itemcard.effect);
        }
        return copyCard;
    }
}
