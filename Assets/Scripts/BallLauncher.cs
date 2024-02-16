using UnityEngine;
using UnityEngine.EventSystems;

public class BallLauncher : MonoBehaviour {

    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private float _force;
    private static bool ready = true;

    void Update() {
        if (Ball.instance != null && Ball.instance.transform.position.y < -10)
            RecallBall();

        if (HasBeenThrown())
            return;

#if UNITY_EDITOR
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
#else
        if (!EventSystem.current.IsPointerOverGameObject(0) && Input.touchCount > 0)
#endif
        {
#if UNITY_EDITOR
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            var touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Input.touchCount <= 0 )
                return;
            if (touch.phase == UnityEngine.TouchPhase.Began)
#endif
            {
                var pos = ray.origin;
                var forw = ray.direction;
                GameObject ball;

                if (Ball.instance == null)
                    ball = Instantiate(_prefab, pos + (forw * 0.1f), Quaternion.identity);
                else {
                    ball = Ball.instance.gameObject;
                    ball.transform.position = pos + (forw * 0.1f);
                }

                if (ball.TryGetComponent(out Rigidbody rb)) {
                    rb.AddForce(forw * _force);
                    ready = false;
                    UIManager.instance.UpdateBallIcon();
                }
            }
        }
    }

    public static void RecallBall() {
        if (Ball.instance != null)
            Destroy(Ball.instance.gameObject);
        ready = true;
        UIManager.instance.UpdateBallIcon();
    }

    public static bool HasBeenThrown() {
        return !ready;
    }
}