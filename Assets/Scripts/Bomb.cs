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
    public static int maxRange = 6;
    //bomba menzilinde 4.5f default, +3 bir sonraki bloðu kapsar.

    [SerializeField] private LayerMask boxLayerMask;
    [SerializeField] private GameObject[] perks;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject explosionAlternate;
    public Player player;

    public bool damagedPlayer = false;
    private bool isPerkBomb = false;

    private bool shouldMove = false;
    private Vector3 newPosition;

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

    public void SetIsPerkBomb(bool isPerk)
    {
        isPerkBomb = isPerk;
    }

    public void SetShouldMove(bool shouldMove_)
    {
        shouldMove = shouldMove_;
    }

    public void SetNewPosition(Vector3 newPos)
    {
        newPosition = newPos;
    }


    private void Update()
    {
        MoveBomb(shouldMove, newPosition);
        timer += Time.deltaTime;
        if (timer > maxTimer )
        {
            HandleExplosionLogic(Direction.Forward);
            HandleExplosionLogic(Direction.Backward);
            HandleExplosionLogic(Direction.Right);
            HandleExplosionLogic(Direction.Left);
            player.DecreaseActiveBombCount();
            ClearBombPlane();
            Destroy(gameObject);                            
        }
      
    }

   
    

    private void HandleExplosionLogic(Direction direction)
    {
     
        GameObject explosionX_ = Instantiate(explosion, transform.position + Vector3.up * 2.21f, Quaternion.identity);
        ExplosionDestroySelf explosionSelf_ = explosionX_.GetComponent<ExplosionDestroySelf>();
        explosionSelf_.SetParentBomb(this);


        float bombsRange = 4.5f;
        RaycastHit raycastHit;
        float explosionRange = 3f;
        if (direction == Direction.Forward)
        {
            for (int i = 0; i < range; i++)
            {
                Physics.Raycast(transform.position, transform.forward, out raycastHit, bombsRange);

                if (raycastHit.collider != null && raycastHit.collider.gameObject != null)
                {
                    //bomba bu yönde bir yüzeyle çarpýþtý
                    if (raycastHit.collider.gameObject.tag.Equals("Box"))
                    {
                        //eðer kutuysa yok et.
                        InstantiatePerkAtRandom(raycastHit);
                        Destroy(raycastHit.collider.gameObject);

                        Vector3 forward = transform.position + Vector3.forward * explosionRange + Vector3.up * 2.21f;
                        GameObject explosionX = Instantiate(explosion, forward, Quaternion.identity);
                        ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                        explosionSelf.SetParentBomb(this);
                        return;
                    }

                    ExploadPerk(raycastHit, "BombRange", 3f, Direction.Forward);
                    ExploadPerk(raycastHit, "ExtraBomb", 3f, Direction.Forward);
                    ExploadPerk(raycastHit, "ExtraHealth", 3f, Direction.Forward);

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

                        Vector3 backward = transform.position - Vector3.forward * explosionRange + Vector3.up * 2.21f;
                        GameObject explosionX = Instantiate(explosion, backward, Quaternion.identity);
                        ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                        explosionSelf.SetParentBomb(this);
                        return;
                    }
                    ExploadPerk(raycastHit, "BombRange", 3f, Direction.Backward);
                    ExploadPerk(raycastHit, "ExtraBomb", 3f, Direction.Backward);
                    ExploadPerk(raycastHit, "ExtraHealth", 3f, Direction.Backward);

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

                        Vector3 right = transform.position + Vector3.right * explosionRange + Vector3.up * 2.21f;
                        GameObject explosionX = Instantiate(explosion, right, Quaternion.identity);
                        ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                        explosionSelf.SetParentBomb(this);
                        return;
                    }
                    ExploadPerk(raycastHit, "BombRange", 3f, Direction.Right);
                    ExploadPerk(raycastHit, "ExtraBomb", 3f, Direction.Right);
                    ExploadPerk(raycastHit, "ExtraHealth", 3f, Direction.Right);


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

                        Vector3 left = transform.position - Vector3.right * explosionRange + Vector3.up * 2.21f;
                        GameObject explosionX = Instantiate(explosion, left, Quaternion.identity);
                        ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
                        explosionSelf.SetParentBomb(this);
                        return;
                    }
                    ExploadPerk(raycastHit, "BombRange", 3f, Direction.Left);
                    ExploadPerk(raycastHit, "ExtraBomb", 3f, Direction.Left);
                    ExploadPerk(raycastHit, "ExtraHealth", 3f, Direction.Left);

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
        int chance1 = UnityEngine.Random.Range(0,4);
        int chance2 = UnityEngine.Random.Range(0, perks.Length);
        if (0 == chance1)
        {
            Instantiate(perks[chance2], box.collider.transform.position, Quaternion.Euler(-90f, 0f, 180f));
        }
    }


    private void ExploadPerk(RaycastHit raycastHit, String str, float explosionRange, Direction direction)
    {

        if (raycastHit.collider.gameObject.tag.Equals(str) && direction == Direction.Forward)
        {          
            Destroy(raycastHit.collider.gameObject);
            Vector3 forward = transform.position + Vector3.forward * explosionRange + Vector3.up * 2.21f;     
            
            GameObject explosionX = Instantiate(explosion, forward, Quaternion.identity);
            ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
            explosionSelf.SetParentBomb(this);
        }

        if (raycastHit.collider.gameObject.tag.Equals(str) && direction == Direction.Backward)
        {        
            Destroy(raycastHit.collider.gameObject);

            Vector3 backward = transform.position - Vector3.forward * explosionRange + Vector3.up * 2.21f;
            GameObject explosionX;
            explosionX = Instantiate(explosion, backward, Quaternion.identity);

            ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
            explosionSelf.SetParentBomb(this);
        }

        if (raycastHit.collider.gameObject.tag.Equals(str) && direction == Direction.Right)
        {
            Destroy(raycastHit.collider.gameObject);
            Vector3 right = transform.position + Vector3.right * explosionRange + Vector3.up * 2.21f;

            GameObject explosionX;
            explosionX = Instantiate(explosion, right, Quaternion.identity);

            ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
            explosionSelf.SetParentBomb(this);
        }

        if (raycastHit.collider.gameObject.tag.Equals(str) && direction == Direction.Left)
        {
            Destroy(raycastHit.collider.gameObject);
            Vector3 left = transform.position - Vector3.right * explosionRange + Vector3.up * 2.21f;

            GameObject explosionX;

            explosionX = Instantiate(explosion, left, Quaternion.identity);

            ExplosionDestroySelf explosionSelf = explosionX.GetComponent<ExplosionDestroySelf>();
            explosionSelf.SetParentBomb(this);
        }         
    }


    private float moveSpeed = 50f;
    public void MoveBomb(bool move, Vector3 newPosition)
    {
        if (move)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
        }
    }
     
 }







  


 

   


