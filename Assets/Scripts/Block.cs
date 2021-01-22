using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //-- Config params --\\

    // Поле для хранения ссылки на аудио разрушения блока
    [SerializeField] AudioClip breakSound = null;

    // Поле для хранения ссылки на эффект частиц
    [SerializeField] GameObject blockSparklesVFX = null;

    // Поле для хранения максимального количества ударов, которые выдерживает блок
    // [SerializeField] int maxHits = 1;
    int maxHits;

    // Поле для хранения массива спрайтов разрушения блока
    [SerializeField] Sprite[] hitSprites = null;


    //-- Cached references --\\

    // Поле для хранения ссылки на объект типа Level (уровня)
    Level level;


    //-- State variables --\\

    // Поле состояния: сколько получено ударов на данный момент
    [SerializeField] int timesHit = 0;  // TODO only serialized for debug



    private void Start()
    {
        InitializeBlock();
    }

    private void InitializeBlock()
    {
        // Засчитываем блок в счётчик блоков
        CountSelf();

        // На основе количества спрайтов разрушения устанавливаем
        // максимальное количество ударов
        maxHits = hitSprites.Length + 1;
        // Debug.Log("maxHits = " + maxHits.ToString());
    }

    private void CountSelf()
    {
        // При старте находим GameObject типа Level
        level = FindObjectOfType<Level>();

        // Проверяем, является ли блок разрушаемым
        if (CompareTag("Breakable"))
        {
            // Используем его метод для подсчёта блоков:
            // данный скрипт прикреплён ко всем блокам на сцене, поэтому
            // при запуске сцены каждый вызовет этот метод и таким образом
            // подсчитается количество всех блоков сцены
            level.CountBreakableBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, является ли блок разрушаемым
        if (CompareTag("Breakable"))
        {
            // Обрабатываем удар
            HandleHit(collision);
        }

    }

    private void HandleHit(Collision2D collision)
    {
        // Блок получил удар мячом
        timesHit++;

        Debug.Log(timesHit);    // TODO Remove after debugging

        // Если количество полученных ударов равно или больше максимальному количеству ударов,
        // уничтожаем блок
        if (timesHit == maxHits)
        {
            // Уничтожаем блок при столкновении
            DestroyBlock(collision);
        }
        // Если количество ударов недостаточно для разрушения
        else
        {
            // Показываем следующий спрайт разрушения
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        // Индекс спрайта для показа
        int spriteIndex = timesHit - 1;
        // Debug.Log(hitSprites[spriteIndex]);

        // Проверяем, есть ли в массиве элемент с индексом spriteIndex
        if (hitSprites[spriteIndex] != null)
        {
            // Присваиваем новый спрайт с индексом spriteIndex
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            // Если в массиве нет элемента с индексом spriteIndex, выводим ошибку
            Debug.LogError("Block sprite is missing from array: " + gameObject.name);
        }
    }

    private void DestroyBlock(Collision2D collision)
    {
        // Воспроизводим звуковые эффекты
        PlayBlockDestroySFX();

        // Воспроизводим анимацию частиц в месте удара
        TriggerSparklesVFX(collision);

        // Добавляем очки за разрушение в счётчик
        FindObjectOfType<GameSession>().AddToScore();

        // Говорим уровню, что блок уничтожен (вычитаем 1 из количества блоков)
        level.BlockDestroyed();

        // Уничтожаем объект без задержки
        Destroy(gameObject, 0f);
    }

    private void PlayBlockDestroySFX()
    {
        // Воспроизводим аудио разрушения мяча.
        // Указываем ссылку на звук и позицию камеры в качестве позиции источника звука
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX(Collision2D collision)
    {
        // Создаём экзмепляр префаба эффекта частиц в месте удара, он сразу же воспроизводится
        GameObject sparkles = Instantiate(blockSparklesVFX, collision.transform.position, collision.transform.rotation);

        // Уничтожаем объект через 2 секунды
        Destroy(sparkles, 2f);
    }

}
