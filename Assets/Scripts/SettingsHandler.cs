using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{

    [SerializeField] private gameSettings settings = null;

    void Start()
    {
        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<gameSettings>();
    }

    public void ToggleSound()
    {
        bool musicState = settings.GetIsMusicPlaying();

        musicState = !musicState;

        settings.SetIsMusicPlaying(musicState);

    }

}
