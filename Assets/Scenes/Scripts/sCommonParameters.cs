using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class sCommonParameters : MonoBehaviour
{

    // Получать данные из интернета. В противном случае из файла Record.txt
    public bool DataFromWeb = true;

    // Отладочный параметр - запиcывать ли логи
    public bool WriteLog = true;

    // Запиcывать ли исходные данные web
    public bool WriteWebData = true;

    // Глобальный масштаб сцены, начальное значение
    public Vector3 WorldScale0 = new Vector3(0.000242f, 0.000242f, 0.000242f);

    // Глобальный масштаб сцены
    [NonSerialized]
    public Vector3 WorldScale;

    // Режимы для меню настроек ----------------------

    // Режим коррекции положения корневого объекта относительно ARCore якоря и другие настройки, доступные через меню
    [NonSerialized]
    public bool MapCorrectionMode = false;

    // Режим выбора глиссад
    [NonSerialized]
    public bool GlideSelection = false;

    // Режим выбора подходов
    [NonSerialized]
    public bool STARselection = false;

    // Режим выбота точек IF
    [NonSerialized]
    public bool IFselection = false;

    // Режим включения/выключения маяков
    [NonSerialized]
    public bool BeaconsSwitch = false;

    // Режим включения/выключения баннеров
    [NonSerialized]
    public bool BannersSwitch = false;

    // ------------------------------------------------

    // Объект карты MapBox
    [SerializeField]
    GameObject _Map;

    // Класс управления картой MapBox
    private AbstractMap _AbsMap;

    // Масштаб карты MapBox, начальное значение
    [NonSerialized]
    public float MapZoom0;

    // Код аэропорта
    public string Airport = "UUEE";

    // Высота аэропорта над уровнем моря
    public float AirportALt = 192;

    // Расстояние от начальной точки (км), на котором следить за самолетами
    [SerializeField]
    float myDistance = 60.0f;

    // Скорости перемещений ступы с наблюдателем
    public float MortarPanSpeed = 5f;
    public float MortarVertSpeed = 2f;

    // Ограничения высоты перемещений ступы с наблюдателем
    public float MortarHeightMin = 100f;
    public float MortarHeightMax = 20000f;

    // Продолжительность перелета в заданную точку, сек
    public float MortarFlightTime = 2.0f;

    // Положение в начале сеанса
    [NonSerialized] public Vector3 MortarHomePos = new Vector3(0, 3000, -4500);
    [NonSerialized] public Vector3 MortarHomeEu = new Vector3(0, 0, 0);
    // Положение "на вышке"
    [NonSerialized] public Vector3 MortarTowerPos = new Vector3(280, 100, 1100);
    [NonSerialized] public Vector3 MortarTowerEu = new Vector3(0, 165, 0);
    // Положение "на хвосте" - локальный сдвиг относительно самолета-носителя
    [NonSerialized] public Vector3 MortarTailPos = new Vector3(225, 300, -650);

    // Объект для вывода сообщений в пространство
    sTextMessage _WorldMessage;

    // Корневая часть запроса к OpenSky
    String REQUEST_OpenSky_BASE_URL = "https://aka2001:MyOpenSky@opensky-network.org/api/states/all?";
    
    // Полный запрос к  OpenSky
    //[NonSerialized]
    public string URL;

    // Start is called before the first frame update
    void Start()
    {
        // Класс управления картой MapBox
        _AbsMap = _Map.GetComponent<AbstractMap>();

        MapZoom0 = _AbsMap.Zoom;
        WorldScale = WorldScale0;

        // Объект для вывода сообщений в пространство
        _WorldMessage = GameObject.Find("TextMessage").GetComponent<sTextMessage>();
    }


    public void GoToNewPoint(Vector2 newGeo, float newAlt, bool newFlat)
    {
        // Новые координаты в формате 2d вектора Mapbox
        Vector2d myNewCoord = new Vector2d(newGeo.x, newGeo.y);

        // Перейти к новым координатам
        _AbsMap.UpdateMap(myNewCoord);

        // Установить тип карты - плоская или рельефная 
        if (newFlat)
        {
            _AbsMap.Terrain.SetElevationType(ElevationLayerType.FlatTerrain);
            // Обнуление высоты плоской карты
            Vector3 pos = _Map.transform.localPosition;
            pos.y = 0.0f;
            _Map.transform.localPosition = pos;
        }
        else
        {
            _AbsMap.Terrain.SetElevationType(ElevationLayerType.TerrainWithElevation);
            // Коррекция высоты рельефной карты
            Vector3 pos = _Map.transform.localPosition;
            pos.y = -AirportALt * WorldScale.y;
            _Map.transform.localPosition = pos;
        }
        _WorldMessage.myFuncShowMessage("Переход в новую точку", 2.0f);
    }

    public float GetZoom()
    {
        return _AbsMap.Zoom;
    }

    public bool SetZoom(float NewZoom)
    {
        if (NewZoom >= 11.0f && NewZoom < 15.0f)
        {
            _AbsMap.UpdateMap(NewZoom);
            return true;
        }
        return false;
    }

    // Географические координаты в прямоугольные
    public Vector3 GeoToWorldPosition(float Latitude, float Longitude)
    {
        //print("GeoToWorldPosition");
        //print("Latitude = " + Latitude + " Longitude = " + Longitude);

        Vector2d latitudeLongitude = new Vector2d(Latitude, Longitude);
        Vector3 worldPos = _AbsMap.GeoToWorldPosition(latitudeLongitude, false);
        //print("worldPos = " + worldPos + " worldPos.X = " + worldPos.x + " worldPos.Z = " + worldPos.z);

        return worldPos;
    }


    // Прямоуголные координаты  в географические
    public Vector3 WorldToGeoPosition(Vector3 Position)
    {
        Vector2d GeoPos = _AbsMap.WorldToGeoPosition(Position);
        Vector3 GeoPosition = new Vector3((float)GeoPos.x, (float)GeoPos.y, 0.0f);
        return GeoPosition;
    }

    public void setWebRequest(float myStartLatitude, float myStartLongitude)
    {
        print("Контрольная точка аэродрома: " + myStartLatitude + "," + myStartLongitude);
        // Границы прямоугольника +/-(myDistance/2)
        float myLatHalfDistance = myDistance / 111.111f; //111.111 - длина одного градуса меридиана в км
        float myLongHalfDistance = myDistance / Mathf.Cos(myStartLatitude * Mathf.Deg2Rad) / 111.111f;
        float myWestMargin = myStartLatitude - myLatHalfDistance;
        float myEastMargin = myStartLatitude + myLatHalfDistance;
        float myNorthMargin = myStartLongitude + myLongHalfDistance;
        float mySouthMargin = myStartLongitude - myLongHalfDistance;

        URL = REQUEST_OpenSky_BASE_URL + "lamin=" + myWestMargin + "&lomin=" + mySouthMargin + "&lamax=" + myEastMargin + "&lomax=" + myNorthMargin;

    }



    // ================================================================================================================

    // Update is called once per frame
    void Update2()
    {
        if (Input.GetKey("left shift") || Input.GetKey("right shift")) // Если нажата кнопка Shift
        {
            if (Input.GetKeyDown("1")) // Переход к новым координатам
            {

                print("UnityTileSize = " + _AbsMap.UnityTileSize);
                print("WorldRelativeScale = " + _AbsMap.WorldRelativeScale);
                print("Zoom = " + _AbsMap.Zoom);
                print("CenterLatitudeLongitude = " + _AbsMap.CenterLatitudeLongitude);
                print("CenterMercator = " + _AbsMap.CenterMercator);
                print("");

                _AbsMap.SetZoom(_AbsMap.Zoom + 0.1f);
                _AbsMap.UpdateMap();

                Vector2d myNewCoord = new Vector2d(47.26666667f, 11.35f); // Иннсбрук
                _AbsMap.UpdateMap(myNewCoord, 12f);
            }
            if (Input.GetKeyDown("2")) // Переход от плоской карты к объемной
            {
                print(_AbsMap.Terrain.ElevationType);
                if (_AbsMap.Terrain.ElevationType != ElevationLayerType.TerrainWithElevation)
                {
                    _AbsMap.Terrain.SetElevationType(ElevationLayerType.TerrainWithElevation);
                }
                else
                {
                    _AbsMap.Terrain.SetElevationType(ElevationLayerType.FlatTerrain);
                }
                print(_AbsMap.Terrain.ElevationType);
            }
        }

        // Тестовое ссобщение
        if (Input.GetKeyDown("9"))
        {
            _WorldMessage.myFuncShowMessage("Нажата клавиша 9", 3.0f);
            Vector3 myGeoPos = WorldToGeoPosition(Vector3.zero);
        }

    }

}
