using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IShooter
{
    public int Damage { get; set; }

    public void Shoot();
}
