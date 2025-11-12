using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public Fader fader;

    public void OnStartGame()
    {
        Debug.Log("Start Game Click");
        fader.LoadScene("Game");
    }

    public void OnSettings()
    {
        fader.LoadScene("Settings");
    }
}
