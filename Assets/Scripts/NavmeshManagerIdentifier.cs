using Niantic.Lightship.AR.NavigationMesh;
using UnityEngine;

public class NavmeshManagerIdentifier : MonoBehaviour
{
    public static NavmeshManagerIdentifier instance { get; private set; }

    void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public LightshipNavMeshManager GetManager() {
        return GetComponent<LightshipNavMeshManager>();
    }
}
