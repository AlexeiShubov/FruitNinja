using System.Collections;
using UnityEngine;

public class MenuInitialize : MonoBehaviour
{
    [SerializeField] private ScenLoader _scenLoader;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private PacManPlayButton _pacManPlayButton;
    [SerializeField] private Animator _fade;

    private void StartGame()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(Fade());
    }

    private void Update()
    {
        _cursorController.InputControllerUpdate();
    }

    private void OnEnable()
    {
        _pacManPlayButton.PlayButtonIsDead += StartGame;
    }

    private void OnDisable()
    {
        _pacManPlayButton.PlayButtonIsDead -= StartGame;
    }

    private IEnumerator Fade()
    {
        _fade.gameObject.SetActive(true);

        yield return new WaitForSeconds(_fade.GetCurrentAnimatorStateInfo(0).length);
        
        _scenLoader.LoadScene();
    }
}
