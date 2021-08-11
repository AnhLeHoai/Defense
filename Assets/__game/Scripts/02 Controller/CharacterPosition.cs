using UnityEngine;

[System.Serializable]
public class CharacterPosition
{
    [SerializeField] private Vector3 position;

    public Vector3 Position => position;

    public CharacterPosition()
    {
    }

    public CharacterPosition(Vector3 position)
    {
        this.position = position;
    }
}