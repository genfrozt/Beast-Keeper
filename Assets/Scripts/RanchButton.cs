using UnityEngine;
using UnityEngine.SceneManagement;

public class RanchButton : MonoBehaviour
{
    public void GoToRanch()
    {
        SceneManager.LoadScene("RanchScene");  // Load the Ranch Scene
    }
}