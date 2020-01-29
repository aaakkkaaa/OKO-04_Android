
using System;
using UnityEngine;
using UnityEngine.UI;

// Положение Mortar в экранном режиме: X = 0, Y = 0.726, Z = -1.089. Наклон камеры X = 30
// В режиме VR: X = -0.5, Y = 0, Z = -1.089. Наклон камеры X = 0

public class sMortarMovement : MonoBehaviour
{
    // Класс, содержащий общие параметры и методы для работы с ними
    [SerializeField]
    private sCommonParameters _ComPars;

    Camera _referenceCamera;

    [SerializeField]
    Text mySceenMessage;

    Vector3 _oldMousePos;
    float myOldMouseX;

    // Параметры перелета
    
    // Положение в начале перелета
    Vector3 myStartPos;
    Vector3 myStarttEu;
    // Положение в конце перелета
    Vector3 myEndPos;
    Vector3 myEndEu;
    // Флаг перелета, блокирует управление
    bool myFlight = false;
    // Время начала перелета, сек
    float myStartTime;

    // Вспомогательный объект - точка проекции камеры на горизонтальную плоскость аватара
    Transform _CameraPlumb;


    void Awake()
    {
        _referenceCamera = Camera.main;

        if (_referenceCamera == null)
        {
            _referenceCamera = GetComponent<Camera>();
            if (_referenceCamera == null)
            {
                throw new System.Exception("You must have a reference camera assigned!");
            }
        }

        // Вспомогательный объект - точка проекции камеры на горизонтальную плоскость аватара
        _CameraPlumb = transform.Find("CameraPlumb");
    }

    void LateUpdate()
	{

        if (myFlight) // Только перелетаем в заданное положение
        {
            float myInterpolant = (Time.time - myStartTime) / _ComPars.MortarFlightTime;
            transform.localPosition = Vector3.Lerp(myStartPos, myEndPos, myInterpolant);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(myStarttEu), Quaternion.Euler(myEndEu), myInterpolant);
            if(myInterpolant > 1)
            {
                myFlight = false;
            }
        }
        else // Все остальное управление
        {
            // Команда на перелет домой
            if (Input.GetKeyDown("h"))
            {
                transform.parent = null; // Выйти в корень иерархии сцены
                myStartTime = Time.time;
                myStartPos = transform.position;
                myStarttEu = transform.eulerAngles;
                myEndPos = Vector3.Scale(_ComPars.MortarHomePos, _ComPars.WorldScale);
                myEndEu = _ComPars.MortarHomeEu;

                myFlight = true;
            }
            // Команда на перелет к башне
            else if (Input.GetKeyDown("t"))
            {
                transform.parent = null; // Выйти в корень иерархии сцены
                myStartTime = Time.time;
                myStartPos = transform.position;
                myStarttEu = transform.eulerAngles;
                myEndPos = Vector3.Scale(_ComPars.MortarTowerPos, _ComPars.WorldScale);
                myEndEu = _ComPars.MortarTowerEu;

                myFlight = true;
            }
            // Команда сесть на хвост
            else if (Input.GetKeyDown("p"))
            {
                if (!transform.parent) // Если находимся в корне иерархии сцены
                {

                    // Найти ближайший самолет
                    Transform myPlanesControllerTr = GameObject.Find("PlanesController").transform; // родительский объект всех активных самолетов
                    int myPlanesCount = myPlanesControllerTr.childCount; // количество активных самолетов
                    if (myPlanesCount > 0) // продолжаем, только если есть активные самолеты
                    {
                        int myNearestPlaneNumber = 0; // Индекс ближайшего самолета среди детей myPlanesControllerTr
                        float myNearestPlaneDist = 1000000f; // расстояние от ступы до самолета
                        for (int i = 0; i < myPlanesCount; i++)
                        {
                            float myDist = Vector3.Distance(myPlanesControllerTr.GetChild(i).position, transform.position);
                            if (myDist < myNearestPlaneDist)
                            {
                                myNearestPlaneNumber = i;
                                myNearestPlaneDist = myDist;
                            }
                        }
                        // Перейти в дети ближайшего самолета
                        transform.parent = myPlanesControllerTr.GetChild(myNearestPlaneNumber);
                        // Перелететь "на хвост" ближайшего самолета
                        myStartPos = transform.localPosition;
                        myStarttEu = transform.localEulerAngles;
                        myStarttEu.y = myFuncNormalizeAngle(myStarttEu.y); // нормализовать курсовой угол в диапазоне +/- 180 градусов
                        myEndPos = _ComPars.MortarTailPos;
                        myEndEu = Vector3.zero;
                        myStartTime = Time.time;

                        myFlight = true;
                    }
                }
                else // Вернуться в корень (слезть с хвоста)
                {
                    transform.parent = null; // Выйти в корень иерархии сцены
                }
            }
            // Управление перемещением и поворотом
            else
            {
                float x = 0f;
                float y = 0f;
                float z = 0f;
                float w = 0f;
                float myCurMouseX = 0;

                // Сначала получим сигналы от джойстика
                // Сигналы от осей джойстика
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
                y = Input.GetAxis("Throttle");
                w = Input.GetAxis("Twist");
                float myCameraPitch = Input.GetAxis("Hat_Vert");

                // Наклонить камеру по тангажу - отработаем сразу. Работает только если VR не активен (проект запущен без маски VR)
                Vector3 myCamEu = _referenceCamera.transform.localEulerAngles;
                myCamEu.x = myCamEu.x + myCameraPitch;
                _referenceCamera.transform.localEulerAngles = myCamEu;

                // Клавиатура и мышь - сигналы суммируются (и с джойстиком). Исключение - нажатая левая кнопка мыши отрабатывается сразу, независимо от других сигналов (то есть, по сути, тоже суммируется)

                // Сначала - перемещение при нажатой левой кнопке мыши
                // Возьмем начальную точку
                if (Input.GetMouseButtonDown(0))
                {
                    _oldMousePos = Input.mousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    Vector3 myCurMousePos = Input.mousePosition;
                    x = x + (myCurMousePos.x - _oldMousePos.x) * _ComPars.MortarPanSpeed / 100;
                    z = z + (myCurMousePos.y - _oldMousePos.y) * _ComPars.MortarPanSpeed / 100;
                }

                // Перемещение по колесику мыши
                y = y + Input.GetAxis("Mouse ScrollWheel") * 20;

                // Поворот при нажатой правой кнопке мыши. Установим параметр поворота w
                if (Input.GetMouseButtonDown(1))
                {
                    myOldMouseX = Input.mousePosition.x;
                }
                if (Input.GetMouseButton(1))
                {
                    myCurMouseX = Input.mousePosition.x;
                    if (myCurMouseX > myOldMouseX)
                    {
                        w = w + 1.0f;
                    }
                    else if (myCurMouseX < myOldMouseX)
                    {
                        w = w - 1.0f;
                    }
                }

                // Теперь получим сигналы от клавиатуры
                if (Input.GetKey("up") || Input.GetKey("w")) z = z + 0.5f;
                if (Input.GetKey("left") || Input.GetKey("a")) x = x - 0.5f;
                if (Input.GetKey("down") || Input.GetKey("s")) z = z - 0.5f;
                if (Input.GetKey("right") || Input.GetKey("d")) x = x + 0.5f;

                // Если нажат shift, трансформируем сигналы
                if (Input.GetKey("left shift") || Input.GetKey("right shift"))
                {
                    // Если нет сигнала от оси "Throttle", то возьмем высоту от оси z ("Vertical")
                    if (y == 0.0f)
                    {
                        y = z*2;
                        z = 0.0f;
                    }
                    // Если нет сигнала от оси "Twist" и мыши то возьмем поворот от оси x ("Horizontal")
                    if (w == 0.0f)
                    {
                        w = x*2;
                        x = 0.0f;
                    }
                }

                // Если был сигнал поворота, поворачиваем
                if (!(w == 0.0f))
                {
                    MyRotateAvatar(w);
                }
                // Если был сигнал перемещения, перемещаем
                if (x != 0.0f || y != 0.0f || z != 0.0f)
                {
                    float myHorSpeed = Mathf.Clamp(_ComPars.MortarPanSpeed * transform.localPosition.y / _ComPars.MortarHeightMin * 1000, _ComPars.WorldScale.x*100.0f, _ComPars.WorldScale.x * 1000000.0f); // Умножим скорость перемещения на относительную высоту
                    float myVertSpeed = Mathf.Clamp(_ComPars.MortarVertSpeed * transform.localPosition.y / _ComPars.MortarHeightMin * transform.localPosition.y / _ComPars.MortarHeightMin * 10, _ComPars.MortarVertSpeed, _ComPars.MortarVertSpeed * 25.0f); // Умножим вертикальную скорость перемещения на относительную высоту
                    // Переместить по горизонтали
                    transform.Translate(x * myHorSpeed, 0f, z * myHorSpeed);
                    //print("Параметры перемещения: x = " + x + ", z = " + z + ", myHorSpeed = " + myHorSpeed + ", y =" + y + ", myVertSpeed = " + myVertSpeed);
                    // Взять позицию
                    Vector3 myPos = transform.localPosition;
                    // Ограничить новую высоту
                    myPos.y = Mathf.Clamp((myPos.y + y * y * y * myVertSpeed * _ComPars.WorldScale.y), _ComPars.MortarHeightMin * _ComPars.WorldScale.y, _ComPars.MortarHeightMax * _ComPars.WorldScale.y);
                    //Применить
                    transform.localPosition = myPos;
                }

            }
        }
        mySceenMessage.text = "Высота камеры = " + Math.Round(transform.position.y / _ComPars.WorldScale.y, 0);
    }

    // Повернуть опорный объект (Mortar) вокруг камеры (так, чтобы камера вращалась, оставалась на месте)
    // Если просто поворачивать аватара вокруг своей оси, камера едет, как на карусели.
    void MyRotateAvatar(float w)
    {
        // Возьмем положение головы
        Vector3 myPos = _referenceCamera.transform.position;
        // Возьмем свои углы Эйлера
        Vector3 myEu = transform.eulerAngles;
        // Откорректируем высоту
        myPos.y = transform.position.y;
        // Поставим туда вспомогательный объект
        _CameraPlumb.position = myPos;
        _CameraPlumb.eulerAngles = myEu;
        //print("1) _CameraPlumb.position = " + myPos.ToString("F4") + " _CameraPlumb.eulerAngles = " + myEu.ToString("F4"));
        // Пойдем к нему в дети
        Transform myParent = transform.parent; // но запомним своего родителя
        _CameraPlumb.parent = null;
        transform.parent = _CameraPlumb;
        // Возьмем углы Эйлера
        myEu.y += w; // Повернуть по курсу
        _CameraPlumb.eulerAngles = myEu; // Применить
        // Вернем отцов и детей на место
        _CameraPlumb.parent = transform;
        transform.parent = myParent;
        //print("2) _CameraPlumb.position = " + myPos.ToString("F4") + " _CameraPlumb.eulerAngles = " + myEu.ToString("F4"));
    }





    // Приведем угол от (0/360) к (-180/+180)
    float myFuncNormalizeAngle(float myAngle)
    {
        while (myAngle > 180.0f)
        {
            myAngle -= 360.0f;
        }
        while (myAngle < -180.0f)
        {
            myAngle += 360.0f;
        }
        return myAngle;
    }

}
