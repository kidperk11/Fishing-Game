using UnityEngine;

public class ExitToDesktop : MonoBehaviour
{
    public void CloseGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
