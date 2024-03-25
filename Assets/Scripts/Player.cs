using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask bombPlaneLayerMask;
    [SerializeField] private LayerMask perkLayerMask;

    [SerializeField] private int health = 3;

    [SerializeField] private GameObject bomb;

    [SerializeField] Transform spawnPoint;
    

    private bool isMoving;

    private int activeBombCount = 0;
    private int bombLimit = 1;




    private void Start()
    {
        //bu event her e'ye basýldýgýnda ateþlenir
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        //gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        DropBomb();
    }

    private void Update()
    {
        HandleMovement();
        HandleBombCollison();
        HandlePerkInteractions();
    }

    
    private void HandleMovement()
    {
        //moveVector ayarlama
        Vector2 inputVector = new Vector2();
        inputVector = gameInput.GetMovementVector();

        Vector3 moveVector = new Vector3(0, 0, 0);
       
        moveVector.x = inputVector.x;
        if(inputVector.x == 0f)
            moveVector.z = inputVector.y;

        //rotasyon
        float rotateSpeed = 12f;
        transform.forward = Vector3.Slerp(transform.forward, moveVector, Time.deltaTime * rotateSpeed);

        //raycastle önde obje olma kontrolü
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        float moveDistance = Time.deltaTime * moveSpeed;

        bool canMove = !Physics.CapsuleCast(transform.position + Vector3.up, transform.position + Vector3.up * playerHeight, playerRadius, moveVector, moveDistance);


        //hareket

        //eðer güncel yönde hareket edebiliyosan hareket et
        if (canMove)
        {
            transform.position = transform.position + moveVector * Time.deltaTime * moveSpeed;
        }


        if (!canMove)
        {
            Vector3 xCheck = moveVector;
            xCheck.z = 0f;
            //xte hareket mümkünse et
            if (!Physics.CapsuleCast(transform.position + Vector3.up , transform.position + Vector3.up * playerHeight, playerRadius, xCheck, moveDistance))
            {
                transform.position = transform.position + xCheck * Time.deltaTime * moveSpeed;
            }

            Vector3 zCheck = moveVector;
            zCheck.x = 0;
            //zde hareket mümkünse et
            if (!Physics.CapsuleCast(transform.position + Vector3.up , transform.position + Vector3.up * playerHeight, playerRadius, zCheck, moveDistance))
            {
                transform.position = transform.position + zCheck * Time.deltaTime * moveSpeed;
            }
            
        }


    }

    private float perkRange = 1f;
    private void HandlePerkInteractions()
    {
        RaycastHit perk;
        Physics.Raycast(transform.position, transform.forward, out perk, perkRange, perkLayerMask);
        if(perk.collider != null)
        {
            if (perk.collider.gameObject.tag.Equals("ExtraBomb"))
            {
                ExtraBombPerkCollected(perk);
            }

            if (perk.collider.gameObject.tag.Equals("BombRange"))
            {
                ExtraRangePerkCollected(perk);
            }
        }       
    }


    public bool IsMoving()
    {
        Vector2 inputVector = new Vector2();
        inputVector = gameInput.GetMovementVector();

        Vector3 moveVector = new Vector3(0, 0, 0);
        moveVector.x = inputVector.x;
        moveVector.z = inputVector.y;

        //this bool is for IsMoving() method
        if (moveVector == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        return isMoving;
    }

    public Transform returnSpawnPoint()
    {
        return spawnPoint;
    }

    private BombPlane curentBombPlane;

    RaycastHit hit;
    private void HandleBombCollison()
    {                            
        Vector3 start = transform.position;

        Vector3 end = Vector3.up * -10f; 
        Physics.Raycast(start, end, out hit, bombPlaneLayerMask);        
        curentBombPlane = hit.collider.GetComponent<BombPlane>();

    }

    Bomb bombX;
    private void DropBomb()
    {
        if(!curentBombPlane.BombIsPresent())
        {
            if(activeBombCount < bombLimit)
            {
                Vector3 dropPosition = hit.transform.position;
                bombX = Instantiate(bomb, dropPosition, Quaternion.identity).GetComponent<Bomb>();
                if (bombX != null)
                {
                    bombX.player = this;
                    curentBombPlane.SetBombOnTop(bombX);
                    bombX.SetCurrentBombPlane(curentBombPlane);
                    activeBombCount++;
                }
            }
                                     
        }                  
    }

    public void DecreaseActiveBombCount()
    {
        activeBombCount--;
    }

    private void ExtraBombPerkCollected(RaycastHit perk)
    {
        bombLimit++;
        Destroy(perk.collider.gameObject);
    }

    private void ExtraRangePerkCollected(RaycastHit perk)
    {
        Bomb.range++;
        Destroy(perk.collider.gameObject);
    }

    public void IncreaseHealth()
    {
        health++;
    }

    public void DecreaseHealth()
    {
        health--;
    }
    
    


}
