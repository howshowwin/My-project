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
    public Sprite backgroundImage1;
    public Sprite backgroundImage2;
    public Sprite backgroundImage3;
    public Sprite backgroundImage4;
    public Sprite backgroundImage5;
    public Sprite backgroundImage6;
    public Sprite backgroundImage7;





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
        switch (card.id)
        {
            case 0:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage1;
                break;
            case 1:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage2;
                break;
            case 2:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage3;
                break;
            case 3:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage4;
                break;
            case 4:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage5;
                break;
            case 5:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage6;
                break;
            case 6:
                backgroundImage.GetComponent<Image>().sprite= backgroundImage7;
                break;
            default:
                break;
        }
        if (card is ElementCard)
        {
            var element = card as ElementCard;
            attackText.text = element.attack.ToString();
            levelPointText.text = element.levelPoint.ToString();
            effectText.gameObject.SetActive(false);

        }
        else if (card is SpecialCard)
        {
            var special = card as SpecialCard;
            effectText.text = special.effect;
            attackText.gameObject.SetActive(false);
            levelPointText.gameObject.SetActive(false);

        }
    }
}
