using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class PositionLight : MonoBehaviour
{
    private bool isEditing;

    AnchorHologram worldAnchor;

    Vector3 lightPosition;

    GameObject adjustmentPlane;
    Object adjPlanePrefab;

    public enum EditState { Idle, Placing, Adjusting };
    public EditState currentState;

    public bool anchorSet;
    public Vector3 anchor;
    public Vector3 anchorNormal;

    private bool planeIsHorizontal;

    // object for debugging on Unity
    GameObject testObj;

    void Awake()
    {
        lightPosition = this.transform.localPosition;
    }

    void Start()
    {
        // MOCK
        //testObj = GameObject.Find("Hue color lamp 2");
        //testObj.transform.position = new Vector3(0, 1, 5);

        currentState = EditState.Idle;

        adjPlanePrefab = Resources.Load("Prefabs/AdjustmentPlane");
        Vector3 prefabPos = new Vector3(transform.position.x, 0, transform.position.z);
        adjustmentPlane = Instantiate(adjPlanePrefab, transform.position, transform.rotation) as GameObject;
        // MOCK if set to "true"
        adjustmentPlane.SetActive(false);

        worldAnchor = GetComponent<AnchorHologram>();

       // MOCK
       //planeIsHorizontal = false;
       // if (!planeIsHorizontal)
       // {
       //     //adjustmentPlane.transform.Rotate(new Vector3(0, 0, 90));
       //     adjustmentPlane.transform.localScale = new Vector3(
       //         adjustmentPlane.transform.localScale.y,
       //         adjustmentPlane.transform.localScale.x,
       //         0.001f);
       // }
    }

    public void getLog()
    {
        Debug.Log("current state " + currentState);
    }

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
                    worldAnchor.RemoveLightAnchor();
                    Debug.Log("CurrState is Placing");
                }
                if (currentState == EditState.Adjusting)
                {
                    var headPosition = Camera.main.transform.position;
                    var gazeDirection = Camera.main.transform.forward;

                    //int layerMask = 1 << 8;

                    RaycastHit hitInfo;
                    if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                        30.0f, SpatialMappingManager.Instance.LayerMask))
                    {
                        anchor = hitInfo.point;
                        anchorNormal = hitInfo.normal;

                        Debug.Log("here is the anchorNormal " + anchorNormal);

                        adjustmentPlane.transform.position = hitInfo.point;
                        planeIsHorizontal = isPlaneHorizontal(anchorNormal);
                        Debug.Log("plane is horizontal? " + planeIsHorizontal);
                        if (!planeIsHorizontal)
                        {
                            adjustmentPlane.transform.localScale = new Vector3(
                            adjustmentPlane.transform.localScale.y,
                            adjustmentPlane.transform.localScale.x,
                            0.001f);
                        }
                        anchorSet = true;
                        // Rotate this object to face the user.
                        //Quaternion toQuat = Camera.main.transform.localRotation;
                        //toQuat.x = 0;
                        //toQuat.z = 0;
                        //this.transform.rotation = toQuat;
                    }
                    Debug.Log("CurrState is Adjusting");
                    adjustmentPlane.SetActive(true);
                }
                if (currentState == EditState.Idle)
                {
                    anchorSet = false;
                    adjustmentPlane.SetActive(false);
                    worldAnchor.SaveLightLocation();
                    Debug.Log("CurrState is Idle");
                }
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
                if (planeIsHorizontal)
                {
                    this.transform.position = new Vector3(anchor.x, hitInfo.point.y, anchor.z);
                }
                else
                {
                    this.transform.position = new Vector3(hitInfo.point.x, anchor.y, anchor.z);
                }

                // Rotate this object to face the user.
                //Quaternion toQuat = Camera.main.transform.localRotation;
                //toQuat.x = 0;
                //toQuat.z = 0;
                //this.transform.rotation = toQuat;
            }

            // locks the adjustmentPlane to the correct axis based on the hit point normal
            var lookDir = headPosition - transform.position;
            if (planeIsHorizontal)
            {
                lookDir.y = 0;
            }
            else
            {
                lookDir.x = 0;
            }
            adjustmentPlane.transform.rotation = Quaternion.LookRotation(lookDir);

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
                isPlaneHorizontal(hitInfo.normal);
                this.transform.position = hitInfo.point;

                // Rotate this object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }
    }

    bool isPlaneHorizontal(Vector3 normal)
    {
        return Mathf.Abs(normal.y) > 0.65f;
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

    /// <summary>
    /// Functions for testing and debugging functions in Unity and HoloLens
    /// </summary>
    public void MockSelect()
    {
        OnSelect();
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
