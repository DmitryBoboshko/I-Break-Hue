using UnityEngine;

public class Ball : MonoBehaviour
{
    //-- Config params --\\

    // Поле для хранения ссылки на платформу
    [SerializeField] Paddle paddle = null;

    // Силы удара при запуске мяча
    [SerializeField] float xPush = 0f;
    [SerializeField] float yPush = 10f;

    // Массив звуков ударов мяча
    [SerializeField] AudioClip[] ballSounds = null;

    // Фактор случайности для направления отскока
    [SerializeField] float randomFactor = 0.2f;


    //-- State --\\

    // Расстояние между мячом и платформой
    Vector2 paddleToBallVector;

    // Начальное расстояние между мячом и платформой
    Vector2 initPaddleToBallVector = new Vector2(0f, 0.52f);    // Для настройки нужно добавить аттрибут SerializedField

    // Флаг: запущен ли мяч
    bool hasStarted = false;


    //-- Cached component references --\\

    // Поле для кэширования компонента AudioSource
    AudioSource myAudioSource;

    // Поле для кэширования компонента Rigidbody2D
    Rigidbody2D myRigidBody;


    void Start()
    {
        // Вычисляем расстояние между мячом и платформой
        paddleToBallVector = transform.position - paddle.transform.position;

        // Получаем компонент AudioSource объекта, т.е. мяча
        myAudioSource = GetComponent<AudioSource>();

        // Получаем компонент Rigidbody2D объекта, т.е. мяча
        myRigidBody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // Если мяч не запущени, то блокируем его и ждём нажатия левой кнопки мыши
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClicked();
        }
    }

    private void LaunchOnMouseClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Задаём ускорение мячу
            myRigidBody.velocity = new Vector2(xPush, yPush);

            // Устанавливаем флаг, что мяч запущен
            hasStarted = true;
        }
    }

    // Метод "приклеивания" мяча к ракетке
    private void LockBallToPaddle()
    {

        // Создаём переменную для хранения позиции платформы
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        // Задаём новое положение мяча
        transform.position = paddlePos + initPaddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (Random.Range(0f, randomFactor),
            Random.Range(0f, randomFactor));

        if (hasStarted)
        {
            // Выбираем аудио-эффект из массива случайным образом
            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            // GetComponent<AudioSource>().PlayOneShot(clip);

            // Воспроизводим аудио-эффект
            // PlayOneShot() отличается от Play() тем, что воспроизводимое аудио
            // не будет прервано другим. В обоих случаях воспроизведение можно остановить
            // методом Stop().
            myAudioSource.PlayOneShot(clip);

            myRigidBody.velocity += velocityTweak;
        }
    }
}
