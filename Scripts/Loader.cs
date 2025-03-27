using UnityEngine.SceneManagement;

public static class Loader
{
    public static SceneType currentScene = SceneType.Menu;

    public static void LoadScene(SceneType sceneType)
    {
        currentScene = sceneType;
        SceneManager.LoadScene(SceneType.Loading.ToString());
    }
}

public enum SceneType
{
    Loading,
    Main,
    Menu,
}