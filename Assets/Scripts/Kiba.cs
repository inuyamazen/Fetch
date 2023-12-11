using Niantic.Lightship.AR.NavigationMesh;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kiba : MonoBehaviour {

    [SerializeField]
    private float velocityRatio = 1f, catchDistance = 0.2f, sitDistance = 0.3f;
    [SerializeField]
    private LightshipNavMeshManager _navmeshManager;
    private LightshipNavMeshAgent _agent;
    private bool caught = false, waiting = false;
    [SerializeField]
    private GameObject heldBall;

    private void Start() {
        _agent = GetComponent<LightshipNavMeshAgent>();
        _navmeshManager = NavmeshManagerIdentifier.instance.GetManager();
    }

    void LateUpdate() {
        if (!BallLauncher.HasBeenThrown()) {
            caught = false;
            waiting = false;
            heldBall.SetActive(false);
            _agent.StopMoving();
            return;
        }

        if (caught && !waiting) {
            NavigateBack();
        } else if (caught && waiting) {
            WaitForPickup();
        } else {
            Fetch();
        }
    }

    private void Fetch() {
        float distance = CalculateFlatDistanceToGoal(Ball.instance.transform.position);
        if (distance > catchDistance) {
            Vector3 goal = CalculateGoal();
            _agent.SetDestination(goal);
        } else {
            caught = true;
            Destroy(Ball.instance.gameObject);
            heldBall.SetActive(true);
        }
    }

    private void WaitForPickup() {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject() || !Input.GetMouseButtonDown(0))
            return;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 100.0f);
#else
            if (EventSystem.current.IsPointerOverGameObject(0) || Input.touchCount == 0)
                return;
            var touch = Input.GetTouch(0);
            Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hitInfo, 100.0f);
#endif
        if (hitInfo.transform.gameObject.Equals(gameObject)) {
            BallLauncher.RecallBall();
            caught = false;
            waiting = false;
            heldBall.SetActive(false);
        }
    }

    private void NavigateBack() {
        _agent.SetDestination(new Vector3(Camera.main.transform.position.x,
                       transform.position.y,
                       Camera.main.transform.position.z));
        float distance = CalculateFlatDistanceToGoal(Camera.main.transform.position);
        if (distance < sitDistance) {
            _agent.StopMoving();
            waiting = true;
            //change model
        }
    }

    private Vector3 CalculateGoal() {
        return Ball.instance.transform.position + Ball.instance.GetComponent<Rigidbody>().velocity * velocityRatio;
    }

    private float CalculateFlatDistanceToGoal(Vector3 goal) {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(goal.x, goal.z));
    }
}
