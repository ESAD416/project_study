using UnityEngine;

[CreateAssetMenu(menuName = "Parameter Storage/Charactor")]
public class CharactorData : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialPos;
    public Vector2 defaultPos;
    public int initialHeight;
    public int defaultHeight;

    public void OnAfterDeserialize()
    {
        initialPos = defaultPos;
        initialHeight = defaultHeight;
    }

    public void OnBeforeSerialize()
    {
    }
}
