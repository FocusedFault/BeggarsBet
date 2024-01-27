using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DicePicker : MonoBehaviour
{
    [SerializeField] public Button prevBtn;
    [SerializeField] public Button nextBtn;
    [SerializeField] public Button selectBtn;
    [SerializeField] public TextMeshProUGUI displayName;
    [SerializeField] public GameObject[] diceDisplays;
    public List<string> availableDice = new() { "D4", "D6", "D8", "D10", "D12", "D20" };
    // Define a delegate and an event to handle the event
    public delegate void DieValueSubmit(string value);
    public static event DieValueSubmit OnDieValueSubmit;
    private List<GameObject> availableDiceDisplays = new();
    private int currentDiceIndex = 0;
    private int diceCount = 0;

    void Start()
    {
        prevBtn.interactable = false;
        GetAvailableDice();

        prevBtn.onClick.AddListener(PrevButtonClick);
        nextBtn.onClick.AddListener(NextButtonClick);
        selectBtn.onClick.AddListener(SelectButtonClick);
    }

    public void GetAvailableDice()
    {
        prevBtn.interactable = false;
        nextBtn.interactable = true;

        currentDiceIndex = 0;
        availableDiceDisplays.Clear();

        foreach (string diceName in availableDice)
        {
            foreach (GameObject diceDisplay in diceDisplays)
            {
                if (diceDisplay.GetComponent<DiceDisplay>().diceName == diceName)
                    availableDiceDisplays.Add(diceDisplay);
            }
        }

        diceCount = availableDiceDisplays.Count - 1;
        if (diceCount > 0)
        {
            availableDiceDisplays[currentDiceIndex].SetActive(true);
            displayName.text = availableDiceDisplays[currentDiceIndex].GetComponent<DiceDisplay>().diceName;
        }
    }

    private void PrevButtonClick()
    {
        if (nextBtn.interactable == false)
            nextBtn.interactable = true;
        if (currentDiceIndex - 1 <= 0)
            prevBtn.interactable = false;
        availableDiceDisplays[currentDiceIndex].SetActive(false);
        currentDiceIndex--;
        availableDiceDisplays[currentDiceIndex].SetActive(true);
        displayName.text = availableDiceDisplays[currentDiceIndex].GetComponent<DiceDisplay>().diceName;
    }
    private void NextButtonClick()
    {
        if (prevBtn.interactable == false)
            prevBtn.interactable = true;
        if (currentDiceIndex + 1 >= diceCount)
            nextBtn.interactable = false;
        availableDiceDisplays[currentDiceIndex].SetActive(false);
        currentDiceIndex++;
        availableDiceDisplays[currentDiceIndex].SetActive(true);
        displayName.text = availableDiceDisplays[currentDiceIndex].GetComponent<DiceDisplay>().diceName;
    }

    private void SelectButtonClick()
    {
        availableDiceDisplays[currentDiceIndex].SetActive(false);
        SelectDice(availableDiceDisplays[currentDiceIndex].GetComponent<DiceDisplay>().diceName);
    }

    public void SelectDice(string valueToSend)
    {
        // Check if there are any subscribers to the event
        if (OnDieValueSubmit != null)
        {
            // Trigger the event and pass the value
            OnDieValueSubmit(valueToSend);
        }
    }
}
