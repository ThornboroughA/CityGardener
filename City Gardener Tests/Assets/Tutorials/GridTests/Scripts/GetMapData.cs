using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GetMapData : MonoBehaviour
{


    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }

    }


    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Highlightable")
            {

                Vector3 hitRelative = hit.point - hit.transform.position;


                if (hit.transform.gameObject.GetComponent<Grid>())
                {
                    Grid grid = hit.transform.gameObject.GetComponent<Grid>();

                    grid.GetCellData((int)hitRelative.x, (int)hitRelative.z);
                }

            }
        }
    }

   


}
