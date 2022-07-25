public class Card
{
    public int id;
    public string cardName;

    public Card(int _id, string _cardName)
    {
        this.id = _id;
        this.cardName = _cardName;
    }
}


public class ElementCard : Card
{
    public int attack;
    public int levelPoint;
    // public int typeElement;


    public ElementCard(int _id, string _cardName, int _attack, int _levelPoint) : base(_id, _cardName)
    {
        this.attack = _attack;
        this.levelPoint = _levelPoint;
        // this.typeElement = _typeElement;
    }
}


public class SpecialCard : Card
{
    public string effect;
    public SpecialCard(int _id, string _cardName, string _effect) : base(_id, _cardName)
    {
        this.effect = _effect;
    }
}