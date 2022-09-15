using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Game.Interface.Init();
        Game.Interface.Connect("127.0.0.1",8000);
        
        SceneManager.LoadScene("Login");
    }
}
