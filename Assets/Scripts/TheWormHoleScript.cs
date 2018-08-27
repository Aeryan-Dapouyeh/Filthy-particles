using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWormHoleScript : MonoBehaviour {

    public bool isActive = true;
    public bool timeSet = false;
    public float recoveryTime = 5.0f;
    public GameObject wormHoleBond;

    private E_particle playerScript;
    private float deActivationMoment;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<E_particle>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(isActive + " " + wormHoleBond.GetComponent<TheWormHoleScript>().isActive);
        wormHoleActivationUpdate();
	}

    private void wormHoleActivationUpdate()
    {
        if (isActive == false)
        {
            //StartCoroutine(ReactivateTheWormhole());  //bugs: 1) moves several times between the wormholes 2) can't move when used twice
            if (timeSet == false)
            {
                deActivationMoment = Time.time;
                //Debug.Log("deactivation time set to: " + deActivationMoment);
                timeSet = true;
            }
            if (timeSet)
            {
                if (Time.time - deActivationMoment - recoveryTime >= 0.1)
                {
                    isActive = true;
                    wormHoleBond.GetComponent<TheWormHoleScript>().isActive = true;
                    timeSet = false;
                }
            }
        }
    }

    IEnumerator ReactivateTheWormhole()
    {
        yield return new WaitForSeconds(recoveryTime);
        isActive = true;
        if(playerScript.wormHoleTransitation == true)
        {
            playerScript.wormHoleTransitation = false;
        }
        Debug.Log("is Active noew!");
    }
}
