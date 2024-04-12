using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lastInputTxt;
    [SerializeField] private TextMeshProUGUI treasureTxt;

    public void UpdateLastKey(string input)
    {
        lastInputTxt.text = input;
    }

    public void UpdateTreasureCount(int count, int total)
    {
        treasureTxt.text = count + " / " + total;
    }
}
