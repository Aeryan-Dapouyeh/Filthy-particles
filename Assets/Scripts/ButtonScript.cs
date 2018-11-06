using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public GameManager gameManager;
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeTheMode(int typeOfbutton)
    {
        if(typeOfbutton == 0 && gameManager.mode != 0)
        {
            gameManager.mode = 0;
        }
        else if(typeOfbutton == 1 && gameManager.mode != 1)
        {
            gameManager.mode = 1;
        }
    }
}
