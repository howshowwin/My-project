using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase
{
    playerDraw, playerAction, enemyDraw, enemyAction, gameStart
}
/*public enum GameEvent
{
    phaseChange, monsterDestroy
}*/
public class BattleManager : MonoSingleton<BattleManager>
{
    public GameObject playerData; // 數據
    public GameObject enemyData;
    public GameObject playerHands; // 手牌
    public GameObject enemyHands;
    public GameObject[] playerBlocks; // 怪獸區
    public GameObject[] enemyBlocks;
    public List<Card> playerDeckList = new List<Card>(); // 卡组
    public List<Card> enemyDeckList = new List<Card>();

    public GameObject cardPrefab;

    public GameObject arrowPrefab;//召喚指示箭頭
    public GameObject attackPrefab;//攻擊指示箭頭
    private GameObject arrow;


    // 生命值
    public int playerHealthPoint;
    public int enemyHealthPoint;

    public GameObject playerIcon;
    public GameObject enemyIcon;

    // 召喚次數
    public int maxPlayerSummonCount;
    public int playerSummonCount;
    public int maxEnemySummonCount;
    public int enemySummonCount;


    public GamePhase currentPhase = GamePhase.playerDraw;


    protected CardData CardDate;

    public Transform canvas;


    private GameObject waitingMonster;
    private int waitingID;
    public GameObject attackingMonster;
    private int attackingID;

    public GameObject comboingMonster;
    private int comboingID;

    // private Dictionary<string, UnityEvent> eventDic = new Dictionary<string, UnityEvent>();
    public UnityEvent phaseChangeEvent;


    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerDrawCard()
    {
        if (currentPhase == GamePhase.playerDraw)
        {
            DrawCard(0, 1);
        }
    }
    public void OnEnemyDrawCard()
    {
        if (currentPhase == GamePhase.enemyDraw)
        {
            DrawCard(1, 1);
        }
    }
    public void DrawCard(int _player, int _number, bool _back = false, bool _state = true)
    {
        if (_player == 0)
        {
            for (int i = 0; i < _number; i++)
            {
                GameObject newCard = GameObject.Instantiate(cardPrefab, playerHands.transform);
                newCard.GetComponent<CardDisplay>().card = playerDeckList[0];
                playerDeckList.RemoveAt(0);
                newCard.GetComponent<BattleCard>().cardState = CardState.inPlayerHand;
                // 顯示卡背
                if (_back)
                {
                    newCard.GetComponent<CardDisplay>().back = true;
                }
                if (_state)
                {
                    currentPhase = GamePhase.playerAction;
                    phaseChangeEvent.Invoke();
                }
            }

        }
        else if (_player == 1)
        {
            for (int i = 0; i < _number; i++)
            {
                GameObject newCard = GameObject.Instantiate(cardPrefab, enemyHands.transform);
                newCard.GetComponent<CardDisplay>().card = enemyDeckList[0];
                enemyDeckList.RemoveAt(0);
                newCard.GetComponent<BattleCard>().cardState = CardState.inEnemyHand;
                // 顯示卡背
                if (_back)
                {
                    newCard.GetComponent<CardDisplay>().back = true;
                }
                if (_state)
                {
                    currentPhase = GamePhase.enemyAction;
                    phaseChangeEvent.Invoke();
                }
            }
        }
    }

    public virtual void OnClickTurnEnd()
    {
        TurnEnd();
    }
    public void TurnEnd()
    {

        if (arrow != null)
        {
            Destroy(arrow);
        }
        if (currentPhase == GamePhase.playerAction)
        {
            currentPhase = GamePhase.enemyDraw;
            enemySummonCount = maxEnemySummonCount;

            //playerIcon.GetComponent<AttackTarget>().attackable = true;
            //enemyIcon.GetComponent<AttackTarget>().attackable = false;
            changeCardClick(enemyBlocks, playerBlocks);
        }
        else if (currentPhase == GamePhase.enemyAction)
        {
            currentPhase = GamePhase.playerDraw;
            playerSummonCount = maxPlayerSummonCount;

            //playerIcon.GetComponent<AttackTarget>().attackable = false;
            //enemyIcon.GetComponent<AttackTarget>().attackable = true;
            changeCardClick(playerBlocks, enemyBlocks);


        }
        phaseChangeEvent.Invoke();
    }

    public void changeCardClick(GameObject[] arrOne, GameObject[] arrTwo)
    {
        foreach (var block in arrOne)
        {
            if (block.GetComponent<CardBlock>().monsterCard != null)
            {
                block.GetComponent<CardBlock>().monsterCard.GetComponent<AttackTarget>().attackable = false;
                block.GetComponent<CardBlock>().monsterCard.GetComponent<AttackTarget>().cancombo = true;

                block.GetComponent<CardBlock>().monsterCard.GetComponent<BattleCard>().hasAttacked = false;
            }
        }
        foreach (var block in arrTwo)
        {
            if (block.GetComponent<CardBlock>().monsterCard != null)
            {
                block.GetComponent<CardBlock>().monsterCard.GetComponent<AttackTarget>().cancombo = false;
                block.GetComponent<CardBlock>().monsterCard.GetComponent<AttackTarget>().attackable = true;
            }
        }
    }

    public void SummonRequst(Vector2 _startPoint, int _player, GameObject _monster) // 請求召喚
    {
        if (arrow != null)
        {
            Destroy(arrow);
        }
        if (_player == 0 && playerSummonCount >= 1)
        {
            arrow = GameObject.Instantiate(arrowPrefab, canvas);
            arrow.GetComponent<ArrowFollow>().SetStartPoint(_startPoint);
            foreach (var block in playerBlocks)
            {
                if (block.GetComponent<CardBlock>().monsterCard == null)
                {
                    block.GetComponent<CardBlock>().SetSummon();
                }
            }
            waitingMonster = _monster;
            waitingID = _player;
        }
        else if (_player == 1 && enemySummonCount >= 1)
        {
            arrow = GameObject.Instantiate(arrowPrefab, canvas);
            arrow.GetComponent<ArrowFollow>().SetStartPoint(_startPoint);
            foreach (var block in enemyBlocks)
            {
                if (block.GetComponent<CardBlock>().monsterCard == null)
                {
                    block.GetComponent<CardBlock>().SetSummon();
                }
            }
            waitingMonster = _monster;
            waitingID = _player;
        }
    }
    public void SummonCofirm(Transform _block) // 召喚確認，點及格子觸發
    {
        Summon(waitingMonster, waitingID, _block);
        waitingMonster = null;
    }

    /// <summary>
    /// 召喚怪獸
    /// </summary>
    /// <param name="_monster">要召喚的怪獸</param>
    /// <param name="_id">照患者編號</param>
    /// <param name="_block">要召喚的格子節點</param>
    public void Summon(GameObject _monster, int _id, Transform _block)
    {
        _monster.transform.SetParent(_block);
        _monster.GetComponent<CardDisplay>().ShowCard();
        _block.GetComponent<CardBlock>().monsterCard = _monster;
        //_block.GetComponent<CardBlock>().hasMonster = true;
        _monster.transform.localPosition = Vector3.zero;
        if (_id == 0)
        {
            _monster.GetComponent<BattleCard>().cardState = CardState.inPlayerBlock;
            playerSummonCount--;
            foreach (var block in playerBlocks)
            {
                block.GetComponent<CardBlock>().CloseAll();
            }
            changeCardClick(playerBlocks, enemyBlocks);
        }
        else if (_id == 1)
        {
            _monster.GetComponent<BattleCard>().cardState = CardState.inEnemyBlock;
            enemySummonCount--;
            foreach (var block in enemyBlocks)
            {
                block.GetComponent<CardBlock>().CloseAll();
            }
            changeCardClick(enemyBlocks, playerBlocks);
        }


        if (arrow != null)
        {
            Destroy(arrow);
        }
    }


    public void AttackRequst(Vector2 _startPoint, int _player, GameObject _monster)
    {
        if (arrow == null)
        {
            arrow = GameObject.Instantiate(attackPrefab, canvas);
        }

        arrow.GetComponent<ArrowFollow>().SetStartPoint(_startPoint);

        // 直接攻擊條件
        bool strightAttack = true;
        if (_player == 0)
        {
            foreach (var block in enemyBlocks)
            {
                if (block.GetComponent<CardBlock>().monsterCard != null)
                {
                    block.GetComponent<CardBlock>().SetAttack();
                    strightAttack = false;
                }
            }
            if (strightAttack)
            {
                // 可以攻擊玩家
            }
        }
        if (_player == 1)
        {
            foreach (var block in playerBlocks)
            {
                if (block.GetComponent<CardBlock>().monsterCard != null)
                {
                    block.GetComponent<CardBlock>().SetAttack();
                    strightAttack = false;
                }
            }
            if (strightAttack)
            {
                //  可以攻擊玩家
            }
        }

        attackingMonster = _monster;
        attackingID = _player;

    }


    public void ComboRequst(Vector2 _startPoint, int _player, GameObject _monster)
    {

        comboingMonster = _monster;
        comboingID = _player;

    }
    public void AttackCofirm(GameObject _target)
    {
        Attack(attackingMonster, attackingID, _target);
        attackingMonster = null;
    }

    public void ComboCofirm(GameObject _target)
    {
        // Attack();
        if (comboingMonster != _target)
        {
            Combo(comboingMonster, comboingID, _target);

        }


        comboingMonster = null;
    }

    public void Combo(GameObject _monster, int _id, GameObject _target)
    {
        if (arrow != null)
        {
            Destroy(arrow);
        }
        Debug.Log("合成成立");
    }

    public void Attack(GameObject _monster, int _id, GameObject _target)
    {
        //結算傷害
        //處理銷毀
        //恢復攻擊狀態，
        if (arrow != null)
        {
            Destroy(arrow);
        }
        _monster.GetComponent<BattleCard>().hasAttacked = true;
        Debug.Log("攻擊成立");

        // 
        var attackMonster = _monster.GetComponent<CardDisplay>().card as MonsterCard;
        var targetMonster = _target.GetComponent<CardDisplay>().card as MonsterCard;
        //Debug.Log(targetMonster.healthPoint);
        targetMonster.GetDamage(attackMonster.attack);
        if (targetMonster.healthPoint > 0)
        {
            _target.GetComponent<CardDisplay>().ShowCard();
        }
        else
        {
            Destroy(_target);
        }


        foreach (var block in playerBlocks)
        {
            block.GetComponent<CardBlock>().CloseAll();
        }
        foreach (var block in enemyBlocks)
        {
            block.GetComponent<CardBlock>().CloseAll();
        }
    }

    public virtual void GameStart() // 遊戲開始 讀取卡牌 抽卡
    {
        playerSummonCount = maxPlayerSummonCount;
        enemySummonCount = maxEnemySummonCount;
        CardDate = playerData.GetComponent<CardData>();

        currentPhase = GamePhase.gameStart;
        ReadDeck();
        //Debug.Log(currentPhase);
        DrawCard(0, 5);
        DrawCard(1, 5);
        currentPhase = GamePhase.playerDraw;
        //Debug.Log(currentPhase);
    }

    public void ReadDeck() // 從數據中讀卡
    {
        PlayerDataManager pdm = playerData.GetComponent<PlayerDataManager>();
        for (int i = 0; i < pdm.playerDeck.Length; i++)
        {
            if (pdm.playerDeck[i] != 0)
            {
                int counter = pdm.playerDeck[i];
                for (int j = 0; j < counter; j++)
                {
                    playerDeckList.Add(CardDate.CopyCard(i));
                }
            }
        }
        // 讀取玩家卡排
        PlayerDataManager edm = enemyData.GetComponent<PlayerDataManager>();
        for (int i = 0; i < edm.playerDeck.Length; i++)
        {
            if (edm.playerDeck[i] != 0)
            {
                int counter = edm.playerDeck[i];
                for (int j = 0; j < counter; j++)
                {
                    enemyDeckList.Add(CardDate.CopyCard(i));
                }
            }
        }
        ShuffletDeck(0);
        ShuffletDeck(1);
        foreach (var item in playerDeckList)
        {
            //Debug.Log(item.cardName);
        }
    }

    public void ShuffletDeck(int _player) // 將卡組洗牌，玩家0，對手1
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
}
