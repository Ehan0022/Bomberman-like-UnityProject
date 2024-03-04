using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;


    [SerializeField] Transform spawnPoint;
    

    private bool isMoving;




    private void Start()
    {
        //bu event her e'ye bas�ld�g�nda ate�lenir
        //gameInput.OnInteractAction += GameInput_OnInteractAction;
        //gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }




    private void Update()
    {
        HandleMovement();      
    }

    
    private void HandleMovement()
    {
        //moveVector ayarlama
        Vector2 inputVector = new Vector2();
        inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveVector = new Vector3(0, 0, 0);
        moveVector.x = inputVector.x;
        moveVector.z = inputVector.y;

        //rotasyon
        float rotateSpeed = 12f;
        transform.forward = Vector3.Slerp(transform.forward, moveVector, Time.deltaTime * rotateSpeed);

        //raycastle �nde obje olma kontrol�
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        float moveDistance = Time.deltaTime * moveSpeed;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVector, moveDistance);


        //hareket

        //e�er g�ncel y�nde hareket edebiliyosan hareket et
        if (canMove)
        {
            transform.position = transform.position + moveVector * Time.deltaTime * moveSpeed;
        }


        if (!canMove)
        {
            Vector3 xCheck = moveVector;
            xCheck.z = 0f;
            //xte hareket m�mk�nse et
            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, xCheck, moveDistance))
            {
                transform.position = transform.position + xCheck * Time.deltaTime * moveSpeed;
            }

            Vector3 zCheck = moveVector;
            zCheck.x = 0;
            //zde hareket m�mk�nse et
            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, zCheck, moveDistance))
            {
                transform.position = transform.position + zCheck * Time.deltaTime * moveSpeed;
            }

        }
    }

    Vector3 lastMoveVector;
    
    private void HandleInteractions()
    {
        //moveVector ayarlama
        Vector2 inputVector = new Vector2();
        inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveVector = new Vector3(0, 0, 0);
        moveVector.x = inputVector.x;
        moveVector.z = inputVector.y;


        //son bakt�g�m�z yeri kaydetme
        if(moveVector!=Vector3.zero)
        {
            lastMoveVector = moveVector;
        }

        RaycastHit raycastHit;
        float maxDistance = 2f;
    }


    public bool IsMoving()
    {
        Vector2 inputVector = new Vector2();
        inputVector = gameInput.GetMovementVectorNormalized();

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

  
}