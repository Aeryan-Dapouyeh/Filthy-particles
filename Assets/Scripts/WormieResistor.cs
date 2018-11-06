using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormieResistor : MonoBehaviour {

    public float resistanceFactor = 2; // the amount of resistance a wormie will put on a moving particle
    [SerializeField]
    public Wormie thisWormie; // this wormie, cunstructed as an object
    public int lengthOfWormie;

    private Vector2 _pos; // the position of this wormie
    private int typeOfWormie; // the type of wormie, according to it's length
       

	void Awake () {
        typeOfWormie = FindWormieType(); // find the type of the wormie
        lengthOfWormie = (typeOfWormie + 1) * 10; // find the legnth of the wormie
        _pos = transform.position; // assign a value to it's position
        thisWormie = new Wormie(typeOfWormie, lengthOfWormie, _pos, resistanceFactor, FindRotationType()); // use our construct to make an object of every single wormie
	}
	private int FindWormieType() // a function to determine the type of wormie according to its tag
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
    private int FindRotationType() // a function to detmermine wether the wormie is vertical or horizontal
    {
        if(gameObject.transform.rotation.z == 0)
        {
            return 0; // zero for horizontal
        }
        else
        {
            return 1; // one for vertical
        }
    }
    void Update () {
        thisWormie = new Wormie(typeOfWormie, lengthOfWormie, _pos, resistanceFactor, FindRotationType()); // update the state of this wormie frame, by frame
	}

    public struct Wormie // a struct to help us make this wormie
    {
        public int type; 
        public int length;
        public Vector2 position;
        public float resistanceValue;
        public int rotationType; // if 0 then horizontal, if 1, vertical

        public Wormie(int Type, int Length, Vector2 Position, float _resistanceValue, int _rotationType) // the syntax defined for the struct
        {
            type = Type;
            length = Length;
            position = Position;
            resistanceValue = _resistanceValue;
            rotationType = _rotationType;
        }
    }
}
