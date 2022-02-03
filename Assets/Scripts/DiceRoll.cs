using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public static DiceRoll Instance;
    void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Generates a random number between 1 and 6
    /// </summary>
    public int Generate()
    {
        int roll = Random.Range(1,7);
        UIManager.Instance.ShowDiceRollText(roll);
        return (roll);
    }
}
