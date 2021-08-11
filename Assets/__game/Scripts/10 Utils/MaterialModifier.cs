using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    public bool isRandom = true;
    public Color mainColor = Color.HSVToRGB(0, 25, 87);

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (!isRandom)
        {
            var propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(SkinCharacterController.MAIN_COLOR_ID, mainColor);
            meshRenderer.SetPropertyBlock(propertyBlock);
        } else
        {
            Color.RGBToHSV(mainColor, out var hue, out var saturation, out var value);
            var newColor = Random.ColorHSV(
                0, 1, saturation, saturation, value, value);
            var propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(SkinCharacterController.MAIN_COLOR_ID, newColor);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
