using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombPlane currentBombPlane;
    private float maxTimer = 2f;
    private float timer = 0f;
    public static float bombRange = 4.5f;
    //bomba menzilinde 4.5f default, +3 bir sonraki bloðu kapsar.

    [SerializeField] private LayerMask boxLayerMask;


    public void SetCurrentBombPlane(BombPlane bombPlane)
    {
        currentBombPlane = bombPlane;
    }

    public void ClearBombPlane()
    {
        currentBombPlane.ClearBomb();
    }


    private RaycastHit forwardHit;
    private RaycastHit rightHit;
    private RaycastHit backwardHit;
    private RaycastHit leftHit;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTimer)
        {
            if (forwardHit.collider != null && forwardHit.collider.gameObject != null)
                Destroy(forwardHit.collider.gameObject);
            if (rightHit.collider != null && rightHit.collider.gameObject != null)
                Destroy(rightHit.collider.gameObject);
            if (backwardHit.collider != null && backwardHit.collider.gameObject != null)
                Destroy(backwardHit.collider.gameObject);
            if (leftHit.collider != null && leftHit.collider.gameObject != null)
                Destroy(leftHit.collider.gameObject);
            Destroy(gameObject);
            ClearBombPlane();           
        }

        Physics.Raycast(transform.position, transform.forward, out forwardHit, bombRange, boxLayerMask);
        Debug.DrawRay(transform.position, transform.forward * bombRange, Color.green);

        Physics.Raycast(transform.position, -transform.forward, out backwardHit, bombRange, boxLayerMask);
        Debug.DrawRay(transform.position, -transform.forward * bombRange, Color.green);

        Physics.Raycast(transform.position, transform.right, out rightHit, bombRange, boxLayerMask);
        Debug.DrawRay(transform.position, transform.right * bombRange, Color.green);

        Physics.Raycast(transform.position, -transform.right, out leftHit, bombRange, boxLayerMask);
        Debug.DrawRay(transform.position, -transform.right * bombRange, Color.green);
    }

}
