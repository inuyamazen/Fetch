using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private float _delay = 1f;
    [SerializeField]
    private GameObject _kibaPrefab;

    private GameObject _kiba;
    private UIManager _uimanager;

    private void Start() {
        _uimanager = UIManager.instance;
        Restart();
    }

    private IEnumerator DelayedSpawn() {
        yield return new WaitForSeconds(_delay);
        RaycastHit hitInfo;
        while (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f))
            yield return new WaitForSeconds(0.1f);
        _kiba = Instantiate(_kibaPrefab, hitInfo.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
    }

    public void Restart() {
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame() {
        BallLauncher.RecallBall();
        if (_kiba != null)
            Destroy(_kiba);
        _uimanager.OpenLoading();
        yield return StartCoroutine(DelayedSpawn());
        _uimanager.CloseLoading();
    }

}
