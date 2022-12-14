using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStorage : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialPos;
    public Vector2 defaultPos;

    public string jumpCollidersName;

    public void OnAfterDeserialize()
    {
        initialPos = defaultPos;
    }

    public void OnBeforeSerialize()
    {
    }
}
