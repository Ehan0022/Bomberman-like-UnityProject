using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroySelf : MonoBehaviour
{
    private Bomb parentBomb;
    private float timer = 0f;
    private float maxTime = 0.65f;
    

    private float explosionRange = 1.5f;

    
    RaycastHit forwardHit;
    RaycastHit backwardHit;
    RaycastHit rightHit;
    RaycastHit leftHit;
    RaycastHit upwardHit;
    

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > maxTime)
        {
            Destroy(gameObject);
        }
        
        Physics.Raycast(transform.position, transform.forward, out forwardHit, explosionRange);
        Physics.Raycast(transform.position, -transform.forward, out backwardHit, explosionRange);
        Physics.Raycast(transform.position, transform.right, out rightHit, explosionRange);
        Physics.Raycast(transform.position, -transform.right, out leftHit, explosionRange);
        Physics.Raycast(transform.position - transform.up * 2.21f, transform.up, out upwardHit, 4f);

        //Debug.DrawRay(transform.position, transform.forward * explosionRange, Color.green);
        //Debug.DrawRay(transform.position, -transform.forward * explosionRange, Color.green);
        //Debug.DrawRay(transform.position, transform.right * explosionRange, Color.green);
        //Debug.DrawRay(transform.position, -transform.right * explosionRange, Color.green);

        DamagePlayer(forwardHit);
        DamagePlayer(backwardHit);
        DamagePlayer(rightHit);
        DamagePlayer(leftHit);
        DamagePlayer(upwardHit);


    }

    private void DamagePlayer(RaycastHit hit)
    {
        if(hit.collider != null && hit.collider.gameObject.tag.Equals("Player") && !parentBomb.damagedPlayer)
        {
            Debug.Log("PLAYER VURULDU");
            parentBomb.damagedPlayer = true;
            Player player = hit.collider.gameObject.GetComponent<Player>();
            player.DecreaseHealth();
        }
    }

    public void SetParentBomb(Bomb bomb)
    {
        parentBomb = bomb;    
    }

}
