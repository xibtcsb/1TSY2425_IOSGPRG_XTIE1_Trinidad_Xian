using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DashButton : MonoBehaviour
{
    [SerializeField] private Button _dashButton;
    private GaugeBar _gaugeBar;
    private Player _player;

    private void Start()
    {
        InitializeReferences();
        _dashButton.gameObject.SetActive(false);
        _dashButton.onClick.AddListener(OnDashButtonClicked);
    }

    private void Update()
    {
        ToggleDashButton();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeReferences()
    {
        _gaugeBar = FindObjectOfType<GaugeBar>();
        _player = FindObjectOfType<Player>();
    }

    private void OnDashButtonClicked()
    {
        if (_gaugeBar.CanDash())
        {
            _gaugeBar.UseDash();
            _player.StartDash();
        }
    }

    private void ToggleDashButton()
    {
        _dashButton.gameObject.SetActive(_gaugeBar.CanDash());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_gaugeBar != null)
        {
            _dashButton.gameObject.SetActive(false);
        }
    }
}
