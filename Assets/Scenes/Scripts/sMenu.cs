﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMenu : MonoBehaviour
{

    // Класс, содержащий общие параметры и методы для работы с ними
    [SerializeField]
    private sCommonParameters _ComPars;

    // Объект для вывода сообщений в пространство
    [SerializeField]
    sTextMessage _WorldMessage;

    // Пункты меню
    [SerializeField]
    GameObject[] _MenuItems;

    // Выбранный пункт меню
    private GameObject _SelectedItem;

    // начальный цвет пунктов меню
    [NonSerialized]
    public Color MenuItemColor;

    // цвет пунктов меню при наведении прицела
    public Color OverlapColor = new Color(1.0f, 0.5f, 0.0f, 0.251f);

    // цвет выбранного пункта меню
    public Color SelectedColor = new Color(1.0f, 0.1f, 0.0f, 0.251f);


    private void OnDisable()
    {
        if (_SelectedItem != null)
        {
            _SelectedItem.GetComponent<Renderer>().material.color = MenuItemColor;
        }
        _SelectedItem = null;
        DisableAll();
    }



    // Получить выбранный пункт меню
    public GameObject getSelectedItem()
    {
        return _SelectedItem;
    }


    // Обработчик меню
    // Сами действия выполняется в sRootCorrection, здесь устанавливаются разрешения (глобальные параметры) на разные виды обработки
    public void SelectNewItem(GameObject newItem)
    {
        // Восстановить цвет предыдущего выбранного пункта
        if (_SelectedItem != null)
        {
            _SelectedItem.GetComponent<Renderer>().material.color = MenuItemColor;
        }

        // Запретить все обработки
        DisableAll();

        _SelectedItem = newItem;

        // Имя нового выбранного пункта меню
        string menuItemName = _SelectedItem.name.Substring(3);
        //_WorldMessage.myFuncShowMessage("Выбран пункт " + menuItemName, 1);

        // Собственно, обработка
        switch (menuItemName)
        {
            case "BasePosition":
                _ComPars.MapCorrectionMode = true;
                break;
            case "Glides":
                _ComPars.GlideSelection = true;
                break;
            case "STARs":
                _ComPars.STARselection = true;
                break;
            case "IFs":
                _ComPars.IFselection = true;
                break;
            case "Banners":
                _ComPars.BannersSwitch = true;
                break;
            case "Close": // обработка закрытия меню
                transform.gameObject.SetActive(false); // Спрятать меню
                break;

        }
    }


    public void DisableAll()
    {
        _ComPars.MapCorrectionMode = false;
        _ComPars.GlideSelection = false;
        _ComPars.STARselection = false;
        _ComPars.IFselection = false;
        _ComPars.BannersSwitch = false;
    }
}
