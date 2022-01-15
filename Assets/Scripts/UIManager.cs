using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI mouseSelectionText;
    [SerializeField] private TextMeshProUGUI activeUnitText;
    [SerializeField] private TextMeshProUGUI gameMessageText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Displays the unit that is selected by left click in UI
    /// </summary>
    /// <param name="inputText"></param>
    public void ShowMouseSelectionText(string inputText)
    {
        mouseSelectionText.text = "Selected: " + inputText;
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
}
