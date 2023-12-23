using System.Collections;
using UnityEngine;

public class MenuBack : MonoBehaviour
{
    [SerializeField] private ScenLoader _scenLoader;
    [SerializeField] private PacManPlayButton pacManPlayButton;
    [SerializeField] private Animator _fade;

    private void BackToMenu()
    {
        GameEvents.OnMenuSceneLoad.Invoke();

        StartCoroutine(Fade());
    }

    private void OnEnable()
    {
        pacManPlayButton.PlayButtonIsDead += BackToMenu;
    }

    private void OnDisable()
    {
        pacManPlayButton.PlayButtonIsDead -= BackToMenu;
    }
    
    private IEnumerator Fade()
    {
        _fade.gameObject.SetActive(true);

        yield return new WaitForSeconds(_fade.GetCurrentAnimatorStateInfo(0).length);
        
        _scenLoader.LoadScene();
    }
}
