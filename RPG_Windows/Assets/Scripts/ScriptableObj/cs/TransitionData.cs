using UnityEngine;

[CreateAssetMenu(menuName = "Parameter Storage/Scence Transition")]
public class TransitionData : ScriptableObject, ISerializationCallbackReceiver
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
