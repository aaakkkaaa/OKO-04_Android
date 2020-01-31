using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sTestInput : MonoBehaviour
{

    // Объект для вывода сообщений в пространство
    sTextMessage _WorldMessage;

    // Поле для вывода тестовых сообщений
    [SerializeField]
    Text _Text3;

    // Start is called before the first frame update
    void Start()
    {
        // Объект для вывода сообщений в пространство
        _WorldMessage = GameObject.Find("TextMessage").GetComponent<sTextMessage>();

    }

    // Update is called once per frame
    void Update()
    {
        // Тестовое ссобщение
        if (Input.GetKeyDown("9"))
        {
            print("Нажата цифра 9");
            //_WorldMessage.myFuncShowMessage("Нажата цифра 9", 3.0f);
            _Text3.text = "Нажата цифра 9";
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            print("Нажата кнопка 0");
            _Text3.text = "Нажата кнопка 0";
        }

        float x = 0f;
        x = Input.GetAxis("Horizontal");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Horizontal = " + x);
            _Text3.text = "Horizontal = " + x;
        }
        x = Input.GetAxis("Vertical");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Vertical = " + x);
            _Text3.text = "Vertical = " + x;
        }
        x = Input.GetAxis("Throttle");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Throttle = " + x);
            _Text3.text = "Throttle = " + x;
        }
        x = Input.GetAxis("Twist");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Twist = " + x);
            _Text3.text = "Twist = " + x;
        }
        x = Input.GetAxis("Hat_Vert");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Hat_Vert = " + x);
            _Text3.text = "Hat_Vert = " + x;
        }
        x = Input.GetAxis("Hat_Hor");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Hat_Hor = " + x);
            _Text3.text = "Hat_Hor = " + x;
        }
        x = Input.GetAxis("Trigger_R");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Trigger_R = " + x);
            _Text3.text = "Trigger_R = " + x;
        }
        x = Input.GetAxis("Trigger_L");
        if (Mathf.Abs(x) > 0.1f)
        {
            print("Trigger_L = " + x);
            _Text3.text = "Trigger_L = " + x;
        }


        for (int i = 1; i<10; i++)
        {
            string JoyButt = "JoyButt" + i;
            if (Input.GetButtonDown(JoyButt))
            {
                print("Нажата кнопка " + i);
                _Text3.text = "Нажата кнопка " + i;
            }
        }


        //if (Input.GetKeyDown(KeyCode.Escape)) // Клавиша "Esc" (в Android смартфоне - "Back"): Выйти из программы
        //{
        //    print("Нажата клавиша Escape");
        //    _Text3.text = "Нажата клавиша Escape";
        //    if (Input.GetKey("left shift") || Input.GetKey("right shift") || Input.GetAxis("Twist") > 0.5f) // еще нужно удерживать Shift или правый джойстик на Game Controller вправо
        //    {
        //        print("Выход");
        //        _Text3.text = _Text3.text + " - Выход";
        //        Application.Quit();
        //    }
        //}


    }
}
