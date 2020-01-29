using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sKillme : MonoBehaviour
{

    // Трансформ шаблона самолетов
    [SerializeField]
    Transform mySamplePlane;

    // Трансформ группового объекта самолетов
    [SerializeField]
    Transform myPlanesController;

    // Трансформы шаблонов путевых точек
    [SerializeField]
    Transform[] myTrackPoint = new Transform[5];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            print("sKillme: Нажата клавиша 0");
            // Создадим новый самолет
            Transform myNewPlane = Instantiate(mySamplePlane);
            myNewPlane.name = "MyTestPlane";
            myNewPlane.gameObject.SetActive(true);
            myNewPlane.parent = myPlanesController;

            // Указатели путевых точек
            Transform[] myTP = new Transform[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 TP_LocalScale = myTrackPoint[j].localScale;
                myTP[j] = Instantiate(myTrackPoint[j]);
                myTP[j].name = "MyTestPlane" + "_Point_" + j;
                myTP[j].parent = myTrackPoint[j].parent;
                myTP[j].localScale = TP_LocalScale;
                myTP[j].gameObject.SetActive(true);
            }

        }
    }
}
