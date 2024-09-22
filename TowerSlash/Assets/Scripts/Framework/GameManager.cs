using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform Player { get; private set; }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
