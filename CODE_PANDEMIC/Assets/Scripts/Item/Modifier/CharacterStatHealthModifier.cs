using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifier : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Debug.Log($"HpUP : {val}");
    }
}
