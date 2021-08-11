using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialValueModifier : MonoBehaviour
{
	public int targetMaterialSlot;
	public string propertyName = "_MainTex";
	public float valueMin;
	public float valueMax;
	public float speed = 0.1f;

	private Renderer rend;
	private float value = 0;

	// Use this for initialization
	void Start()
	{
		rend = GetComponent<Renderer>();
		value = rend.materials[targetMaterialSlot].GetFloat(propertyName);
	}

	// Update is called once per frame
	void Update()
	{
		float newValue = value + speed;
		if (newValue < valueMin)
        {
			newValue = valueMin;
			speed *= -1;
        }
		else if (newValue > valueMax)
        {
			newValue = valueMax;
			speed *= -1;
		}
		value = newValue;


		rend.materials[targetMaterialSlot].SetFloat(propertyName, value);
	}
}
