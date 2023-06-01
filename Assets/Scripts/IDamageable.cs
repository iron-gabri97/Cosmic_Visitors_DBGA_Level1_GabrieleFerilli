using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageable
{
    public int Health { get; set; }

    public void TakeDamage(GameObject damagedObject, int damage);
}
