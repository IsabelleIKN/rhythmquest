using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

        public TextMeshProUGUI lastInputTxt;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.ui = this;
    }

    // Update is called once per frame
    public void UpdateLastKey(string input)
    {
        lastInputTxt.text = input;
    }
}
