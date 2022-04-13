using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public Sprite diceOne;
    public Sprite diceTwo;
    public Sprite diceThree;
    public Sprite diceFour;
    public Sprite diceFive;
    public Sprite diceSix;

    public SpriteRenderer spriteRenderer;

    public static DiceRoll Instance;
    void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Generates a random number between 1 and 6
    /// </summary>
    public int GenerateRoll()
    {
        int roll = Random.Range(1,7);
        UIManager.Instance.ShowDiceRollText(roll);
        Debug.Log("Roll: " + roll);
        if(roll == 1)
        {
            ChangeSprite(diceOne);
        }
        else if(roll == 2)
        {
            ChangeSprite(diceTwo);
        }
        else if(roll == 3)
        {
            ChangeSprite(diceThree);
        }
        else if(roll == 4)
        {
            ChangeSprite(diceFour);
        }
        else if(roll == 5)
        {
            ChangeSprite(diceFive);
        }
        else if(roll == 6)
        {
            ChangeSprite(diceSix);
        }


        return (roll);
    }

    public void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}
