using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{

    [SerializeField] public GameObject diceToRoll;
    [SerializeField] public GameObject[] availableDice;
    public delegate void FinishRoll(int value);
    public static event FinishRoll OnFinishRoll;
    public string diceName;
    public bool hasRolled = false;
    private Rigidbody rb;
    private bool isRolling = false;
    private float rollForce = 10f;
    private float stopThreshold = 0.01f;
    private float stopTimeThreshold = 0.1f;
    private GameObject dieInstance;
    private int dieValue = 0;

    void Update()
    {
        if (!isRolling && !hasRolled)
        {
            RollDice();
        }
    }
    public void SendRollValue(int valueToSend)
    {
        // Check if there are any subscribers to the event
        if (OnFinishRoll != null)
        {
            // Trigger the event and pass the value
            OnFinishRoll(valueToSend);
        }
    }
    void RollDice()
    {
        isRolling = true;
        GameObject die = Instantiate(diceToRoll, new Vector3(-1.17884755f, 8.42399788f, -3.65027475f), Quaternion.identity);
        rb = die.GetComponent<Rigidbody>();
        dieInstance = die;

        // Randomize the rotation of the dice
        Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * rollForce;

        // Add a force to the dice to make it roll
        if (dieInstance && rb)
        {
            rb.AddForce(Vector3.up * rollForce, ForceMode.Impulse);
            rb.AddTorque(randomTorque, ForceMode.Impulse);

            // Start checking when the Rigidbody stops moving
            StartCoroutine(CheckStopped());
        }
    }

    IEnumerator CheckStopped()
    {
        yield return new WaitForSeconds(1.0f); // Adjust the initial delay as needed

        float timer = 0f;

        while (timer < stopTimeThreshold)
        {
            if (rb && rb.velocity.magnitude < stopThreshold)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f; // Reset the timer if the Rigidbody is still moving
            }
            yield return null;
        }

        // The Rigidbody has stopped moving for the specified duration
        // Now, start detecting the upward face
        StartCoroutine(DetectUpwardFace());
    }

    IEnumerator DetectUpwardFace()
    {
        yield return new WaitForSeconds(1.0f); // Adjust the delay as needed

        string name = diceToRoll.GetComponent<Dice>().diceName;
        Ray[] rays;
        // Create an array of rays to check each face of the die
        switch (name)
        {
            case "D4":
                rays = new Ray[]
                {
                    new Ray(rb.transform.position, rb.transform.up) , // 1
                    new Ray(rb.transform.position, rb.transform.forward) , // 2
                    new Ray(rb.transform.position, -rb.transform.forward), // 3
                    new Ray(rb.transform.position, -rb.transform.up) // 4
                };
                break;
            default:
                rays = new Ray[] { };
                break;
        }


        float maxHitDistance = 0.0f;
        int upwardFaceIndex = -1;

        foreach (Ray ray in rays)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                float hitDistance = hit.distance;
                if (hitDistance > maxHitDistance)
                {
                    maxHitDistance = hitDistance;
                    upwardFaceIndex = Array.IndexOf(rays, ray) + 1;
                }
            }
        }

        Dice dice = dieInstance.GetComponent<Dice>();

        if (upwardFaceIndex == -1)
        {
            if (dice.diceValue == 0)
            {
                GameObject.Destroy(dieInstance);
                RollDice();
            }
            else
                dieValue = dice.diceValue;
        }
        else
            dieValue = upwardFaceIndex;

        hasRolled = true;
        isRolling = false;
        rb = null;
        GameObject.Destroy(dieInstance);
        switch (dice.diceName)
        {
            case "D4":
                SendRollValue(2);
                break;
            case "D6":
                SendRollValue(3);
                break;
            case "D8":
                SendRollValue(4);
                break;
            case "D10":
                SendRollValue(5);
                break;
            case "D12":
                SendRollValue(6);
                break;
            case "D20":
                SendRollValue(10);
                break;
        }
    }
}
