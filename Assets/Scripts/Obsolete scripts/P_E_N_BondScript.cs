using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_E_N_BondScript : MovableObject {

    private bool canMove = true;
    private float inverseMoveTime;
    private GameManager gameManagerScript;
    private int horizontal = 0;
    private int vertical = 0;
    private Rigidbody2D rb2d;
    private Vector2 _pos;
    private Vector2 targetPosition;

    public GameObject atiePrefab;
    public int pointsPer_A_CollisionForPE = 35;
    public int pointsPer_A_CollisionForNE = 5;

    protected override void Awake()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        inverseMoveTime = 1.0f / moveTime;
        rb2d = GetComponent<Rigidbody2D>();
        _pos = GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	protected override void Update () {
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        Move(horizontal, vertical, targetPosition);
    }

    protected override void Move(int horizontalInput, int verticalInput, Vector2 target)
    {
        if (target == Vector2.zero)
        {
            target = _pos + new Vector2(horizontalInput, verticalInput) * movingDistance;
        }
        if (target != Vector2.zero)
        {
            Vector2 current = transform.position;
            if (current == target)
            {
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }
            if (current == Vector2.zero)
            {
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }
        }

        if (verticalInput != 0 || horizontalInput != 0)
        {
            if (canMove)
            {
                StartCoroutine(Smoothing(target));
                canMove = false;
            }
        }
        targetPosition = target;
    }

    private IEnumerator Smoothing(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "a_particle")
        {
            Destroy(other.gameObject);
            if (gameObject.tag == "PE_bond")
                gameManagerScript.Points += pointsPer_A_CollisionForPE;
            if (gameObject.tag == "NE_bond")
                gameManagerScript.Points += pointsPer_A_CollisionForNE;
        }
        if ((other.tag == "N_particle" && gameObject.tag == "PE_bond") || (other.tag == "p_particle" && gameObject.tag == "NE_bond"))
        {
            Instantiate(atiePrefab, other.transform);
            other.transform.GetChild(0).parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
