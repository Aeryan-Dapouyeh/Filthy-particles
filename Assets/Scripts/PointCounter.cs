using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PointCounter : MonoBehaviour {

    public GameObject gameManager;

    private Text textComponent;

    private void Awake()
    {
        textComponent = gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        textComponent.text = "0";
	}
	
	// Update is called once per frame
	void Update () {

	}
}
