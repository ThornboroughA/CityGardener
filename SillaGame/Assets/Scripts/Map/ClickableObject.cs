using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrategyCamera;

namespace MapData { 
    public class ClickableObject : MapObject
    {

        private int numClicks = 0;

        public void OnMouseDown()
        {
            SingleClickBehavior();

            // Check for double click
            numClicks++;
            StartCoroutine(DoubleClickTimeout());
            if (numClicks != 2)
                return;
            numClicks = 0;

            DoubleClickBehavior();
        }


        private void SingleClickBehavior()
        {
            // Display selection ring around the object

            // Open the object's info panel
        }
        private void DoubleClickBehavior()
        {
            CameraController.instance.CenterCamOnObj(transform);
        }


        private IEnumerator DoubleClickTimeout()
        {
            //HACK: this should probably be converted to a global static managed by the mouse pointer instead, then checked against with each clickable object.

            yield return new WaitForSeconds(1);

            numClicks = 0;
        }
    }
}
