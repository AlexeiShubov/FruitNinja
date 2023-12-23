using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ScenLoader
{
    [SerializeField] private string _nameScenForLoad;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(_nameScenForLoad);
    }
}
