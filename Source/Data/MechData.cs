using UnityEngine;

[CreateAssetMenu(fileName = "MechData", menuName = "ScriptableObjects/MechData")]

public class MechData : ScriptableObject
{
    public string displayName = "Everest";
    public ManufacturerData manufacturer;

    [Header("CORE")]
    public int armor = 0;
    public int savetarget = 10;
    public int sensors = 10;

    [Header("HULL")]
    public int hp = 10;
    public int repcap = 5;

    [Header("AGILITY")]
    public int evasion = 8;
    public int speed = 4;
    
    [Header("SYSTEMS")]
    public int edefense = 8;
    public int techattack = 0;
    public int sp = 6;

    [Header("ENGINEERING")]
    public int heatcap = 6;
}
