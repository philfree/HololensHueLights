using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class PositionLight : MonoBehaviour
{

    private bool isEditing;

    private bool placing;
    private bool adjusting = true;
    private bool columnCollExist;

    //GameObject adjustmentPlane;
    //Object adjPlanePrefab;

    Object colliderObject;
    Object colliderPrefab;
    Vector3 lightPosition;

    GameObject adjustmentPlane;
    Object adjPlanePrefab;

    public enum EditState { Idle, Placing, Adjusting };
    public EditState currentState;

    GameObject testObj;

    public bool anchorSet;
    public Vector3 anchor;
    public Vector3 anchorNormal;

    void Awake()
    {
        lightPosition = this.transform.localPosition;
    }

    void Start()
    {
        // testObj = GameObject.Find("kitchen");
        // anchor = new Vector3(0, 2, 2);
        //SendMessageUpwards("SetAdjPlane", anchor);

        //currentState = EditState.Idle;
        //currentState++;
        //currentState++;
        //currentState = 0;
        //Debug.Log(currentState);

        adjPlanePrefab = Resources.Load("Prefabs/AdjustmentPlane");
        adjustmentPlane = Instantiate(adjPlanePrefab, transform.position, transform.rotation) as GameObject;
        adjustmentPlane.SetActive(false);
    }

    public void getLog()
    {
        Debug.Log("current state " + currentState);
    }

    // TODO remove public when deploying
    void OnSelect()
    {
        // only select light is app state is in edit mode
        if (true)
        // if (ActivityStateManager.Instance.IsEditMode)
        {
            // Called by GazeGestureManager when the user performs a tap gesture.
            if (SpatialMappingManager.Instance != null)
            {
                // If the user is in placing mode, display the spatial mapping mesh.
                if ((int)currentState < 2)
                {
                    currentState++;
                }
                else
                {
                    currentState = 0;
                }

                if (currentState == EditState.Placing)
                {
                    Debug.Log("CurrState is Placing");
                }
                if (currentState == EditState.Adjusting)
                {
                    adjustmentPlane.SetActive(true);
                    Debug.Log("CurrState is Adjusting");
                }
                if (currentState == EditState.Idle)
                {
                    anchorSet = false;
                    adjustmentPlane.SetActive(false);
                    Debug.Log("CurrState is Idle");
                }
                //if (currentState == EditState.Idle)
                //{

                //    currentState = EditState.Idle;
                //    var headPosition = Camera.main.transform.position;
                //    var gazeDirection = Camera.main.transform.forward;

                //    RaycastHit hitInfo;
                //    if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                //        30.0f))
                //    {
                //        Debug.Log("hitPoint set " + hitInfo.point);
                //        anchor = hitInfo.point;
                //        currentState = EditState.Placing;
                //        //SpatialMappingManager.Instance.DrawVisualMeshes = true;
                //        getCurrentState();
                //        SendMessageUpwards("SetAdjPlane", anchor);
                //    }

                //}
                //else if (currentState == EditState.Placing)
                //{
                //    currentState = EditState.Adjusting;
                //    getCurrentState();
                //    Debug.Log("state changed to " + currentState);
                //}
                //else if (currentState == EditState.Adjusting)
                //{
                //    //SpatialMappingManager.Instance.DrawVisualMeshes = false;
                //    currentState = EditState.Idle;
                //    getCurrentState();
                //    SendMessageUpwards("DeactivateAdjPlane");
                //}
            }
            else
            {
                Debug.Log("TapToPlace requires spatial mapping.  Try adding SpatialMapping prefab to project.");
            }
        }
    }

    // Update is called once per frame.
    void Update()
    {
        if (currentState == EditState.Adjusting)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            // raycast will only return hits from smart bulbs or adjustment planes
            int layerMask = 1 << 8;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, layerMask))
            {
                if (!anchorSet)
                {
                    anchor = hitInfo.point;
                    anchorNormal = hitInfo.normal;
                    ;// if (hitInfo.normal)
                    {

                    }
                    Debug.Log("here is the anchorNormal " + anchorNormal);

                    adjustmentPlane.transform.position = hitInfo.point;
                    anchorSet = true;
                }
                this.transform.position = new Vector3(anchor.x, hitInfo.point.y, anchor.z);

                // Rotate this object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }

        }
        // If the user is in placing mode,
        // update the placement to match the user's gaze.
        if (currentState == EditState.Placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMappingManager.Instance.LayerMask))
            {
                this.transform.position = hitInfo.point;

                // Rotate this object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }

        //if (currentState == EditState.Adjusting)
        //{
        //    RaycastHit hitInfo;
        //    //float targetDistance;

        //    Vector3 headPosition = Camera.main.transform.position;
        //    Vector3 gazeDirection = Camera.main.transform.forward;

        //    Physics.Raycast(headPosition, gazeDirection, out hitInfo,
        //        30.0f, SpatialMappingManager.Instance.LayerMask);

        //    float angleA = Vector3.Angle(anchor, gazeDirection);
        //    float angleB = Vector3.Angle(gazeDirection, new Vector3(0, 1, 0));

        //    float anchorDistanceFromCamera = Vector3.Distance(headPosition, anchor);
        //    //Debug.Log("anchor dis cam " + anchorDistanceFromCamera);
        //    // finds the distance to cast object along imaginary axis
        //    // targetDistance = distanceFromAnchor(angleA, angleB, anchorDistanceFromCamera);
        //    //Debug.Log("move on y is " + (targetDistance));
        //    this.transform.position = new Vector3(anchor.x, hitInfo.point.y, anchor.z);
        //    //if (Physics.Raycast(headPosition,  gazeDirection, out hitInfo,
        //    //    30.0f))
        //    //{
        //    //    this.gameObject.transform.position = new Vector3();
        //    //}

        //    //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
        //    //    30.0f, SpatialMappingManager.Instance.LayerMask))
        //    //{
        //    //    this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, hitInfo.point.y, gameObject.transform.position.z);
        //    //    Debug.Log("hitpoint y: " + hitInfo.point.y);
        //    //    //Debug.Log("coll name: " + hitInfo.collider.GetComponent<GameObject>().name);
        //    //}
        //}
    }

    float distanceFromAnchor(float angleA, float angleB, float cSide)
    {
        // sides of triangle
        float a;
        float b;
        float c = cSide;

        // convert and store angle as radians
        float A = Mathf.PI * angleA / 180;
        float C = Mathf.PI * angleB / 180;

        // find radian value of remaining angle
        float B = Mathf.PI * (180 - (A + C) * (180 / Mathf.PI)) / 180;

        // find length of raycast to target column/row

        a = (c * Mathf.Sin(A)) / (Mathf.Sin(C));

        //Debug.Log("this is the value of c " + c);
        Debug.Log("this is the value of a " + a);

        // find distance from anchor to target on y-axis
        b = (Mathf.Sin(B) * c) / (Mathf.Sin(C));
        Debug.Log("this is the value of b " + b * -1);

        return b * -1;
    }

    public void setAnchorTest()
    {
        anchor = Camera.main.transform.forward;
        Debug.Log("stored anchor " + anchor);
    }

    public void logValue()
    {
        var gazeDirection = Camera.main.transform.forward;
        var headPosition = Camera.main.transform.position;

        float angleB = Vector3.Angle(anchor, gazeDirection);

        RaycastHit hitInfo;
        Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f);
        Debug.Log("this is hit  c distance " + hitInfo.distance);

        Debug.Log("this is inverse of raycast " + gazeDirection * -1);

        float angleA = Vector3.Angle(gazeDirection * -1, new Vector3(0, -1, 0));

        distanceFromAnchor(angleA, angleB, hitInfo.distance);
    }

    void getCurrentState()
    {
        Debug.Log("here is the current state -" + currentState);
    }
}
