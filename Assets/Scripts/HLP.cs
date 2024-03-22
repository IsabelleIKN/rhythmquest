using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class HLP
{
    public enum KeyPress 
    { 
        Up, 
        Down, 
        Left, 
        Right, 
        None
    };

    public static int[,] level1 = new int[5, 5] {
        { 0, 0, 2, 0, 0},
        { 1, 0, 0, 0, 3},
        { 0, 0, 0, 0, 0},
        { 0, 3, 0, 2, 0},
        { 0, 0, 0, 0, 0}
    };


}
