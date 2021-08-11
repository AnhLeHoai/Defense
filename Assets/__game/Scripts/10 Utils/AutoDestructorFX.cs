using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestructorFX : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        SimplePool.Despawn(gameObject);
    }
}
