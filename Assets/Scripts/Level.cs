using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //-- Parameters --\\

    // Количество блоков, доступных для разрушения
    [SerializeField] int breakableBlocks = 0;   // Serialized for debugging

    //-- Cached references --\\

    // Поле для хранения ссылки на объекты типа SceneLoader
    SceneLoader sceneLoader = null;


    private void Start()
    {
        // Инициализация sceneLoader
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void CountBreakableBlocks()
    {
        // Увеличиваем значение счётчика количества блоков
        breakableBlocks++;
    }

    public void BlockDestroyed()
    {
        // Уменьшаем значение счётчика количества блоков
        breakableBlocks--;

        // Если блоков больше нет, то загружаем следующую сцену
        if (breakableBlocks <= 0)
        {
            sceneLoader.LoadNextScene();
        }
    }
}
