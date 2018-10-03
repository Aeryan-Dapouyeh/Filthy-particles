using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_particleScript : UltimatePlayerScript {

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        base.OnTriggerEnter2D(other);
        if(other.tag == "p_particle")
        {
            GameObject PE_par = Instantiate(PE_particle, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.tag == "N_particle")
        {
            GameObject NE_par = Instantiate(NE_particle, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.tag == "E_particle")
        {
            GameObject two_Es = Instantiate(binaryBond, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            two_Es.GetComponent<eBond_particleScript>().isOnAPoint = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
