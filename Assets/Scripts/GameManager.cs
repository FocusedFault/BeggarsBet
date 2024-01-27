using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject pickContainer;
    [SerializeField] public GameObject rollContainer;
    [SerializeField] public DiceRoller diceRoller;
    [SerializeField] public DicePicker dicePicker;
    [SerializeField] public TextMeshProUGUI playerPointsText;
    [SerializeField] public TextMeshProUGUI AIPointsText;
    public string selectedDice;
    public int playerPoints = 0;
    public int AIPoints = 0;

    // Start is called before the first frame update
    private void OnEnable()
    {
        playerPointsText.text = "You - " + playerPoints;
        AIPointsText.text = "AI - " + AIPoints;
        // Subscribe to the event when this component becomes active/enabled
        DiceRoller.OnFinishRoll += HandleFinishRoll;
        DicePicker.OnDieValueSubmit += HandleDiceSelect;
    }

    private void OnDisable()
    {
        // Subscribe to the event when this component becomes active/enabled
        DicePicker.OnDieValueSubmit -= HandleDiceSelect;
        DiceRoller.OnFinishRoll -= HandleFinishRoll;
    }

    private void HandleFinishRoll(int valueReceived)
    {
        string usedDie = "D" + (valueReceived * 2).ToString();
        dicePicker.availableDice.Remove(usedDie);
        dicePicker.GetAvailableDice();

        playerPoints += valueReceived;
        UpdatePointsUI();

        rollContainer.SetActive(false);
        pickContainer.SetActive(true);
    }

    private void UpdatePointsUI()
    {
        playerPointsText.text = "You - " + playerPoints;
        AIPointsText.text = "AI - " + AIPoints;
    }

    private void HandleDiceSelect(string valueReceived)
    {
        selectedDice = valueReceived;
        pickContainer.SetActive(false);
        rollContainer.SetActive(true);
        diceRoller.hasRolled = false;
        foreach (GameObject die in diceRoller.availableDice)
        {
            if (die.GetComponent<Dice>().diceName == selectedDice)
            {
                die.SetActive(true);
                diceRoller.diceToRoll = die;
            }
        }
    }
}
