using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> portalList;
    public List<GameObject> activePortalList;
    public static int activePortals = 0;
    private int maxActivePortal = 6;


    void Start()
    {
        int randomIndex = Random.Range(0, portalList.Count);
        portalList[randomIndex].SetActive(true);
        activePortalList.Add(portalList[randomIndex]);
        activePortals++;
    }

    private void Update()
    {        
        ActivateRandomPortals();
    }



    private float maxTime = 25f;
    public float timer = 0f;
    private void ActivateRandomPortals()
    {
        timer += Time.deltaTime;
        if (timer > maxTime && activePortals <= maxActivePortal)
        {
            int randomIndex = Random.Range(0, portalList.Count);
            if(!portalList[randomIndex].activeSelf)
            {
                portalList[randomIndex].SetActive(true);
                activePortalList.Add(portalList[randomIndex]);
                activePortals++;
            }
            timer = 0f;
        }
        else if (timer > maxTime)
        {
            timer = 0f;
        }
    }


    public GameObject ReturnRandomPortal(GameObject callerPortal)
    {
        int randomIndex = Random.Range(0, activePortalList.Count);
        while(activePortalList[randomIndex] == callerPortal)
        {
            randomIndex = Random.Range(0, activePortalList.Count);
        }
        return activePortalList[randomIndex];
    }

    public void RemoveFromActivePortalList(GameObject portal)
    {
        activePortalList.Remove(portal);
    }
}
