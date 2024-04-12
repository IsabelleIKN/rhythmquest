using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class HLP
{
    public enum Dir 
    { 
        Up, 
        Down, 
        Left, 
        Right, 
        None
    };

    // 0 = null, 1 = empty, 2 =  wall, 3 = player, 4 = treasure, 5 = enemy
    public static int[,] level1 = new int[10, 10] {
        { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0},
        { 0, 1, 1, 1, 1, 1, 1, 2, 2, 0},
        { 1, 1, 1, 1, 1, 2, 5, 1, 1, 0},
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        { 3, 1, 1, 1, 1, 2, 1, 4, 1, 1},
        { 1, 1, 1, 0, 0, 2, 1, 1, 1, 1},
        { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
        { 0, 1, 1, 1, 4, 1, 1, 5, 1, 0},
        { 0, 1, 2, 1, 1, 1, 1, 1, 0, 0},
        { 0, 0, 4, 5, 1, 1, 1, 0, 0, 0}
    };
}
