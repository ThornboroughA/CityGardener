using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The manifestation of a card in game space, handling its animations and visuals.
/// </summary>
public class ClickableCard : ClickableObject
{
    [Header("Card info")]
    public Card card;
    [HideInInspector] public bool isActive = false;

    [Header("Animation parameters")]
    [Tooltip("Distance the card moves vertically on hover.")]
    [SerializeField] private Vector3 movementDistance = new Vector3(0, 0, 0.08f);
    [Tooltip("Time the card floats before automatically moving back down.")]
    [SerializeField] private float hoverTime = 0.15f;

    private Coroutine animatingRoutine;


    #region Animations
    /// <summary>
    /// Animate the card to move upwards to set coordinates.
    /// </summary>
    public void AnimateUp()
    {
        if (animatingRoutine == null)
        {
            animatingRoutine = StartCoroutine(AnimateSelf(movementDistance, false));
        }
    }

    /// <summary>
    /// Animate the card to return to its starting position.
    /// </summary>
    public void AnimateDown()
    {
        if (animatingRoutine == null)
        {
            animatingRoutine = StartCoroutine(AnimateSelf(movementDistance, true));
        }
    }


    private IEnumerator AnimateSelf(Vector3 offset, bool downOnly)
    {
        // Animate up

        float waitTime = 0.15f;
        float elapsedTime = 0;
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.localPosition + offset;

        if (!downOnly) {  

            while (elapsedTime < waitTime) {
                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = endPosition;

            // Wait a period
            yield return new WaitForSeconds(hoverTime);

            // If card is still active, end routine and wait for inactivity before returning.
            if (isActive) {
                animatingRoutine = null;
                yield break; 
            }

        }

        // If card is no longer active, animate down.
        elapsedTime = 0;

        if (downOnly) {
            endPosition = transform.position;
            startPosition = transform.localPosition - offset;
        }

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;

        animatingRoutine = null;
    }

    #endregion Animations

}
