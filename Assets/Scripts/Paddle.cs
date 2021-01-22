using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    //-- Configuration fields --\\

    // Ширина вьюпорта
    // Камера в ортографическом режиме имеет параметр Size.
    // Параметр Size определяет размер вьюпорта, а именно половину его высоты.
    // Для экрана соотнешния 4:3 и Size = 6 высота вьюпорта равна 12 единицам, а ширина - 16.
    [SerializeField] float screenWidthInUnits = 16f;

    // Минимальная допустимая координата X платформы
    [SerializeField] float minX = -7f;

    // Максимальная допустимая координата X платформы
    [SerializeField] float maxX = 7f;


    //-- Cached references --\\

    Ball theBall = null;
    GameSession theGameSession = null;


    // Start is called before the first frame update
    void Start()
    {
        theBall = FindObjectOfType<Ball>();
        theGameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.mousePosition.x / Screen.width * screenWidthInUnits);

        // Вычисляем текущую позицию мыши:

        // Получаем координату x курсора, делим на ширину экрана, получаем координату x курсора в интервале [0;1]
        // Чтобы перейти к интервалу [0;16], то есть к мировым единицам (World Units), 
        // умножаем полученное значение на ширину экрана в мировых единицах.

        // Так как камера размещена на позиции (0,0,0), необходимо изменить интервал
        // координаты x курсора с [0;16] на [-8, 8]. Для этого вычитаем из позиции курсора
        // половину ширины экрана (вьюпорта).

        // float mousePosInUnits = (Input.mousePosition.x / Screen.width * screenWidthInUnits) - 8;

        // Переменная типа Vector2 для хранения нового положения плафтормы.
        // Для ограничения выходы платформы за границы экрана используем метод Mathf.Clamp
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);

        //paddlePos.x = Mathf.Clamp(mousePosInUnits, minX, maxX);

        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);

        // Устанавливаем новую позицию ракетки
        transform.position = paddlePos;
    }

    // Метод получения координаты x в зависимости от режима (авто/ручной)
    private float GetXPos()
    {
        float xPos;
        if (theGameSession.IsAutoPlayEnabled())
        {
            // Получаем позицию мяча
            xPos = theBall.transform.position.x;
            return xPos;
        } else
        {
            // Получаем позицию мыши (обоснование выше)
            xPos = (Input.mousePosition.x / Screen.width * screenWidthInUnits) - 8;
            return xPos;
        }
    }
}
