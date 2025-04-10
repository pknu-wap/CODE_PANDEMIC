using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifier : CharacterStatModifier
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Debug.Log($"HpUP : {val}");
    }
}
