using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    private BombPlane currentBombPlane;
    private float maxTimer = 2f;
    private float timer = 0f;
    public static int range = 1;
    //bomba menzilinde 4.5f default, +3 bir sonraki bloðu kapsar.

    [SerializeField] private LayerMask boxLayerMask;
    [SerializeField] private GameObject[] perks;
    [SerializeField] private GameObject explosion;
    public Player player;

    public bool damagedPlayer = false;
  
    public enum Direction
    {
        Forward,
        Backward,
        Right,
        Left
    }


    public void SetCurrentBombPlane(BombPlane bombPlane)
    {
        currentBombPlane = bombPlane;
    }

    public void ClearBombPlane()
    {
        currentBombPlane.ClearBomb();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTimer)
        {
            HandleExplosionLogic(Direction.Forward);
            HandleExplosionLogic(Direction.Backward);
            HandleExplosionLogic(Direction.Right);
            HandleExplosionLogic(Direction.Left);

            //SpawnExplosionEffects();
            Destroy(gameObject);
            player.DecreaseActiveBombCount();
            ClearBombPlane();           
        }    
    }

    


    private void HandleExplosionLogic(Direction direction)
    {
        float bombsRange = 4.5f;
        RaycastHit raycastHit;
        float explosionRange = 3f;
        if (direction == Direction.Forward)
        {
            for (int i = 0; i < range; i++)
            {               
                Physics.Raycast(transform.position, transform.forward, out raycastHit, bombsRange);

                if(raycastHit.collider != null && raycastHit.collider.gameObject != null)
                {
                    //bomba bu yönde bir yüzeyle çarpýþtý
                    if (raycastHit.collider.gameObject.tag.Equals("Box"))
                    {
                        //eðer kutuysa yok et.
                        InstantiatePerkAtRandom(raycastHit);
                        Destroy(raycastHit.collider.gameObject);
                        return;
                    }
                }
                else
                {
                    //raycast hiçbirþeyle çarpýþmadý, explosion effect spawn edilecek.
                    Vector3 forward = transform.position + Vector3.forward * explosionRange + Vector3.up * 2.21f;
                    GameObject explosionX = Instantiate(explosion, forward, Quaternion.identity);
                    ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                    explosionSelf.SetParentBomb(this);
                }

                bombsRange += 3f;
                explosionRange += 3f;
            }
        }

        if (direction == Direction.Backward)
        {
            for (int i = 0; i < range; i++)
            {
                Physics.Raycast(transform.position, -transform.forward, out raycastHit, bombsRange);

                if (raycastHit.collider != null && raycastHit.collider.gameObject != null)
                {
                    //bomba bu yönde bir yüzeyle çarpýþtý
                    if (raycastHit.collider.gameObject.tag.Equals("Box"))
                    {
                        //eðer kutuysa yok et.
                        InstantiatePerkAtRandom(raycastHit);
                        Destroy(raycastHit.collider.gameObject);
                        return;
                    }
                }
                else
                {
                    //raycast hiçbirþeyle çarpýþmadý, explosion effect spawn edilecek.
                    Vector3 backward = transform.position - Vector3.forward * explosionRange + Vector3.up * 2.21f;
                    GameObject explosionX = Instantiate(explosion, backward, Quaternion.identity);
                    ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                    explosionSelf.SetParentBomb(this);


                }
                bombsRange += 3f;
                explosionRange += 3f;
            }
        }

        if (direction == Direction.Right)
        {
            for (int i = 0; i < range; i++)
            {
                Physics.Raycast(transform.position, transform.right, out raycastHit, bombsRange);

                if (raycastHit.collider != null && raycastHit.collider.gameObject != null)
                {
                    //bomba bu yönde bir yüzeyle çarpýþtý
                    if (raycastHit.collider.gameObject.tag.Equals("Box"))
                    {
                        //eðer kutuysa yok et.
                        InstantiatePerkAtRandom(raycastHit);
                        Destroy(raycastHit.collider.gameObject);
                        return;
                    }
                }
                else
                {
                    //raycast hiçbirþeyle çarpýþmadý, explosion effect spawn edilecek.
                    Vector3 right = transform.position + Vector3.right * explosionRange + Vector3.up * 2.21f;
                    GameObject explosionX = Instantiate(explosion, right, Quaternion.identity);
                    ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                    explosionSelf.SetParentBomb(this);
                }
                bombsRange += 3f;
                explosionRange += 3f;
            }
        }

        if (direction == Direction.Left)
        {
            for (int i = 0; i < range; i++)
            {
                Physics.Raycast(transform.position, -transform.right, out raycastHit, bombsRange);

                if (raycastHit.collider != null && raycastHit.collider.gameObject != null)
                {
                    //bomba bu yönde bir yüzeyle çarpýþtý
                    if (raycastHit.collider.gameObject.tag.Equals("Box"))
                    {
                        //eðer kutuysa yok et.
                        InstantiatePerkAtRandom(raycastHit);
                        Destroy(raycastHit.collider.gameObject);
                        return;
                    }
                }
                else
                {
                    //raycast hiçbirþeyle çarpýþmadý, explosion effect spawn edilecek.
                    Vector3 left = transform.position - Vector3.right * explosionRange + Vector3.up * 2.21f;
                    GameObject explosionX = Instantiate(explosion, left, Quaternion.identity);
                    ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                    explosionSelf.SetParentBomb(this);
                }
                bombsRange += 3f;
                explosionRange += 3f;
            }
        }



    }

    private void InstantiatePerkAtRandom(RaycastHit box)
    {
        int chance1 = UnityEngine.Random.Range(0,3);
        int chance2 = UnityEngine.Random.Range(0, perks.Length);
        if (0 == chance1)
        {
            Instantiate(perks[chance2], box.collider.transform.position, Quaternion.Euler(-90f, 0f, 180f));
        }
    }

   

}
