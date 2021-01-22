using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // Config params

    // Скорость игры (для замедления/ускорения)
    [Range(0.5f, 3f)] [SerializeField] float gameSpeed = 1f;

    // Количество очков за разрушение блока
    [SerializeField] int pointsPerBlockDestroyed = 50;    // Serialazied for debugging

    // Поле для хранения ссылки на объект Text в UI
    [SerializeField] Text scoreText = null;

    // Флаг: активен ли авто-режим
    [SerializeField] bool isAutoPlayEnabled = false;

    // State variables

    // Текущий счёт
    [SerializeField] int currentScore = 0;              // Serialazied for debugging


    private void Awake()
    {
        // Реализация паттерна Singleton для объекта GameStatus
        // Находим количество всех GameObjects типа GameStatus
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;

        // Если количество всех GameObjects типа GameStatus больше 1, тогда
        // удаляем текущий объект : иначе не уничтожаем текущий объект при загрузке сцены
        if (gameStatusCount > 1)
        {
            // Во время уничтожения объекта могут выполняться другие скрипты, 
            // что вызывает исключения и ошибки выполнения. Поэтому сначала 
            // нужно отключить объект, чтобы предотвратить исполнения других
            // скриптов (помним, что внутри объекта Game Status находится Canvas)
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else
        {
            // Запрещаем уничтожать объект при загрузке сцены
            DontDestroyOnLoad(gameObject);  
        }
    }

    private void Start()
    {
        // При старте уровня выводим текущий счёт в интерфейс
        scoreText.text = currentScore.ToString();
    }

    void Update()
    {
        // Управление скоростью игры через поле gameSpeed и Time.timeScale
        Time.timeScale = gameSpeed;
    }


    // Метод увеличения счёта
    public void AddToScore()
    {
        currentScore += pointsPerBlockDestroyed;
        scoreText.text = currentScore.ToString();
    }

    // Метод уничтожения объекта: 
    // нужен для обновления состояния игры при запуске начальной сцены
    public void ResetGame()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public bool IsAutoPlayEnabled()
    {
        return isAutoPlayEnabled;
    }
}
