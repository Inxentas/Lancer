using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "ScriptableObjects/BackgroundData")]

public class BackgroundData : ScriptableObject
{
    public string displayName = "Celebrity";
    public TriggerData[] exampleTriggers = new TriggerData[4];
}
