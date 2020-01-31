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

    // Скорость коррекции положения по горизонтали
    [SerializeField]
    float _HorSpeed = 0.0001f;

    // Основная камера (установить в Inspector)
    [SerializeField]
    Camera _MainCam;

    // Дополнительная (правая) камера (установить в Inspector)
    [SerializeField]
    Camera _RightCam;

    // Поле зрения в градусах
    int _FOV;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Если режим коррекции корневого объекта включен
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
                pos.x += _HorSpeed * x / 1000000;
                pos.z += _HorSpeed * z / 1000000;
                pos.y += _HorSpeed * z / 1000000;
                transform.localPosition = pos;
                //_WorldMessage.myFuncShowMessage("Новое положение = " + transform.position, 3);
            }
            if (yaw != 0)
            {
                //_Text3.text = "Поворот = " + (_HorSpeed * yaw / 1000000);
                Vector3 eu = transform.localEulerAngles;
                eu.y += _HorSpeed * yaw / 1000000;
                transform.localPosition = eu;
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
