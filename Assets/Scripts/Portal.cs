using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] PortalManager portalManager;
    private float lifetimeMax = 60f;
    private float timer = 0f;
    [SerializeField] private bool left;
    [SerializeField] private bool right;
    [SerializeField] private bool forward;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > lifetimeMax)
        {
            TerminateSelf();
        }
    }

    private void TerminateSelf()
    {
        PortalManager.activePortals--;
        portalManager.RemoveFromActivePortalList(gameObject);
        gameObject.SetActive(false);
    }
    
    public void TeleportPlayer(Player player)
    {
        if(PortalManager.activePortals > 1)
        {
            Vector3 newPosition = player.transform.position;
            GameObject newPortal = portalManager.ReturnRandomPortal(gameObject);
            Portal newPortalsScript = newPortal.GetComponent<Portal>();

            if (newPortalsScript.ReturnPortalsDirection().Equals("Right"))
            {
                newPosition = -Vector3.right * 2f + newPortal.transform.position -Vector3.up*1.603f;
            }
            if (newPortalsScript.ReturnPortalsDirection().Equals("Left"))
            {
                newPosition = Vector3.right * 2f + newPortal.transform.position - Vector3.up * 1.603f;
            }
            if (newPortalsScript.ReturnPortalsDirection().Equals("Forward"))
            {
                newPosition = -Vector3.forward * 2f + newPortal.transform.position - Vector3.up * 1.603f;
            }
            player.transform.position = newPosition;
            
        }      
    }

    public string ReturnPortalsDirection()
    {
        if(right)        
            return "Right";
        if (left)
            return "Left";
        if (forward)
            return "Forward";
        else
            return "error";
    }
}
