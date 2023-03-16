using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// The main card management class. Currently handles their animations, instantiation, etc.
/// </summary>
[RequireComponent(typeof(HandLayout))]
public class CardManager : MonoBehaviour
{
    /*
     
    - if card is hovered over, moves up slightly
    - if card is selected, moves to the front + outline
    - card can be dragged

     */

    // The currently active card.
    public ClickableCard activeCard;

    [HideInInspector]
    public HandLayout handLayout;


    private void OnEnable()
    {
        CameraRayHandler.objectExit += CardExit;
        CameraRayHandler.objectEnter += CardEnter;
    }
    private void OnDisable()
    {
        CameraRayHandler.objectExit -= CardExit;
        CameraRayHandler.objectEnter -= CardEnter;
    }

    private void Start()
    {
        handLayout = GetComponent<HandLayout>();

        for (int i = 0; i < 5;  i++)
        {
            handLayout.AddCardAnchor();
        }
    }


    private void CardEnter()
    {
        activeCard = VerifyIsCard();
        activeCard.isActive = true;
        activeCard.AnimateUp();

        Debug.Log(activeCard.card.Name);
    }
    private void CardExit()
    {
        activeCard.isActive = false;
        activeCard.AnimateDown();
        activeCard = null;
    }

    private ClickableCard VerifyIsCard()
    {
        ClickableCard card = null;

        if (CameraRayHandler.lastHoveredObject != null)
        {
            if (CameraRayHandler.lastHoveredObject.CompareTag("Card"))
            {
                card = CameraRayHandler.lastHoveredObject.GetComponent<ClickableCard>();
                //Debug.Log(activeCard.card.Name);
            }
        }

        return card;
    }


}
