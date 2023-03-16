using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The root class for a card, holding its base data.
/// </summary>
[System.Serializable]
public class Card
{
    public string Name;

    public Card()
    {
        Name = "Default Card";
    }
}
