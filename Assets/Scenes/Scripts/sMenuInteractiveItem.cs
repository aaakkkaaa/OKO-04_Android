using System;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    // Переделано из sExampleInteractiveItem

    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class sMenuInteractiveItem : MonoBehaviour
    {
        [SerializeField] private Material m_NormalMaterial;                
        [SerializeField] private Material m_OverMaterial;                  
        [SerializeField] private Material m_ClickedMaterial;               
        [SerializeField] private Material m_DoubleClickedMaterial;         
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private Renderer m_Renderer;

        // Главный объект меню
        [SerializeField]
        private GameObject _Menu;

        // Объект для вывода сообщений в пространство
        [SerializeField]
        sTextMessage _WorldMessage;

        Material _MenuItemMat;

        // Сценарий меню
        sMenu _MenuScript;

        private void Awake ()
        {
            m_Renderer.material = m_NormalMaterial;
            //_MenuItemColor = m_Renderer.material.color;

            // Сценарий меню
            _MenuScript = _Menu.GetComponent<sMenu>();
        }


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;

            _MenuItemMat = m_Renderer.material;
            _MenuItemMat.color = _MenuScript.MenuItemColor; // Восстановить начальный цвет пункта меню
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
        }


        //Handle the Over event
        private void HandleOver()
        {
            if (gameObject != _MenuScript.getSelectedItem())
            {
                _MenuItemMat.color = _MenuScript.OverlapColor;
                //_WorldMessage.myFuncShowMessage("Show over " + _MenuItemMat.name, 1);
            }
        }


        //Handle the Out event
        private void HandleOut()
        {
            //_WorldMessage.myFuncShowMessage(gameObject + "/" + _MenuScript.getSelectedItem(), 3);
            if (gameObject != _MenuScript.getSelectedItem())
            {
                _MenuItemMat.color = _MenuScript.MenuItemColor; // Восстановить начальный цвет пункта меню
            }
        }

        // Меня выбрали!
        //Handle the Click event
        private void HandleClick()
        {
            if (gameObject != _MenuScript.getSelectedItem())
            {
                _MenuItemMat.color = _MenuScript.SelectedColor;
                // Вызываем обработчик меню
                _MenuScript.SelectNewItem(gameObject);
            }
        }


        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
            Debug.Log("Show double click");
            //m_Renderer.material = m_DoubleClickedMaterial;
        }
    }
}