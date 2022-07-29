using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManger : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform deckPanel;
    public Transform libraryPanel;
    public GameObject deckPrefab;

    public GameObject cardPrefab;

    public GameObject DataManager;

    private PlayData PlayData;
    private CardStore CardStore;


    void Start()
    {
        PlayData = DataManager.GetComponent<PlayData>();
        CardStore = DataManager.GetComponent<CardStore>();
        UpdateLibrary();
        UpdataDeck();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLibrary()
    {
        for (int i = 0; i < PlayData.playerCards.Length; i++)
        {
            if (PlayData.playerCards[i] > 0)
            {
                GameObject newCard = Instantiate(cardPrefab, libraryPanel);
                newCard.GetComponent<CardCounter>().counter.text = PlayData.playerCards[i].ToString();
                newCard.GetComponent<CardDisplay>().card = CardStore.cardList[i];
            }
        }
    }
    public void UpdataDeck()
    {
        for (int i = 0; i < PlayData.playerDeck.Length; i++)
        {
            if (PlayData.playerDeck[i] > 0)
            {
                GameObject newCard = Instantiate(deckPrefab, deckPanel);
                newCard.GetComponent<CardCounter>().counter.text = PlayData.playerDeck[i].ToString();
                newCard.GetComponent<CardDisplay>().card = CardStore.cardList[i];
            }
        }
    }
}
