using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public GameManager gameManagerScript;
    public Text currencyDisplayer;
    public int atomicMoney = 0;
    private void Awake()
    {
        if(gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }
    private void Update()
    {
        atomicMoney = gameManagerScript.atomiccurreny;
        currencyDisplayer.text = atomicMoney.ToString();
    }
}
