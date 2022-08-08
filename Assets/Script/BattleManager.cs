using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GamePhase
{
    gameStart, playerDraw, playerAction, enemyDraw, enemyAction
}
public class BattleManager : MonoBehaviour
{
    public PlayData playData;
    public PlayData enemyData;

    public List<Card> playerDeckList = new List<Card>();
    public List<Card> enemyDeckList = new List<Card>();


    public GameObject cardPrefab;

    public Transform playerhand;
    public Transform enemyhand;

    public GameObject[] playerBlock;
    public GameObject[] enemyBlock;

    public GameObject playerIcon;
    public GameObject enemyIcon;

    public GamePhase GamePhase = GamePhase.gameStart;

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //加載數據-洗牌-抽基本手排
    //回合結束-遊戲階段
    public void GameStart()
    {
        ReadDeck();
        ShuffletDeck(0);
        ShuffletDeck(1);

        DrawCard(0, 5);
        DrawCard(1, 5);
        GamePhase = GamePhase.playerDraw;

    }

    public void ReadDeck()
    {
        Debug.Log(enemyData.playerDeck.Length);

        for (int i = 0; i < playData.playerDeck.Length; i++)
        {
            if (playData.playerDeck[i] != 0)
            {
                int count = playData.playerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    playerDeckList.Add(playData.CardStore.CopyCard(i));
                }
            }
        }
        for (int i = 0; i < enemyData.playerDeck.Length; i++)
        {

            if (enemyData.playerDeck[i] != 0)
            {
                int count = enemyData.playerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    enemyDeckList.Add(enemyData.CardStore.CopyCard(i));
                }
            }
        }
    }

    public void ShuffletDeck(int _player) //0 p 1 e
    {
        switch (_player)
        {
            case 0:


                for (int i = 0; i < playerDeckList.Count; i++)
                {
                    int rad = Random.Range(0, playerDeckList.Count);
                    Card temp = playerDeckList[i];
                    playerDeckList[i] = playerDeckList[rad];
                    playerDeckList[rad] = temp;
                }
                break;
            case 1:
                for (int i = 0; i < enemyDeckList.Count; i++)
                {
                    int rad = Random.Range(0, enemyDeckList.Count);
                    Card temp = enemyDeckList[i];
                    enemyDeckList[i] = enemyDeckList[rad];
                    enemyDeckList[rad] = temp;
                }
                break;
        }
    }

    public void OnPlayerDraw()
    {
        if (GamePhase == GamePhase.playerDraw)
        {
            DrawCard(0, 1);
        }

        GamePhase = GamePhase.playerAction;
    }

    public void OnEnemyDraw()
    {
        if (GamePhase == GamePhase.enemyDraw)
        {
            DrawCard(1, 1);
        }
        GamePhase = GamePhase.enemyAction;
    }

    public void DrawCard(int _player, int _count)
    {
        List<Card> drawDeck = new List<Card>();
        Transform hand = transform;
        switch (_player)
        {
            case 0:
                drawDeck = playerDeckList;
                hand = playerhand;

                break;
            case 1:
                drawDeck = enemyDeckList;
                hand = enemyhand;
                break;
        }

        for (int i = 0; i < _count; i++)
        {
            GameObject card = Instantiate(cardPrefab, hand);
            card.GetComponent<CardDisplay>().card = drawDeck[0];
            drawDeck.RemoveAt(0);
        }
    }
    public void OnClickTurnEnd()
    {
        TurnEnd();
    }
    public void TurnEnd()
    {
        if (GamePhase == GamePhase.playerAction)
        {
            GamePhase = GamePhase.enemyDraw;
        }
        else if (GamePhase == GamePhase.enemyAction)
        {
            GamePhase = GamePhase.playerDraw;
        }
    }


}
