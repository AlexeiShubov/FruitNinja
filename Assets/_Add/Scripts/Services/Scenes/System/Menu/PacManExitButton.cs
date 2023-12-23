using UnityEditor;
using UnityEngine;

public class PacManExitButton : PacManPlayButton
{
    protected override void ButtonIsDead()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
