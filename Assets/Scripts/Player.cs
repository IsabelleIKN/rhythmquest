using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{

    private void Start()
    {
        GameManager.instance.player = transform;   
    }
}
