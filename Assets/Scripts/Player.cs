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
    [SerializeField] private LayerMask boxOrWallLayerMask;
    [SerializeField] private LayerMask bombsLayerMask;
    [SerializeField] private LayerMask portalsLayerMask;

    [SerializeField] private int health = 3;

    [SerializeField] private GameObject bomb;

    [SerializeField] Transform spawnPoint;
    

    private bool isMoving;

    private int activeBombCount = 0;
    private int bombLimit = 1;

    public enum Direction
    {
        Right,
        Left,
        Forward,
        Backward,
    }

    public Direction currentDirection;


    Vector3 lastMoveDir;
    private void Start()
    {
        //bu event her e'ye basýldýgýnda ateþlenir
        lastMoveDir = new Vector3(0, 0, 0);
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    //bomb push
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        PushBomb();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        DropBomb();
    }


   
    private void Update()
    {
        HandleMovement();        
        HandlePerkInteractions();
        HandleTransports();

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
     
        if (IsMoving())
        {
            lastMoveDir = moveVector;
        }

        //rotasyon
        /*
        float rotateSpeed = 150f;
        transform.forward = Vector3.Slerp(transform.forward, lastMoveDir, Time.deltaTime * rotateSpeed);
        */
        transform.forward =  lastMoveDir;

        Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f); // Hedef dönüþ 90 derece olarak ayarlandý
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            currentDirection = Direction.Right;
        }
        targetRotation = Quaternion.Euler(0f, -90f, 0f); // Hedef dönüþ 90 derece olarak ayarlandý
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            currentDirection = Direction.Left;
        }
        targetRotation = Quaternion.Euler(0f, 0f, 0f); // Hedef dönüþ 90 derece olarak ayarlandý
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            currentDirection = Direction.Forward;
        }
        targetRotation = Quaternion.Euler(0f, 180f, 0f); // Hedef dönüþ 90 derece olarak ayarlandý
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            currentDirection = Direction.Backward;
        }

        //Debug.Log(currentDirection);
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

            if(perk.collider.gameObject.tag.Equals("ExtraHealth"))
            {
                ExtraHealthPerkCollected(perk);
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
    Bomb bombX;
    private void DropBomb()
    {
        Vector3 start = transform.position;
        Vector3 end = Vector3.up * -3f;
        Physics.Raycast(start, end, out hit, bombPlaneLayerMask);
        curentBombPlane = hit.collider.GetComponent<BombPlane>();


        if (!curentBombPlane.BombIsPresent())
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


    RaycastHit portalInFront;
    Portal portalInFrontS;
    private bool canTeleportAgain = true;
    private float maxCooldown = 3f;
    private float timer = 0f;
    private void HandleTransports()
    {
        if(canTeleportAgain)
        {
            Vector3 start2 = transform.position;
            Vector3 end2 = lastMoveDir.normalized;
            Physics.Raycast(start2, end2, out portalInFront, 1f, portalsLayerMask);
            portalInFrontS = portalInFront.collider.gameObject.GetComponent<Portal>();

            if (portalInFrontS != null)
            {
                canTeleportAgain = false;
                portalInFrontS.TeleportPlayer(this);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        if(timer > maxCooldown)
        {
            canTeleportAgain = true;
            timer = 0;
        }

    }


    RaycastHit collidedObject;
    RaycastHit bombInFront;
    private void PushBomb()
    {
        Vector3 start = transform.position;
        Vector3 end = lastMoveDir.normalized;
        Physics.Raycast(start, end, out collidedObject, 39f, boxOrWallLayerMask);

        Vector3 start2 = transform.position;
        Vector3 end2 = lastMoveDir.normalized;
        Physics.Raycast(start2, end2, out bombInFront, 2f, bombsLayerMask);      
        Bomb bombInFrontS = bombInFront.collider.gameObject.GetComponent<Bomb>();
        
       // Debug.Log(bombInFront.collider.gameObject);
        
        if (collidedObject.collider != null)
        {
            //Debug.Log("Spawn denendi");
            GameObject boxOrWall = collidedObject.collider.gameObject;
            Vector3 start1 = boxOrWall.transform.position;
            Vector3 end1 = Vector3.up * -3f;
            RaycastHit hit;
            Physics.Raycast(start1, end1, out hit, bombPlaneLayerMask);
            if (hit.collider != null)
            {
                if(currentDirection == Direction.Left)
                {
                    Vector3 newBombPosition = hit.collider.gameObject.transform.position + new Vector3(3f, 0, 0);
                    //Instantiate(debugBomb, newBombPosition, Quaternion.identity);

                    if(bombInFrontS != null)
                    {
                        bombInFrontS.SetShouldMove(true);
                        bombInFrontS.SetNewPosition(newBombPosition);
                    }
                        
                }
                if (currentDirection == Direction.Right)
                {
                    Vector3 newBombPosition = hit.collider.gameObject.transform.position + new Vector3(-3f, 0, 0);
                    //Instantiate(debugBomb, newBombPosition, Quaternion.identity);

                    if (bombInFrontS != null)
                    {
                        bombInFrontS.SetShouldMove(true);
                        bombInFrontS.SetNewPosition(newBombPosition);
                    }
                }
                if (currentDirection == Direction.Backward)
                {
                    Vector3 newBombPosition = hit.collider.gameObject.transform.position + new Vector3(0, 0, 3f);
                    //Instantiate(debugBomb, newBombPosition, Quaternion.identity);

                    if (bombInFrontS != null)
                    {
                        bombInFrontS.SetShouldMove(true);
                        bombInFrontS.SetNewPosition(newBombPosition);
                    }
                }
                if (currentDirection == Direction.Forward)
                {
                    Vector3 newBombPosition = hit.collider.gameObject.transform.position + new Vector3(0, 0, -3f);
                    //Instantiate(debugBomb, newBombPosition, Quaternion.identity);

                    if (bombInFrontS != null)
                    {
                        bombInFrontS.SetShouldMove(true);
                        bombInFrontS.SetNewPosition(newBombPosition);
                    }
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
        if(Bomb.maxRange >= Bomb.range)
        {
            Bomb.range++;
        }      
        Destroy(perk.collider.gameObject);
    }

    private void ExtraHealthPerkCollected(RaycastHit perk)
    {
        IncreaseHealth();
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
