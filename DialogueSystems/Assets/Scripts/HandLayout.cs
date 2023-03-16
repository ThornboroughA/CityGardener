using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that stores the layout of the card anchors. 
/// </summary>
public class HandLayout : MonoBehaviour 
{

    // TODO: there's no reason this shouldn't be static with pre-fixed positions for each of the possible locations.

    public List<GameObject> activeCardAnchors = new List<GameObject>();

    private int currentAnchors = 0;


    /// <summary>
    /// Add a new anchor to the hand, to allow for a new card. 
    /// Its location is predetermined based on CardHandCoordinates.
    /// </summary>
    public void AddCardAnchor()
    {
        currentAnchors++;

        Vector3 anchorCoords = new Vector3((CardHandCoordinates.cardOffset.x * currentAnchors), 0, 0);
        GameObject newAnchor = Instantiate(gameObject, anchorCoords, Quaternion.identity, transform);
        activeCardAnchors.Add(newAnchor);

        //activeCardAnchors.Add(new Vector3((CardHandCoordinates.cardOffset.x * currentAnchors),0,0));
        //Debug.Log("Adding card anchor #" + currentAnchors + " at pos of " + activeCardAnchors[activeCardAnchors.Count-1]);


    }
    /// <summary>
    /// Removes the final card anchor in the list.
    /// </summary>
    public void RemoveCardAnchor()
    {
        currentAnchors--;
        activeCardAnchors.Remove(activeCardAnchors[activeCardAnchors.Count-1]);
    }
}


public static class CardHandCoordinates
{

    public static Vector3 cardOffset = new Vector3(0.08f, 0, 0);

}