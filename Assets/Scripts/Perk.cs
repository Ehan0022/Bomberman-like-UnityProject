using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] bool isExtraBomb;

    public void ExplodeIfExtraBomb()
    {
        if(isExtraBomb)
        {
            GameObject bombX = Instantiate(bomb, transform.position, Quaternion.identity);
            Bomb b = bombX.GetComponent<Bomb>();
            b.SetIsPerkBomb(true);
        }
            
    }
}
