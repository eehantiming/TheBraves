using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public void HoverInfo()
    {
        UIManager.Instance.ShowGameMessageText("asd");
    }
    public void ResetHover()
    {
        UIManager.Instance.ShowGameMessageText("");
    }
}
