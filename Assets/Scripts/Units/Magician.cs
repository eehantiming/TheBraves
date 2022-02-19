using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : HeroUnit
{
    public new void ActivateSkill()
    {
        base.ActivateSkill();
        Debug.Log("Teleport");
    }

}
