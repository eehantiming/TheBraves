using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : HeroUnit
{
    public new void ActivateSkill()
    {
        base.ActivateSkill();
        Debug.Log("Set Trap");
    }

}
