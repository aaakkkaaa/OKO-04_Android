using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sUtils : MonoBehaviour
{
    // Основная камера (установить в Inspector)
    [SerializeField]
    Camera _MainCam;

    // Дополнительная (правая) камера (установить в Inspector)
    [SerializeField]
    Camera _RightCam;

    // Поле зрения в градусах
    int _FOV;

    // Текстовое поле для вывода FOV
    Text _TextFOV;

    // Слайдер для установки FOV
    Slider _SliderFOV;

    // Start is called before the first frame update
    void Start()
    {

        // Поле зрения в градусах
        _FOV = (int)_MainCam.fieldOfView;

        // Текстовое поле для вывода FOV
        _TextFOV = GameObject.Find("TextFOV").GetComponent<Text>();
        _TextFOV.text = "FOV = " + _FOV;

        // Положение слайдера для установки FOV
        _SliderFOV = GameObject.Find("SliderFOV").GetComponent<Slider>();
        _SliderFOV.value = _FOV;
    }

    // Значение слайдера изменено (вызов установлен в свойствах слайдера SliderFOV в Inspector)
    public void SliderValueChanged()
    {
        _FOV = (int)_SliderFOV.value;
        // Текстовое поле для вывода FOV
        _TextFOV.text = "FOV = " + _FOV;
        // Установить полz зрения обеих камер
        _MainCam.fieldOfView = _FOV;
        _RightCam.fieldOfView = _FOV;
    }
}
