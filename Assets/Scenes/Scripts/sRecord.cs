using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class sRecord : MonoBehaviour
// ********************** Запись данных в файлы ********************************************
{

    // Папка для записи файлов
    public String RecDir = "Record";
    // Словарь - массив файлов для записи данных. Ключ - имя файла, значение - объект StreamWriter
    Dictionary<String, StreamWriter> _RecFile = new Dictionary<String, StreamWriter>();
    /*
    Main     - Файл для записи по умолчанию
    RawData  - Файл для записи получаемых данных
    Thread   - Файл для записи в фоновом потоке
    ProcData - Файл для записи в процессе обработки данных (комбинированный поток: фоновый + корутина)
    Update   - Файл для записи в каждом кадре
    Banners  - Файл для отладки "Избавиться от наложения баннеров"

    ADSB_Exchange - Файл для записи исходных данных adsbexchange.com
    OpenSky - Файл для записи исходных данных opensky-network.org
    */

    // Класс, содержащий общие параметры и методы для работы с ними
    private sCommonParameters _ComPars;

    // Параметры времени
    sTime _Time;



    void Awake()
    {
        // Класс, содержащий общие параметры и методы для работы с ними
        _ComPars = transform.GetComponent<sCommonParameters>();

        print("RecDir = " + RecDir);

        // Создать папку
        Directory.CreateDirectory(RecDir);
        RecDir = Path.Combine(Directory.GetCurrentDirectory(), RecDir);
        print("RecDir = " + RecDir);

        // Создать (пересоздать) файлы и добавить из в словарь
        if (_ComPars.WriteLog) // Если установлен параметр записи логов
        {
            // Файл для записи по умолчанию
            AddToDic("Main");
            // Файл для записи получаемых данных
            AddToDic("RawData");
            // Файл для записи в фоновом потоке
            AddToDic("Thread");
            // Файл для записи в процессе обработки данных (комбинированный поток: фоновый + корутина)
            AddToDic("ProcData");
            // Файл для записи в каждом кадре
            AddToDic("Update");
            // Файл для отладки "Избавиться от наложения баннеров"
            AddToDic("Banners");
        }

        if (_ComPars.WriteWebData) // Если установлен параметр записи полученных из Web данных
        {
            //Adsbexchange - Файл для записи исходных данных adsbexchange.com
            //AddToDic("ADSB_Exchange");
            //OpenSky - Файл для записи исходных данных opensky-network.org
            AddToDic("OpenSky");
        }

        // ******************************************************************

        // Параметры времени
        _Time = transform.GetComponent<sTime>();

    }

    // Добавить в словарь имя файла и созданный объект StreamWriter
    public void AddToDic(String myRecFileName)
    {
        _RecFile.Add(myRecFileName, new StreamWriter(Path.Combine(RecDir, myRecFileName + ".txt")));
    }


    // ****************  4 перегруженных функции для записи лог-файлов   ********************************
    // Запись в указанный файл
    public void MyLog(string myRecName, String myInfo)
    {
        if (_ComPars.WriteLog)
        {
            int myCurrentTime = _Time.CurrentTime();
            _RecFile[myRecName].WriteLine(myInfo.Replace(".", ",") + " CurrentTime = " + myCurrentTime);
        }
    }

    // Запись в указанный файл с возможностью не добавлять время
    public void MyLog(string myRecName, String myInfo, bool myTime)
    {
        if (_ComPars.WriteLog)
        {
            if (myTime)
            {
                int myCurrentTime = _Time.CurrentTime();
                _RecFile[myRecName].WriteLine(myInfo.Replace(".", ",") + " CurrentTime = " + myCurrentTime);
            }
            else
            {
                _RecFile[myRecName].WriteLine(myInfo.Replace(".", ","));
            }
        }
    }

    // Запись в файл по умолчанию
    public void MyLog(String myInfo)
    {
        if (_ComPars.WriteLog)
        {
            _RecFile["Main"].WriteLine(myInfo.Replace(".", ","));
        }
    }

    // Запись в два файла
    public void MyLog(string myRecName1, string myRecName2, String myInfo)
    {
        if (_ComPars.WriteLog)
        {
            int myCurrentTime = _Time.CurrentTime();
            _RecFile[myRecName1].WriteLine(myInfo.Replace(".", ",") + " CurrentTime = " + myCurrentTime);
            _RecFile[myRecName2].WriteLine(myInfo.Replace(".", ",") + " CurrentTime = " + myCurrentTime);
        }
    }

    // Запись в файл web данных
    public void WebData(string myRecName, string myInfo)
    {
        if (_ComPars.WriteWebData)
        {
            _RecFile[myRecName].WriteLine(myInfo);
        }
   }

    // ******************************************************************

    // Закрыть один лог-файл и удалить его запись из словаря лог-файлов
    public void Close(string myRecName)
    {
        if (_ComPars.WriteLog)
        {
            _RecFile[myRecName].Close();
            _RecFile.Remove(myRecName);
        }
    }

    // Закрыть все открытые лог-файлы
    public void CloseAll()
    {
        // Закрыть все открытые лог-файлы
        List<String> myKeys = new List<String>(_RecFile.Keys);
        for (int i = 0; i < myKeys.Count; i++)
        {
            _RecFile[myKeys[i]].Close();
        }
    }



}
