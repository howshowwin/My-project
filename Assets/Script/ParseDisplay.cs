using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParseDisplay : MonoBehaviour
{
    // public BattleManager BattleManager;
    public Text phaseText;
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.phaseChnageEvent.AddListener(UpdataText);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdataText(){
        phaseText.text = BattleManager.Instance.GamePhase.ToString();
    }
}
