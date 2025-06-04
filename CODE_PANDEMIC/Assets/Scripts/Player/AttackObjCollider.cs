using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjCollider : MonoBehaviour
{
    [SerializeField] PlayerController _controller;

    public void TakeDamage(GameObject attacker, int damage)
    {
        _controller.TakeDamage(attacker , damage);
    }
}
