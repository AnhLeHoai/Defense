using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PrefabStorage : SerializedMonoBehaviour
{
    private static PrefabStorage instance;
    public static PrefabStorage Instance
    {
        get
        {
            if (instance == null)
            {
                var instances = FindObjectsOfType<PrefabStorage>();
                while (instances.Length > 1)
                {
                    Debug.LogWarning($"There shouldn't be more than one {nameof(PrefabStorage)}!");
                    Destroy(instances[instances.Length - 1]);
                }

                instance = instances[0];
            }
            
            return instance;
        }
    }
    
    public GameObject[] Hats;
    public GameObject DefaultHat;
    [Space]
    public Material[] skyboxes;

    private void Awake()
    {
        instance = this;
    }
}
