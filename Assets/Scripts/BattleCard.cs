using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    inPlayerHand, inPlayerBlock, inEnemyHand, inEnemyBlock
}
public class BattleCard : MonoBehaviour, IPointerDownHandler
{
    //public BattleManager BattleManager;

    public CardState cardState = CardState.inPlayerHand;

    public bool hasAttacked;


    // Start is called before the first frame update
    void Start()
    {
        // BattleManager = GameObject.Find("AIBattleManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardState == CardState.inPlayerHand && BattleManager.Instance.currentPhase == GamePhase.playerAction)
        {
            if (transform.GetComponent<CardDisplay>().card is MonsterCard)
            {
                BattleManager.Instance.SummonRequst(transform.position, 0, transform.gameObject);
            }
        }
        else if (cardState == CardState.inEnemyHand && BattleManager.Instance.currentPhase == GamePhase.enemyAction)
        {
            if (transform.GetComponent<CardDisplay>().card is MonsterCard)
            {
                BattleManager.Instance.SummonRequst(transform.position, 1, transform.gameObject);
            }
        }
        else if (cardState == CardState.inPlayerBlock && BattleManager.Instance.currentPhase == GamePhase.playerAction)
        {
            
            if (BattleManager.Instance.comboingMonster != null)
            {
                
                Debug.Log("同隊");
                BattleManager.Instance.ComboRequst(transform.position, 0, transform.gameObject);
            }
            else
            {
                if (!hasAttacked)
                {
                    Debug.Log("敵對");
                    BattleManager.Instance.AttackRequst(transform.position, 0, transform.gameObject);
                }
            }



        }
        else if (cardState == CardState.inEnemyBlock && BattleManager.Instance.currentPhase == GamePhase.enemyAction)
        {
            if (!hasAttacked)
            {
                BattleManager.Instance.AttackRequst(transform.position, 1, transform.gameObject);
            }
        }
    }
}
