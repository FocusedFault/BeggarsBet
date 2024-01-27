using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField] public int sideValue;
    private Dice dice;

    private void Start()
    {
        dice = GetComponentInParent<Dice>();
    }

    private void OnTriggerEnter(Collider other)
    {
        dice.diceValue = sideValue;
    }

    private void OnTriggerExit(Collider other)
    {
        dice.diceValue = 0;
    }
}
