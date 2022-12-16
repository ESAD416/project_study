using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TransitionStorage : ScriptableObject, ISerializationCallbackReceiver
{
    public bool fadeTransitionActive = false;

    public void OnAfterDeserialize()
    {
        fadeTransitionActive = false;
    }

    public void OnBeforeSerialize()
    {
    }

}
