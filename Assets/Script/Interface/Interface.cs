using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    void TakeDamage(float damage, DieType dieType, Vector3 hitPosition);
}

public interface ITakeFlameDamage
{
    void TakeFlameDamage(float damage, float timeExist, DieType dieType, Vector3 hitPosition);
}

public interface IUpdatePrice
{
    void UpdatePrice();
}