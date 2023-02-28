using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChineseChecker : MonoBehaviour
{
    public enum ChineseState
    {
        Blank,
        Red,
        Blue,
        Potential
    }

    [SerializeField]
    private ChineseState state = ChineseState.Blank;

    public int i;
    public int j;

    public void SetState(ChineseState newState)
    {
        state = newState;
        this.GetComponent<Image>().color = state switch
        {
            ChineseState.Blank => Color.white,
            ChineseState.Red => Color.red,
            ChineseState.Blue => Color.blue,
            ChineseState.Potential => Color.yellow,
            _ => Color.white,
        };
    }

    public ChineseState GetState()
    {
        return state;
    }

}
