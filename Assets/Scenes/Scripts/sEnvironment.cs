﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sEnvironment : MonoBehaviour {

    // Объекты и массивы объектов окружения, которые нужно включать/выключать

    // Глиссады
    [SerializeField]
    GameObject[] myGlidePaths = new GameObject[4];

    // Глиссады: какая сейчас включена (-1 - все выключены)
    [SerializeField]
    int myActiveGlidePath = 0;

    // Точки IF
    [SerializeField]
    GameObject[] myIFPoints = new GameObject[2];

    // Точки IF: какая сейчас включена (-1 - все выключены)
    [SerializeField]
    int myActiveIFPoint = 0;

    // Маршруты STAR
    [SerializeField]
    GameObject[] myStarPaths = new GameObject[5];

    // Маршруты STAR: какой сейчас включен (-1 - все выключены)
    [SerializeField]
    int myActiveStarPath = 0;

    // Путевые точки
    [SerializeField]
    GameObject myPoints;

    // Строения
    [SerializeField]
    GameObject myBuildings;

    // Дороги
    [SerializeField]
    GameObject mySurface;
    
    // Баннеры точек маршрутов
    GameObject[] myPointBanners;

    // Use this for initialization
    void Start () {

        // Баннеры точек маршрутов
        myPointBanners = GameObject.FindGameObjectsWithTag("PointBanner");

        // Глиссады
        MyFuncSet(myGlidePaths, myActiveGlidePath);

        // Точки IF
        MyFuncSet(myIFPoints, myActiveIFPoint);

        // Маршруты STAR
        MyFuncSet(myStarPaths, myActiveStarPath);

    }
	
	// Update is called once per frame
	void Update () {

        // Клавиша 1 - Глиссады
        if (Input.GetKeyDown("1"))
        {
            myActiveGlidePath = MyFuncSwitch(myGlidePaths, myActiveGlidePath);
        }

        // Клавиша 2 - Точки IF
        else if (Input.GetKeyDown("2"))
        {
            myActiveIFPoint = MyFuncSwitch(myIFPoints, myActiveIFPoint);
        }


        // Клавиша 3 - Маршруты STAR
        else if (Input.GetKeyDown("3"))
        {
            myActiveStarPath = MyFuncSwitch(myStarPaths, myActiveStarPath);
        }

        // Клавиша 4 - Путевые точки
        else if (Input.GetKeyDown("4"))
        {
            myPoints.SetActive(!myPoints.activeSelf);
        }

        // Клавиша 5 - Строения
        else if (Input.GetKeyDown("5"))
        {
            myBuildings.SetActive(!myBuildings.activeSelf);
        }

        // Клавиша 6 - Дороги
        else if (Input.GetKeyDown("6"))
        {
            mySurface.SetActive(!mySurface.activeSelf);
        }

        // Клавиша b - баннеры путевых точек
        else if (Input.GetKeyDown("b"))
        {
            bool myActive = !myPointBanners[0].activeSelf;
            // Все, как один
            foreach (GameObject myBanner in myPointBanners)
            {
                myBanner.SetActive(myActive);
            }
        }

        // Клавиша ctrl+c - очистить кэш MapBox
        //else if (Input.GetKeyDown("c"))
        //{
        //    if (Input.GetKey("left ctrl") || Input.GetKey("right ctrl"))
        //    {
        //        Mapbox.Unity.MapboxAccess.Instance.ClearCache();
        //    }
        //}

    }

    // Установить активные объекты (начальная установка)
    void MyFuncSet(GameObject[] myObjects, int myPointer)
    {
        for (int i = 0; i < myObjects.Length; i++)
        {
            if (i == myPointer)
            {
                myObjects[i].SetActive(true);
            }
            else
            {
                myObjects[i].SetActive(false);
            }
        }
    }

    // Включить следующий объект из массива
    private int MyFuncSwitch(GameObject[] myObjects, int myPointer)
    {
        int myCount = myObjects.Length;
        myPointer++;
        if (myPointer >= myCount)
        {
            myPointer = -1;
        }
        for (int i = 0; i < myCount; i++)
        {
            if (i == myPointer)
            {
                myObjects[i].SetActive(true);
            }
            else
            {
                myObjects[i].SetActive(false);
            }
        }
        return myPointer;
    }

    public int SwitchEnv(string envType)
    {
        switch (envType)
        {
            case "Glides":
                myActiveGlidePath = MyFuncSwitch(myGlidePaths, myActiveGlidePath);
                return myActiveGlidePath;
                //break;
            case "STARs":
                myActiveStarPath = MyFuncSwitch(myStarPaths, myActiveStarPath);
                return myActiveStarPath;
                //break;
            case "IFs":
                myActiveIFPoint = MyFuncSwitch(myIFPoints, myActiveIFPoint);
                return myActiveIFPoint;
                //break;
            case "Beacons":
                myPoints.SetActive(!myPoints.activeSelf);
                int BeaconsIsActive = -1;
                if (myPoints.activeSelf)
                {
                    BeaconsIsActive = 0;
                }
                return BeaconsIsActive;
                //break;
            case "Banners":
                bool myActive = !myPointBanners[0].activeSelf;
                // Все, как один
                foreach (GameObject myBanner in myPointBanners)
                {
                    myBanner.SetActive(myActive);
                }
                int BannersIsActive = -1;
                if (myActive)
                {
                    BannersIsActive = 0;
                }
                return BannersIsActive;
        }
        return -1;
    }

}
