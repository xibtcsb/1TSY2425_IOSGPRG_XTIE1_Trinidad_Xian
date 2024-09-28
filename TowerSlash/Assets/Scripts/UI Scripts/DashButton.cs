using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DashButton : MonoBehaviour
{
    [SerializeField] private Button _dashButton;
    private GaugeBar _gaugeBar;
    private Player _player;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _gaugeBar = FindObjectOfType<GaugeBar>();
        _player = FindObjectOfType<Player>();
        _canvasGroup = _dashButton.GetComponent<CanvasGroup>();

        _dashButton.interactable = false;

        _dashButton.onClick.AddListener(() =>
        {
            if (_gaugeBar.CanDash())
            {
                _gaugeBar.UseDash();
                _player.StartDash();
            }
        });

        DisableCanvasGroup();
    }

    private void Update()
    {
        if (_canvasGroup != null)
        {
            if (_gaugeBar.CanDash())
            {
                _canvasGroup.alpha = 0; 
                _dashButton.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _canvasGroup.alpha = 0; 
                _dashButton.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DisableCanvasGroup();
    }

    private void DisableCanvasGroup()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = false; 
            _canvasGroup.alpha = 0; 
        }
    }
}
