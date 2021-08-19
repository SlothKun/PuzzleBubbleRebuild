using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlace : MonoBehaviour
{
    public bool occupied;
    public GameObject Bobble;

    void Update()
    {
        if (!Bobble)
        {
            occupied = false;
        }
    }
}
