using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum GamePhase
{
    gameStart, playerDraw, playerAction, enemyDraw, enemyAction
}
public class BattleManager : Monosingletion<BattleManager>
{
    public static BattleManager Instance;
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

    public UnityEvent phaseChnageEvent = new UnityEvent();

    public int[] SummonCountMax = new int[2];
    private int[] SummonCounter = new int[2];

    private GameObject waitingCard;
    private int waitingPlayer;


    public GameObject ArrowPrefab;
    private GameObject arrow;
    public GameObject canvas;

    private GameObject attackingMonster;
    private int attackingPlayer;
    public GameObject attackArrow;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStart();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            DestroyArrow();
            waitingCard = null;
            CloseBlocks();
        }
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
        NextPhase();

        SummonCounter = SummonCountMax;

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
            NextPhase();
        }
    }

    public void OnEnemyDraw()
    {
        if (GamePhase == GamePhase.enemyDraw)
        {
            DrawCard(1, 1);
            NextPhase();
        }


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
            card.GetComponent<BattleCard>().playerID = _player;
            drawDeck.RemoveAt(0);
        }
    }
    public void OnClickTurnEnd()
    {
        TurnEnd();
    }
    public void TurnEnd()
    {
        if (GamePhase == GamePhase.playerAction || GamePhase == GamePhase.enemyAction)
        {
            NextPhase();
        }


    }
    public void NextPhase()
    {
        if ((int)GamePhase == System.Enum.GetNames(GamePhase.GetType()).Length - 1)
        {
            GamePhase = GamePhase.playerDraw;
        }
        else
        {
            GamePhase += 1;
        }
        phaseChnageEvent.Invoke();

    }

    public void AttackRequest(int _player, GameObject _elementCard)
    {
        GameObject[] blocks = playerBlock;
        bool hasMonsterBlock = false;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = enemyBlock;
        }
        else if (_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = playerBlock;
        }
        else
        {
            return;
        }
        foreach (var block in blocks)
        {
            if (block.GetComponent<Blockcon>().card != null)
            {
                block.GetComponent<Blockcon>().AttackBlock.SetActive(true);
                block.GetComponent<Blockcon>().card.GetComponent<AttackTarget>().attackable = true;
                hasMonsterBlock = true;
            }
        }
        if (hasMonsterBlock)
        {
            attackingMonster = _elementCard;
            attackingPlayer = _player;
            CreatArrow(_elementCard.transform, attackArrow);
        }

    }
    public void AttackConfirm(GameObject _target)
    {
        Attack(attackingMonster, _target);
        DestroyArrow();
        CloseBlocks();
        GameObject[] blocks;
        blocks = enemyBlock;
        if (attackingPlayer == 0)
        {
            blocks = enemyBlock;
        }
        else
        {
            blocks = playerBlock;
        }
        foreach (var block in blocks)
        {
            if (block.GetComponent<Blockcon>().card != null)
            {
                block.GetComponent<Blockcon>().card.GetComponent<AttackTarget>().attackable = false;
            }
        }
    }
    public void Attack(GameObject _attacker, GameObject _target)
    {
        ElementCard element = GetComponent<CardDisplay>().card as ElementCard;
        _target.GetComponent<AttackTarget>().ApplyDamage(element.attack);
        _attacker.GetComponent<BattleCard>().CostAttackCount();
        _target.GetComponent<CardDisplay>().ShowCard();
    }
    public void SummonRequest(int _player, GameObject _elementCard)
    {
        GameObject[] blocks = playerBlock;
        bool hasEmptyBlock = false;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = playerBlock;
        }
        else if (_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = enemyBlock;
        }
        else
        {
            return;
        }
        if (SummonCounter[_player] > 0)
        {
            foreach (var block in blocks)
            {
                if (block.GetComponent<Blockcon>().card == null)
                {
                    block.GetComponent<Blockcon>().SummonBlock.SetActive(true);
                    // waitingCard = _elementCard;
                    hasEmptyBlock = true;
                }
            }
        }
        if (hasEmptyBlock)
        {
            waitingCard = _elementCard;
            waitingPlayer = _player;
            CreatArrow(_elementCard.transform, ArrowPrefab);
            arrow.transform.SetParent(canvas.transform, false); ;
        }
    }
    public void SummonConfirm(Transform _block)
    {
        Summon(waitingPlayer, waitingCard, _block);
        GameObject[] blocks = playerBlock;
        if (waitingPlayer == 0)
        {
            blocks = playerBlock;
        }
        else if (waitingPlayer == 1)
        {
            blocks = enemyBlock;
        }
        foreach (var block in blocks)
        {
            block.GetComponent<Blockcon>().SummonBlock.SetActive(false);
        }
        DestroyArrow();
    }
    public void Summon(int _player, GameObject _elementCard, Transform _block)
    {
        _elementCard.transform.SetParent(_block);
        _elementCard.transform.localPosition = Vector3.zero;
        _elementCard.GetComponent<BattleCard>().state = BattleCardState.inBlock;
        _block.GetComponent<Blockcon>().card = _elementCard;
        SummonCounter[_player]--;

        ElementCard mc = _elementCard.GetComponent<CardDisplay>().card as ElementCard;
        _elementCard.GetComponent<BattleCard>().AttackCount = mc.attackCount;
        _elementCard.GetComponent<BattleCard>().ResetAttack();
    }


    public void CreatArrow(Transform _stratPoint, GameObject _prefab)
    {
        DestroyArrow();
        arrow = GameObject.Instantiate(_prefab, _stratPoint);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(_stratPoint.position.x, _stratPoint.position.y));

    }
    public void DestroyArrow()
    {
        Destroy(arrow);
        Destroy(attackArrow);

    }

    public void CloseBlocks()
    {
        foreach (var block in playerBlock)
        {
            block.GetComponent<Blockcon>().SummonBlock.SetActive(false);
            block.GetComponent<Blockcon>().AttackBlock.SetActive(false);

        }
        foreach (var block in enemyBlock)
        {
            block.GetComponent<Blockcon>().SummonBlock.SetActive(false);
            block.GetComponent<Blockcon>().AttackBlock.SetActive(false);

        }
    }
}
