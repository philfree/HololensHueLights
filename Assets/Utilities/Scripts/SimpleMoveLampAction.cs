using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class SimpleMoveLampAction : MonoBehaviour {


    /// <summary>
    /// Indicates if the object is in the process of being placed.
    /// </summary>
    public bool IsPlacing { get; private set; }

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    // The most recent distance to the surface.  This is used to 
    // locate the object when the user's gaze does not intersect
    // with the Spatial Mapping mesh.
    private float lastDistance = 2.0f;

    // The distance away from the target surface that the object should hover prior while being placed.
    private float hoverDistance = 0.3f;

    // Threshold (the closer to 0, the stricter the standard) used to determine if a surface is flat.
    //private float distanceThreshold = 0.02f;

    // Threshold (the closer to 1, the stricter the standard) used to determine if a surface is vertical.
    //private float upNormalThreshold = 0.9f;

    // Maximum distance, from the object, that placement is allowed.
    // This is used when raycasting to see if the object is near a placeable surface.
    //private float maximumPlacementDistance = 5.0f;

    // Speed (1.0 being fastest) at which the object settles to the surface upon placement.
    private float placementVelocity = 0.06f;

    // Indicates whether or not this script manages the object's box collider.
    //private bool managingBoxCollider = false;

    // The box collider used to determine of the object will fit in the desired location.
    // It is also used to size the bounding cube.
    private BoxCollider boxCollider = null;


    // Visible asset used to show the dimensions of the object. This asset is sized
    // using the box collider's bounds.
    //private GameObject boundsAsset = null;

    // Visible asset used to show the where the object is attempting to be placed.
    // This asset is sized using the box collider's bounds.
    //private GameObject shadowAsset = null;

    // The location at which the object will be placed.
    private Vector3 targetPosition;

    private Rigidbody rigidBody = null;

    void Awake ()
    {
        targetPosition = gameObject.transform.position;

        // Get the rigid body
        rigidBody = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }


    public void OnSelect()
    {
        /* TODO: 4.a CODE ALONG 4.a */

        if (!IsPlacing)
        {
            OnPlacementStart();
        }
        else
        {
            OnPlacementStop();
        }
    }

    /// <summary>
    /// Put the object into placement mode.
    /// </summary>
    public void OnPlacementStart()
    {

        if (rigidBody != null)
        {
            rigidBody.useGravity = false;
        }
        boxCollider.enabled = false;

        // Tell the gesture manager that it is to assume
        // all input is to be given to this object.
        GestureManager.Instance.OverrideFocusedObject = gameObject;

        // Enter placement mode.
        IsPlacing = true;
    }

    /// <summary>
    /// Take the object out of placement mode.
    /// </summary>
    /// <remarks>
    /// This method will leave the object in placement mode if called while
    /// the object is in an invalid location.  To determine whether or not
    /// the object has been placed, check the value of the IsPlacing property.
    /// 
    /// Philippe: Commented everything, my hope is that the object will just fall in the plate with physics
    /// 
    /// </remarks>
    public void OnPlacementStop()
    {
        // ValidatePlacement requires a normal as an out parameter.
        Vector3 position;
        Vector3 surfaceNormal;

        if (rigidBody != null)
        {
            rigidBody.useGravity = true;
        }

        Vector3 objectCenter = GetColliderCenter();
        // Cast a ray from the center of the box collider face to the surface.
        RaycastHit centerHit;


        Physics.Raycast(Camera.main.transform.position,
                        Camera.main.transform.forward,
                        out centerHit,
                        20f,
                        RaycastLayerMask);
/**
        Physics.Raycast(objectCenter,
                             -(Vector3.up),
                             out centerHit,
                             20f,
                             RaycastLayerMask);
**/

        //gameObject.transform.position = centerHit.point;

        // We have found a surface.  Set position and surfaceNormal.
        //position = centerHit.point;
        surfaceNormal = centerHit.normal;

        // The object is allowed to be placed.
        // We are placing at a small buffer away from the surface.
        targetPosition = centerHit.point + (0.17f * surfaceNormal);
        
            //+ (0.01f * surfaceNormal);

        // Tell the gesture manager that it is to resume
        // its normal behavior.
        GestureManager.Instance.OverrideFocusedObject = null;
        boxCollider.enabled = true;

        // Exit placement mode.
        IsPlacing = false;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /* TODO: 4.a CODE ALONG 4.a */

        if (IsPlacing)
        {
            // Move the object.
            Move();
        }
        else
        {

            // Gracefully place the object on the target surface.
            // Note gravity did not worked as plan, I need more understanding of the physics ( changing this to let it fall.)

            float dist = (gameObject.transform.position - targetPosition).magnitude;
            if (dist > 0)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, placementVelocity / dist);
            }

        }
    }


    /// <summary>
    /// Positions the object along the surface toward which the user is gazing.
    /// </summary>
    /// <remarks>
    /// If the user's gaze does not intersect with a surface, the object
    /// will remain at the most recently calculated distance.
    /// </remarks>
    private void Move()
    {
        Vector3 moveTo = gameObject.transform.position;
        //Vector3 surfaceNormal = Vector3.zero;
        RaycastHit hitInfo;

        bool hit = Physics.Raycast(Camera.main.transform.position,
                                Camera.main.transform.forward,
                                out hitInfo,
                                20f,
                                RaycastLayerMask);

        if (hit)
        {
            float offsetDistance = hoverDistance;

            // Place the object a small distance away from the surface while keeping 
            // the object from going behind the user.
            if (hitInfo.distance <= hoverDistance)
            {
                offsetDistance = 0f;
            }

            moveTo = hitInfo.point + (offsetDistance * hitInfo.normal);
           // moveTo = hitInfo.point;
           

            lastDistance = hitInfo.distance;
            //surfaceNormal = hitInfo.normal;
        }
        else
        {
            // The raycast failed to hit a surface.  In this case, keep the object at the distance of the last
            // intersected surface.
            if (lastDistance < 1.0f) { lastDistance = 1.0f;  }
            moveTo = Camera.main.transform.position + (Camera.main.transform.forward * lastDistance);
        }

        // Follow the user's gaze.
        float dist = Mathf.Abs((gameObject.transform.position - moveTo).magnitude);
         gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, moveTo, placementVelocity / dist);
        
        // Orient the object.
        // We are using the return value from Physics.Raycast to instruct
        // the OrientObject function to align to the vertical surface if appropriate.

        //OrientObject(hit, surfaceNormal);
    }

    private Vector3 GetColliderCenter()
    {
        // Get the collider extents.  
        // The size values are twice the extents.
        Vector3 extents = boxCollider.size / 2;

        // Calculate the min and max values for each coordinate.
        float minY = boxCollider.center.y - extents.y;
        float maxZ = boxCollider.center.z + extents.z;

        Vector3 center;


       // if (PlacementSurface == PlacementSurfaces.Horizontal)
      //  {
            // Placing on horizontal surfaces.
            center = new Vector3(boxCollider.center.x, minY, boxCollider.center.z);
           // Translate to the current game object potion.
            center = gameObject.transform.TransformVector(center) + gameObject.transform.position;
        // }
        // else
        // {
        // Placing on vertical surfaces.
        /**
        center = new Vector3(boxCollider.center.x, boxCollider.center.y, maxZ);

    }**/

        return center;
    }

}
