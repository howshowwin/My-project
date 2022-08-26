using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AttackTarget : MonoBehaviour, IPointerClickHandler
{
    public bool attackable;
    // CardDisplay display;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData)
    {
        if (attackable)
        {
            // if (display == null)
            // {
            //     BattleManager.Instance.AttackConfirm();
            // }
            // else
            // {
            BattleManager.Instance.AttackConfirm(gameObject);

            // }
        }
    }
    public void ApplyDamage(int _damage)
    {
        ElementCard element = GetComponent<CardDisplay>().card as ElementCard;

        element.levelPoint -= _damage;
        if (element.levelPoint <= 0)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // display = GetComponent<CardDisplay>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
