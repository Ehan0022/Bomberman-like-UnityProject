using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlane : MonoBehaviour
{   
    [SerializeField] private bool bombPresent = false;
    private Bomb bombOnTop;

    public void SetBombOnTop(Bomb bomb)
    {
        bombOnTop = bomb;
        bombPresent = true;
    }

    public void ClearBomb()
    {
        bombOnTop = null;
        bombPresent = false;
    }

    public bool BombIsPresent()
    {
        return bombPresent;
    }
    
    public Transform GetTransform()
    {
        return this.transform;
    }
}
