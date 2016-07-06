using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class ActivityStateManager : Singleton<ActivityStateManager>
{
    private bool isEditMode;
    private bool isRevealMode;

    public bool IsEditMode
    {
        get { return isEditMode; }
    }

    public bool IsRevealMode
    {
        get { return isRevealMode; }
    }

    private void Awake()
    {
        isEditMode = false;
        isRevealMode = false;
    }

    // voice keyword > "edit room"
    public void ToggleEditMode()
    {
        isEditMode = !isEditMode;
    }
}
