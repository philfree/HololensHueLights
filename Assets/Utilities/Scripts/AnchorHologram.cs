using UnityEngine;
using System.Collections;

using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class AnchorHologram : MonoBehaviour {

    WorldAnchorStore anchorStore;
    WorldAnchor worldAnchor;

    bool savedAnchor;
    string anchorName;

    bool Placing = false;

    void Start() {
        // waits till data returns from store before trying to use data
        WorldAnchorStore.GetAsync(StoreLoaded);
    }

    void Update() {

    }

    void StoreLoaded(WorldAnchorStore store)
    {
        anchorStore = store;
        // key to be saved in store
        anchorName = gameObject.name;

        // method sets the position of gameObjects with script attached and record in anchor store
        LoadLightLocation();

        // lists all keys recorded in anchor store
        //getExistingAnchors();
    }
    public void SaveLightLocation()
    {
        if (!worldAnchor)
        {
            // pins hologram in place. Stores location for saving to anchor store
            worldAnchor = gameObject.AddComponent<WorldAnchor>();
        }

        // ** attempting to save a key that already exists will fail, not overwrite! **
        // if key id exists in anchor store, delete first, then add new reference
        string[] ids = this.anchorStore.GetAllIds();
        foreach (string id in ids)
        {
            if (id == anchorName)
            {
                Debug.Log("anchor key already exists...deleting " + id);
                anchorStore.Delete(id);
            }
        }
        savedAnchor = anchorStore.Save(anchorName, worldAnchor);
    }

    // WorldAnchor component needs to be removed prior to moving gameObject or it will be pinned in place
    public void RemoveLightAnchor()
    {
        if (gameObject.GetComponent<WorldAnchor>())
        {
            Debug.Log("removing world anchor on " + anchorName);
            DestroyImmediate(gameObject.GetComponent<WorldAnchor>());
        }
    }

    public void LoadLightLocation()
    {
        worldAnchor = anchorStore.Load(anchorName, gameObject);

        if (!worldAnchor)
        {
            Debug.Log("No Saved data to load");
        }
        else
        {
            Debug.Log("here is testAnchor " + worldAnchor);
        }
    }

    void getExistingAnchors()
    {
        string[] ids = this.anchorStore.GetAllIds();
        foreach (string id in ids)
        {
            Debug.Log("here is an anchor id: " + id);
        }
    }

    //private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    //{
    //    if (located)
    //    {
    //        bool saved = anchorStore.Save(anchorName, self);
    //        self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
    //    }
    //}
}
