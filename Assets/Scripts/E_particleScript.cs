using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_particleScript : E_particle {

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
        if(other.tag == "p_particle")
        {
            GameObject PE_par = Instantiate(PE_particle, other.transform);
            PE_par.GetComponent<E_particle>().isA_PE_Bond = true;
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.tag == "N_particle")
        {
            GameObject NE_par = Instantiate(NE_particle, other.transform);
            NE_par.GetComponent<E_particle>().isA_NE_bond = true;
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.tag == "E_particle")
        {
            GameObject two_Es = Instantiate(binaryBond, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            two_Es.GetComponent<E_particle>().isOnAPoint = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
