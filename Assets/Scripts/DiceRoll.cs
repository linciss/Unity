using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngineInternal;
public class DiceRoll : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    Vector3 pos;
    [SerializeField] private float maxForce, startRollingForce; 
    private float forceX, forceY, forceZ;
    public string rolledNumber;
    public string rolledNumberUI;
    public bool isLanded = false; 
    private bool firstThrow = false;

    public int timesThrown = 0;
    private GameLoop gameLoop;



    void Start()
    {
        Initialize();
        gameLoop = FindObjectOfType<GameLoop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;

        if ((Input.GetMouseButton(0) && (isLanded || !firstThrow)) && !gameLoop.isAITurn && !gameLoop.gamePaused){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null && hit.collider.gameObject == this.gameObject) {
                    if (!firstThrow)
                        firstThrow = true;
                    timesThrown++;
                    rollDice();
                }
            }
        }

    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        pos = transform.position;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
    }
    public bool isRolled = false;
    public void rollDice()
    {

        rb.isKinematic = false;
        forceX = Random.Range(0, maxForce);
        forceY = Random.Range(0, maxForce);
        forceZ = Random.Range(0, maxForce);

        rb.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rb.AddTorque(forceX, forceY, forceZ);

        isRolled = true;


    }

    public void resetDice()
    {
        rb.isKinematic = true;
        firstThrow = false;
        isLanded = false;
        transform.position = pos;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
        rolledNumber = "";
    }

}
