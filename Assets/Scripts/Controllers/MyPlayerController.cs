using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour
{
    [Header("References")]
    [Space]
    public Rigidbody2D rb;
    public Animator myAnimator;
    public static MyPlayerController instance;

    [Header("Physics")]
    [Space]
    public float moveSpeed;
    public Vector3 gapToBorder;
    [HideInInspector]
    public string areaTransitionName; // Area we are exiting from
    public bool canMove = true; // Variable for dialogue system

    // Vectors
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    void Awake()
    {
        // Only one instance that should exist
        if(instance == null)
        {
            instance = this;
        }else
        {
            // Destroy this new player
            Destroy(gameObject);
        }
        // When we load a new scene dont destroy "gameObject"
        DontDestroyOnLoad(gameObject);
    }    
    void Update()
    {
        if(canMove)
        {
            // Moving our player
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }else
        {
            // If we cant move then velocity should be zero
            rb.velocity = Vector2.zero; 
        }
        // Setting up animator values to produce the animation
        myAnimator.SetFloat("moveX", rb.velocity.x);
        myAnimator.SetFloat("moveY", rb.velocity.y);
        // Setting up last direction value 
        if((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1) && canMove) 
        {
            myAnimator.SetFloat("lastX", Input.GetAxisRaw("Horizontal"));
            myAnimator.SetFloat("lastY", Input.GetAxisRaw("Vertical"));
        }
        // Limiting the camera inside the borders
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    public void SetBounds(Vector3 botLeftLim, Vector3 topRightLim)
    {
        bottomLeftLimit = botLeftLim + gapToBorder;
        topRightLimit = topRightLim - gapToBorder;
    }
}
