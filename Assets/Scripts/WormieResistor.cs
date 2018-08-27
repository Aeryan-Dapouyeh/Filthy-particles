using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormieResistor : MonoBehaviour {

    public float resistanceFactor = 2;
    [SerializeField]
    public Wormie thisWormie;

    private Vector2 _pos;
    private int typeOfWormie;
    private int lengthOfWormie;    

	void Awake () {
        typeOfWormie = FindWormieType();
        lengthOfWormie = (typeOfWormie + 1) * 10;
        _pos = transform.position;
        thisWormie = new Wormie(typeOfWormie, lengthOfWormie, _pos, resistanceFactor, FindRotationType());
	}
	private int FindWormieType()
    {
        if (gameObject.tag == "Wormie(1r)")
        {
            return 0;
        }
        else if (gameObject.tag == "Wormie(2r)")
        {
            return 1;
        }
        else if (gameObject.tag == "Wormie(3r)")
        {
            return 2;
        }
        else
        {
            Debug.Log("Not a registered wormie");
            return -1;
        }
    }
    private int FindRotationType()
    {
        if(gameObject.transform.rotation.z == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    void Update () {
        thisWormie = new Wormie(typeOfWormie, lengthOfWormie, _pos, resistanceFactor, FindRotationType());
        //Debug.Log("Position: " + thisWormie.position + " resistanceValue: " + thisWormie.resistanceValue);
        //Debug.Log("This wormie is of type " + (thisWormie.type + 1) + " and of length " + thisWormie.length + " unity units, located at " + thisWormie.position + " it has a resistance of " + thisWormie.resistanceValue + ".");
	}

    public struct Wormie
    {
        public int type;
        public int length;
        public Vector2 position;
        public float resistanceValue;
        public int rotationType; // if 0 then horizontal, if 1, vertical

        public Wormie(int Type, int Length, Vector2 Position, float _resistanceValue, int _rotationType)
        {
            type = Type;
            length = Length;
            position = Position;
            resistanceValue = _resistanceValue;
            rotationType = _rotationType;
        }
    }
}
