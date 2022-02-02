using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    /// <summary>
    /// Generates a random number between 1 and 6
    /// </summary>
    public int Generate()
    {
        int roll = Random.Range(1,6);
        return (roll);
    }
}
