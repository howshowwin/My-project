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

    public int AttackCount;
    private int attackCount;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (GetComponent<CardDisplay>().card is ElementCard)
        {
            if (state == BattleCardState.inHand)
            {
                BattleManager.Instance.SummonRequest(playerID, gameObject);
            }
            if (state == BattleCardState.inBlock && attackCount > 0)
            {
                BattleManager.Instance.AttackRequest(playerID, gameObject);
            }
        }

        //in hand

    }
    public void ResetAttack(){
        attackCount = AttackCount;
    }
    // Start is called before the first frame update
    public void CostAttackCount(){
        attackCount--;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
