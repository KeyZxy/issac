using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{

    void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1f);

   
}
