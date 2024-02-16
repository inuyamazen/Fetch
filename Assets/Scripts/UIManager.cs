using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
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
    [SerializeField]
    private TextMeshProUGUI _fetchedText;
    [SerializeField]
    private TextMeshProUGUI _longestText;

    private int _fetchedNumber = 0;
    private float _longestDistance = 0;
    private const string _fetchedString = "Fetched: ", _longestString = "Longest: ";

    void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void SetButtonsActive(bool active) {
        _resetButton.interactable = active;
        _recallButton.interactable = active;
        _tutorialButton.interactable = active;
    }

    public void OpenTutorial() {
        _tutorialPanel.SetActive(true);
        SetButtonsActive(false);
    }
    public void CloseTutorial() {
        _tutorialPanel.SetActive(false);
        SetButtonsActive(true);
    }
    public void OpenLoading() {
        _loadingPanel.SetActive(true);
        SetButtonsActive(false);
    }
    public void CloseLoading() {
        _loadingPanel.SetActive(false);
        SetButtonsActive(true);
    }

    public void UpdateBallIcon() {
        _ballIcon.SetActive(!BallLauncher.HasBeenThrown());
    }

    public void Fetched(float distance) {
        _fetchedNumber++;
        _fetchedText.text = _fetchedString + _fetchedNumber;
        if(distance > _longestDistance) {
            _longestDistance = distance;
            _longestText.text = _longestString + distance.ToString("0.00");
        }
    }
}
