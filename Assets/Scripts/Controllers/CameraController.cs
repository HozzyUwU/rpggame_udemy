using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [Header("References: ")]
    public Transform target;
    public Tilemap myTilemap;

    [Header("Music To Play In Scene")]
    public int musicToPlay;
    private bool isMusicStarted = false;

    // Vectors
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    // Floats
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        // Setting up the target
        target = MyPlayerController.instance.transform;
        // Setting up variables for limiting the camera inside borders
        halfHeight = Camera.main.orthographicSize; // Height
        halfWidth = halfHeight * Camera.main.aspect; // Width

        bottomLeftLimit = myTilemap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = myTilemap.localBounds.max - new Vector3(halfWidth, halfHeight, 0f);

        // Pass boeder varoables to the player instance
        MyPlayerController.instance.SetBounds(myTilemap.localBounds.min, myTilemap.localBounds.max);
    } 
    void LateUpdate()
    {
        // Moving the camera 
        if(target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
        // Limiting the camera inside the borders
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if(!isMusicStarted)
        {
            isMusicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }

}
