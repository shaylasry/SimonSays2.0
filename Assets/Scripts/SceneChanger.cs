using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;

    private string nextScene;

    public void FadeOutTrigger(string scene)
    {
        nextScene = scene;
        animator.SetTrigger("FadeOut");
    }
    
    //use as unity event at end of fade out animation
    public void ChangeToScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
