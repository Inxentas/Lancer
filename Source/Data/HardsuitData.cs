using UnityEngine;

[CreateAssetMenu(fileName = "HardsuitData", menuName = "ScriptableObjects/HardsuitData")]

public class HardsuitData : ScriptableObject
{
    public string displayName = "Light Hardsuit";
    // tags

    // bonusses
    public int hp = 3;
    public bool flight = false;
    public bool invisibility = false;
    // stats
    public int armor = 0;
    public int evasion = 10;
    public int edefense = 10;
    public int speed = 4;
}
