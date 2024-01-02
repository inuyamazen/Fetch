using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private float _delay = 1f;
    [SerializeField]
    private GameObject _kibaPrefab;
    [SerializeField]
    private GameObject _loadingPanel;
    [SerializeField]
    private Button _resetButton;
    [SerializeField]
    private Button _recallButton;
    [SerializeField]
    private Button _tutorialButton;
    [SerializeField]
    private GameObject _tutorialPanel;
    [SerializeField]
    private GameObject _ballIcon;

    private GameObject _kiba;

    private void Start() {
        Restart();
    }

    private void Update() {
        _ballIcon.SetActive(!BallLauncher.HasBeenThrown());
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
        SetButtonsActive(false);
        _loadingPanel.SetActive(true);
        yield return StartCoroutine(DelayedSpawn());
        SetButtonsActive(true);
        _loadingPanel.SetActive(false);
    }

    public void SetButtonsActive(bool active) {
        _resetButton.interactable = active;
        _recallButton.interactable = active;
        _tutorialButton.interactable = active;
    }

    public void OpenTutorial() {
        SetButtonsActive(false);
        _tutorialPanel.SetActive(true);
    }

    public void CloseTutorial() {
        _tutorialPanel.SetActive(false);
        SetButtonsActive(true);
    }
}
