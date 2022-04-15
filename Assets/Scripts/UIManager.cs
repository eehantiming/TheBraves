using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI mouseSelectionName;
    [SerializeField] private TextMeshProUGUI mouseSelectionExtra;
    [SerializeField] private TextMeshProUGUI activeUnitText;
    [SerializeField] private TextMeshProUGUI gameMessageText;
    [SerializeField] private TextMeshProUGUI buttonHoverText;
    [SerializeField] private TextMeshProUGUI calamityCounterText;
    [SerializeField] private TextMeshProUGUI DiceRollText;
    [SerializeField] private GameObject playerLoseScreen, playerWinScreen;
    [SerializeField] private List<Button> playerButtons;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }
    /// <summary>
    /// Display the dice roll
    /// </summary>
    /// <param name="inputInt"></param>
    public void ShowDiceRollText(int inputInt)
    {
        DiceRollText.text = "Dice Roll: " + inputInt;
    }

    /// <summary>
    /// Displays the unit that is selected by left click in UI
    /// </summary>
    /// <param name="inputText"></param>
    public void ShowMouseSelectionText(string inputText)
    {
        mouseSelectionName.text = "Selected: " + inputText;
    }

    public void ShowMouseSelectionExtraInfo(string inputText)
    {
        mouseSelectionExtra.text = inputText;
    }

    public void ShowButtonHoverText(string inputText)
    {
        buttonHoverText.text = inputText;
    }
    /// <summary>
    /// Displays the current Active unit in UI
    /// </summary>
    /// <param name="inputText"></param>
    public void ShowActiveUnitText(string inputText)
    {
        if(inputText == null)
        {
            activeUnitText.gameObject.SetActive(false);
        }
        else
        {
            activeUnitText.text = "Active: " + inputText;
            activeUnitText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Displays message for the player
    /// </summary>
    /// <param name="inputText">Text to display. Set to null to disable text</param>
    public void ShowGameMessageText(string inputText)
    {
        if (inputText == null)
        {
            gameMessageText.gameObject.SetActive(false);
        }
        else
        {
            gameMessageText.text = inputText;
            gameMessageText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Displays the current Calamity Counter value
    /// </summary>
    /// <param name="currentCount"></param>
    public void ShowCalamityCount(int currentCount)
    {
        // TODO: animation to move counter
        calamityCounterText.text = $"Calamity: {currentCount}";
    }

    /// <summary>
    /// Displays the player lose screen
    /// </summary>
    public void ShowLoseText()
    {
        playerLoseScreen.SetActive(true);
    }

    /// <summary>
    /// Displays the player win screen
    /// </summary>
    public void ShowWinText()
    {
        playerWinScreen.SetActive(true);
    }

    /// <summary>
    /// Function to make each button in playerButtons non-interactable
    /// </summary>
    /// <param name="exceptions">button to keep interactable from playerButtons</param>
    public void DisableButtons(Button exceptions=null)
    {
        foreach(Button button in playerButtons)
        {
            if(button != exceptions)
                    button.interactable = false;
        }
    }

    /// <summary>
    /// Function to make each button in playerButtons interactable
    /// </summary>
    /// <param name="exceptions">buttons to keep non interactable from playerButtons</param>
    public void EnableButtons(Button exceptions=null)
    {
        foreach (Button button in playerButtons)
        {
            if (button != exceptions)
                button.interactable = true;
        }
    }
}
