using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DataMaterialSkin", menuName = "ScriptableObjects/Data Material Skin")]
public class DataMaterial : SerializedScriptableObject
{
    public Dictionary<Skin, List<Material>> dictMaterialsBody;
    public Dictionary<Skin, List<Material>> dictMaterialsDead;
}
