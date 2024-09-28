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
        _gaugeBar = FindObjectOfType<GaugeBar>();
        _player = FindObjectOfType<Player>();

        _dashButton.gameObject.SetActive(false);

        _dashButton.onClick.AddListener(() =>
        {
            if (_gaugeBar.CanDash())
            {
                _gaugeBar.UseDash();
                _player.StartDash();
            }
        });
    }

    private void Update()
    {
        if (_gaugeBar.CanDash())
        {
            _dashButton.gameObject.SetActive(true); 
        }
        else
        {
            _dashButton.gameObject.SetActive(false); 
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
        if (_gaugeBar != null)
        {
            _dashButton.gameObject.SetActive(false);
        }
    }
}
