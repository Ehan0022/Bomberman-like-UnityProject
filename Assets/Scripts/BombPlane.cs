using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlane : MonoBehaviour
{
    [SerializeField] GameObject bombDropPoint;
    [SerializeField] private bool bombPresent = false;

    public Transform GetBombDropPointTransform()
    {
        return bombDropPoint.transform;
    }

    
}
