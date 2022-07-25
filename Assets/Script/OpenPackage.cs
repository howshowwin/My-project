using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPackage : MonoBehaviour
{
    public GameObject cardPrefab;

    public GameObject cardPool;

    CardStore CardStore;
    List<GameObject> cards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CardStore = GetComponent<CardStore>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnclickOpen()
    {
        ClearPool();
        for (int i = 0; i < 5; i++)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, cardPool.transform);
            newCard.GetComponent<CardDisplay>().card = CardStore.RandomCard();

            cards.Add(newCard);
        }
    }
    public void ClearPool()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();

    }
}
