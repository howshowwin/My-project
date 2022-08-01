using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform deckPanel;
    public Transform libraryPanel;
    public GameObject deckPrefab;

    public GameObject cardPrefab;

    public GameObject DataManager;

    private PlayData PlayData;
    private CardStore CardStore;

    private Dictionary<int, GameObject> libraryDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> deckDic = new Dictionary<int, GameObject>();

    void Start()
    {
        PlayData = DataManager.GetComponent<PlayData>();
        CardStore = DataManager.GetComponent<CardStore>();
        UpdateLibrary();
        UpdateDeck();
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
                CreatCard(i, CardState.Library);
            }
        }
    }
    public void UpdateDeck()
    {
        for (int i = 0; i < PlayData.playerDeck.Length; i++)
        {
            if (PlayData.playerDeck[i] > 0)
            {
                CreatCard(i, CardState.Deck);
            }
        }
    }
    public void UpdateCard(CardState _state, int _id)
    {
        if (_state == CardState.Deck)
        {
            PlayData.playerDeck[_id]--;
            PlayData.playerCards[_id]++;

            if (!deckDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                deckDic.Remove(_id);
            }
            if (libraryDic.ContainsKey(_id))
            {
                libraryDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                CreatCard(_id, CardState.Library);
            }

        }
        else if (_state == CardState.Library)
        {
            PlayData.playerDeck[_id]++;
            PlayData.playerCards[_id]--;

            if (deckDic.ContainsKey(_id))
            {
                deckDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                CreatCard(_id, CardState.Deck);
            }
            if (!libraryDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                libraryDic.Remove(_id);
            }
           
        }
    }
    public void CreatCard(int _id, CardState _cardState)
    {
        Transform targetPanel;
        GameObject targetPrefab;
        var refData = PlayData.playerCards;
        Dictionary<int, GameObject> targetDic = libraryDic;
        if (_cardState == CardState.Library)
        {
            targetPanel = libraryPanel;
            targetPrefab = cardPrefab;
        }
        else
        {
            targetPanel = deckPanel;
            targetPrefab = deckPrefab;
            refData = PlayData.playerDeck;
            targetDic = deckDic;
        }
        GameObject newCard = Instantiate(targetPrefab, targetPanel);
        newCard.GetComponent<CardCounter>().SetCounter(refData[_id]);
        newCard.GetComponent<CardDisplay>().card = CardStore.cardList[_id];
        targetDic.Add(_id, newCard);
    }
}

