using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Коррекция положения корневого объекта
public class sRootCorrection : MonoBehaviour
{
    // Класс, содержащий общие параметры и методы для работы с ними
    [SerializeField]
    private sCommonParameters _ComPars;

    // Объект для вывода сообщений в пространство
    [SerializeField]
    sTextMessage _WorldMessage;

    // Поле для вывода тестовых сообщений
    [SerializeField]
    Text _Text3;

    // Класс для переключения объектов (глиссад и т.д.)
    [SerializeField]
    sEnvironment _Env;

    // Скорость коррекции положения по горизонтали
    [SerializeField]
    float _HorSpeed = 1.0f;

    // Основная камера (установить в Inspector)
    [SerializeField]
    Camera _MainCam;

    // Дополнительная (правая) камера (установить в Inspector)
    [SerializeField]
    Camera _RightCam;

    // Меню настроек
    [SerializeField]
    GameObject _Menu;

    // Выбранная глиссада
    [SerializeField]
    Text _GlidesText;

    // Выбранный подход
    [SerializeField]
    Text _STARsText;

    // Выбранная точка IF
    [SerializeField]
    Text _IFsText;

    // Отображение маяков (путевых точек)
    [SerializeField]
    Text _BeaconsText;

    // Отображение баннеров путевых точек
    [SerializeField]
    Text _BannersText;

    // Поле зрения в градусах
    int _FOV;

    string _GlidesStr = " 1 2 3 4 5 6 − ";
    string _STARsStr = " 1 2 3 4 5 − ";
    string _IFsStr = " W E − ";
    string _BeaconsStr = " + − ";
    string _BannersStr = " + − ";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Коррекция корневого объекта
        if (_ComPars.MapCorrectionMode)
        {
            float x = -Input.GetAxis("Horizontal");
            float z = -Input.GetAxis("Vertical");
            float y = Input.GetAxis("Throttle");
            float yaw = Input.GetAxis("Twist");
            float f = -Input.GetAxis("Hat_Vert");

            if (x != 0 || z != 0 || y != 0)
            {
                //_Text3.text = "Сдвиг X = " + (_HorSpeed * x / 1000000) + " Сдвиг Y = " + +(_HorSpeed * y / 1000000) + " Сдвиг Z = " + +(_HorSpeed * z / 1000000);
                Vector3 pos = transform.localPosition;
                pos.x += _HorSpeed * x / 10;
                pos.z += _HorSpeed * z / 10;
                pos.y += _HorSpeed * y / 10;
                transform.localPosition = pos;
                //_WorldMessage.myFuncShowMessage("Новое положение = " + transform.position, 3);
            }
            if (yaw != 0)
            {
                //_Text3.text = "Поворот = " + (_HorSpeed * yaw / 1000000);
                Vector3 eu = transform.localEulerAngles;
                eu.y += _HorSpeed * yaw;
                transform.localEulerAngles = eu;
                //_WorldMessage.myFuncShowMessage("Новый поворот = " + transform.eulerAngles.y, 3);
            }
            if (f != 0)
            {
                // Поле зрения в градусах
                _FOV = (int)(_MainCam.fieldOfView + f);
                _MainCam.fieldOfView = _FOV;
                _RightCam.fieldOfView = _FOV;
                //_Text3.text = "FOV: Изменение f = " + f + ", Новый = " + _FOV;
               _WorldMessage.myFuncShowMessage("Новый FOV = " + _FOV, 3);
            }
        }

        // Нажата кнопка геймпада "B"
        if (Input.GetButtonDown("JoyButt1"))
        {
            // Переключение глиссад
            if (_ComPars.GlideSelection)
            {
                int newGlide = _Env.SwitchEnv("Glides");
                //_WorldMessage.myFuncShowMessage("Новая глиссада = " + newGlide, 3);
                string myString;
                switch (newGlide)
                {
                    case 0:
                        myString = "[" + _GlidesStr.Substring(1, 1) + "]" + _GlidesStr.Substring(3);
                        break;
                    case 1:
                        myString = _GlidesStr.Substring(0, 2) + "[" + _GlidesStr.Substring(3, 1) + "]" + _GlidesStr.Substring(5);
                        break;
                    case 2:
                        myString = _GlidesStr.Substring(0, 4) + "[" + _GlidesStr.Substring(5, 1) + "]" + _GlidesStr.Substring(7);
                        break;
                    case 3:
                        myString = _GlidesStr.Substring(0, 6) + "[" + _GlidesStr.Substring(7, 1) + "]" + _GlidesStr.Substring(9);
                        break;
                    case 4:
                        myString = _GlidesStr.Substring(0, 8) + "[" + _GlidesStr.Substring(9, 1) + "]" + _GlidesStr.Substring(11);
                        break;
                    case 5:
                        myString = _GlidesStr.Substring(0, 10) + "[" + _GlidesStr.Substring(11, 1) + "]" + _GlidesStr.Substring(13);
                        break;
                    case -1:
                        myString = _GlidesStr.Substring(0, 12) + "[" + _GlidesStr.Substring(13, 1) + "]" + _GlidesStr.Substring(15);
                        break;
                    default:
                        myString = "Ошибка";
                        break;
                }
                    _GlidesText.text = myString;
            }

            // Переключение подходов
            else if (_ComPars.STARselection)
            {
                int newSTAR = _Env.SwitchEnv("STARs");
                //_WorldMessage.myFuncShowMessage("Новый подход = " + newSTAR, 3);
                string myString;
                switch (newSTAR)
                {
                    case 0:
                        myString = "[" + _STARsStr.Substring(1, 1) + "]" + _STARsStr.Substring(3);
                        break;
                    case 1:
                        myString = _STARsStr.Substring(0, 2) + "[" + _STARsStr.Substring(3, 1) + "]" + _STARsStr.Substring(5);
                        break;
                    case 2:
                        myString = _STARsStr.Substring(0, 4) + "[" + _STARsStr.Substring(5, 1) + "]" + _STARsStr.Substring(7);
                        break;
                    case 3:
                        myString = _STARsStr.Substring(0, 6) + "[" + _STARsStr.Substring(7, 1) + "]" + _STARsStr.Substring(9);
                        break;
                    case 4:
                        myString = _STARsStr.Substring(0, 8) + "[" + _STARsStr.Substring(9, 1) + "]" + _STARsStr.Substring(11);
                        break;
                    case -1:
                        myString = _STARsStr.Substring(0, 10) + "[" + _STARsStr.Substring(11, 1) + "]" + _STARsStr.Substring(13);
                        break;
                    default:
                        myString = "Ошибка";
                        break;
                }
                _STARsText.text = myString;
            }

            // Переключение точек IF
            else if (_ComPars.IFselection)
            {
                int newIF = _Env.SwitchEnv("IFs");
                //_WorldMessage.myFuncShowMessage("Новая точка IF = " + newIF, 3);
                string myString;
                switch (newIF)
                {
                    case 0:
                        myString = "[" + _IFsStr.Substring(1, 1) + "]" + _IFsStr.Substring(3);
                        break;
                    case 1:
                        myString = _IFsStr.Substring(0, 2) + "[" + _IFsStr.Substring(3, 1) + "]" + _IFsStr.Substring(5);
                        break;
                    case -1:
                        myString = _IFsStr.Substring(0, 4) + "[" + _IFsStr.Substring(5, 1) + "]" + _IFsStr.Substring(7);
                        break;
                    default:
                        myString = "Ошибка";
                        break;
                }
                _IFsText.text = myString;
            }

            // Включение/выключение маяков
            else if (_ComPars.BeaconsSwitch)
            {
                int NewBeaconsIsActive = _Env.SwitchEnv("Beacons");
                //_WorldMessage.myFuncShowMessage("Новое состояние маяков = " + NewBeaconsIsActive, 2);
                string myString;
                switch (NewBeaconsIsActive)
                {
                    case 0:
                        myString = "[" + _BeaconsStr.Substring(1, 1) + "]" + _BeaconsStr.Substring(3);
                        break;
                    case -1:
                        myString = _BeaconsStr.Substring(0, 2) + "[" + _BeaconsStr.Substring(3, 1) + "]" + _BeaconsStr.Substring(5);
                        break;
                    default:
                        myString = "Ошибка";
                        break;
                }
                _BeaconsText.text = myString;
            }

            // Включение/выключение баннеров
            else if (_ComPars.BannersSwitch)
            {
                int NewBannersIsActive = _Env.SwitchEnv("Banners");
                //_WorldMessage.myFuncShowMessage("Новое состояние баннеров = " + NewBannersIsActive, 2);
                string myString;
                switch (NewBannersIsActive)
                {
                    case 0:
                        myString = "[" + _BannersStr.Substring(1, 1) + "]" + _BannersStr.Substring(3);
                        break;
                    case -1:
                        myString = _BannersStr.Substring(0, 2) + "[" + _BannersStr.Substring(3, 1) + "]" + _BannersStr.Substring(5);
                        break;
                    default:
                        myString = "Ошибка";
                        break;
                }
                _BannersText.text = myString;
            }
        }




        // Нажата кнопка "Y" - Вызвать меню настроек
        if (Input.GetButtonDown("JoyButt3"))
        {
            //_WorldMessage.myFuncShowMessage("Нажата кнопка Y", 2);
            if (_Menu.activeSelf)
            {
                _Menu.SetActive(false); // Спрятать меню
            }
            else
            {
                _Menu.transform.parent = Camera.main.transform; // временно перенести меню в дети камеры
                _Menu.transform.localPosition = Vector3.forward * 5; // поместить перед камерой
                _Menu.transform.localEulerAngles = Vector3.zero; // выровнять перед камерой
                _Menu.transform.parent = null; // вернуть меню в корень сцены
                _Menu.SetActive(true); // включить меню
            }
        }
    }






    //// "Превращаем" значение оси в -1/0/1
    //int _SnapAxes(float AxeValue)
    //{
    //    print("_AxeValue = " + _AxeValue);
    //    print("AxeValue = " + AxeValue);
    //    if (AxeValue > 0.95f)
    //    {
    //        if (AxeValue > _AxeValue)
    //        {
    //            _AxeValue = AxeValue;
    //            return 1;
    //        }
    //    }
    //    else if (AxeValue < -0.95f)
    //    {
    //        if (AxeValue < _AxeValue)
    //        {
    //            _AxeValue = AxeValue;
    //            return -1;
    //        }
    //    }
    //    else if (_AxeValue != 0.0f)
    //    {
    //        _AxeValue = 0.0f;
    //    }
    //    return 0;
    //}

}
