using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GyroPuzzle : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI X;
    [SerializeField]
    TextMeshProUGUI Y;
    [SerializeField]
    TextMeshProUGUI Z;


    public void OnGyroscopeSensorChanged(float valX, float valY, float valZ)
    {

        X.text = ("valX = " + Mathf.Rad2Deg * (valX));
        Y.text = ("valY = " + Mathf.Rad2Deg * valY);
        Z.text = ("valZ = " + Mathf.Rad2Deg * valZ);

    }

}
