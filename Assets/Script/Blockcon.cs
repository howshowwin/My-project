using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Blockcon : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public GameObject card;
    public GameObject SummonBlock;
    public GameObject AttackBlock;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (SummonBlock.activeInHierarchy)
        {
            BattleManager.Instance.SummonConfirm(transform);
        }
    }
}
