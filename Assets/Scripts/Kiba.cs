using Niantic.Lightship.AR.NavigationMesh;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kiba : MonoBehaviour {

    [SerializeField]
    private float _velocityRatio = 0.1f, _catchDistance = 0.3f, _sitDistance = 0.3f, _lookRate = 2f;
    [SerializeField]
    private LightshipNavMeshManager _navmeshManager;
    private LightshipNavMeshAgent _agent;
    private bool caught = false, waiting = false;
    [SerializeField]
    private GameObject heldBall;
    private Animator _animator;


    private void Start() {
        _agent = GetComponent<LightshipNavMeshAgent>();
        _navmeshManager = NavmeshManagerIdentifier.instance.GetManager();
        _animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        if (!BallLauncher.HasBeenThrown()) {
            caught = false;
            waiting = false;
            heldBall.SetActive(false);
            _agent.StopMoving();
            _animator.SetBool("isWalking", false);
            LookAtPlayer();
            return;
        }

        if (caught && !waiting) {
            NavigateBack();
            _animator.SetBool("isWalking", true);
        } else if (caught && waiting) {
            WaitForPickup();
            _animator.SetBool("isWalking", false);
            LookAtPlayer();
        } else {
            Fetch();
            _animator.SetBool("isWalking", true);
        }
    }

    private void Fetch() {
        float distance = CalculateFlatDistanceToGoal(Ball.instance.transform.position);
        if (distance > _catchDistance) {
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
        if (distance < _sitDistance) {
            _agent.StopMoving();
            waiting = true;
        }
    }

    private Vector3 CalculateGoal() {
        return Ball.instance.transform.position + Ball.instance.GetComponent<Rigidbody>().velocity * _velocityRatio;
    }

    private float CalculateFlatDistanceToGoal(Vector3 goal) {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(goal.x, goal.z));
    }

    private void LookAtPlayer() {
        Vector3 lookPosition = Camera.main.transform.position - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _lookRate);
    }
}
