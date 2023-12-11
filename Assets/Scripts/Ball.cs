using UnityEngine;

public class Ball : MonoBehaviour {

    public static Ball instance { get; private set; }

    void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
}
