using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleCardState
{
    inHand, inBlock
}
public class BattleCard : MonoBehaviour, IPointerDownHandler
{
    public int playerID;
    public BattleCardState state = BattleCardState.inHand;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (GetComponent<CardDisplay>().card is ElementCard)
        {
            if (state == BattleCardState.inHand)
            {
                BattleManager.Instance.SummonRequest(playerID, gameObject);
            }

        }

        //in hand

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
