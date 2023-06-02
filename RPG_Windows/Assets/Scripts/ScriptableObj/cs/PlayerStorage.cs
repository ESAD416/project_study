using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters Storage/Player")]
public class PlayerStorage : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialPos;
    public Vector2 defaultPos;
    public float initialHeight;
    public float defaultHeight;

    public string jumpCollidersName;

    public void OnAfterDeserialize()
    {
        initialPos = defaultPos;
        initialHeight = defaultHeight;
    }

    public void OnBeforeSerialize()
    {
    }
}
