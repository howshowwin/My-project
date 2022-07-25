using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // 白癡
    // Start is called before the first frame update
    public Text nameText;
    public Text attackText;
    public Text levelPointText;
    
    public Text effectText;
    public Image backgroundImage;


    public Card card;
    void Start()
    {
        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowCard()
    {
        nameText.text = card.cardName;
        if(card is ElementCard)
        {
            var element = card as ElementCard;
            attackText.text = element.attack.ToString();
            levelPointText.text = element.levelPoint.ToString();
            effectText.gameObject.SetActive(false);
        }else if(card is SpecialCard)
        {
            var special = card as SpecialCard;
            effectText.text = special.effect;
            attackText.gameObject.SetActive(false);
            levelPointText.gameObject.SetActive(false);

        }
    }
}
