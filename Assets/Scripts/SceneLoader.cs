using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Метод загрузки следующей сцены
    public void LoadNextScene()
    {
        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Загружаем следующую сцену путём инкрементирования индекса текущей сцены на единицу
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Метод загрузки начальной сцены
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        
        // Уничтожаем объект GameStatus, чтобы обнулить прогресс 
        FindObjectOfType<GameSession>().ResetGame();
    }

    // Метод выхода из игры
    public void QuitGame()
    {
        Application.Quit();
    }
}
