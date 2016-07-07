using UnityEngine;
using System.Collections;

public class AdjustmentPlane : MonoBehaviour
{

    private Vector3 pos = new Vector3();
    private Vector3 scale = new Vector3();

    GameObject adjustmentPlane;
    Object adjPlanePrefab;

    void Start()
    {
        //adjPlanePrefab = Resources.Load("Prefabs/AdjustmentPlane");
        //adjustmentPlane = Instantiate(adjPlanePrefab, transform.position, transform.rotation) as GameObject;
        //adjustmentPlane.SetActive(false);
    }

    void SetAdjPlane(Vector3 spawnPos)
    {
        pos = spawnPos;
        Debug.Log("set adj plane has been set " + spawnPos);
        adjustmentPlane.transform.position = spawnPos;
        adjustmentPlane.SetActive(true);
    }

    void DeactivateAdjPlane()
    {
        adjustmentPlane.SetActive(false);
    }
}
