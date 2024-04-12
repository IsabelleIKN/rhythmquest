using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Move(Vector3 playerPos)
    {
            Vector3 moveDir = Vector3.zero;
            moveDir = ChooseMoveDirection(playerPos);
            transform.Translate(moveDir);
    }

    private Vector3 ChooseMoveDirection(Vector3 playerPos)
    {
        Vector3 primaryDirection = DeterminePrimaryDirection(playerPos);
        Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        if (CanMove(primaryDirection))
        {
            return primaryDirection;
        }
        else
        {
            foreach (Vector3 dir in directions)
            {
                // Check if this direction is the opposite of primary direction to avoid going backward
                if (dir != -primaryDirection && CanMove(dir))
                {
                    return(dir);
                }
            }
        }

        return Vector3.zero;
    }

    private Vector3 DeterminePrimaryDirection(Vector3 target)
    {
        Vector3 direction = Vector3.zero;

        float xDifference = target.x - transform.position.x;
        float zDifference = target.z - transform.position.z;

        if (Mathf.Abs(xDifference) > Mathf.Abs(zDifference))
        {
            if (xDifference > 0)
            {
                direction = Vector3.right;
            }
            else
            {
                direction = Vector3.left;
            }
        }
        else
        {
            if (zDifference > 0)
            {
                direction = Vector3.forward;
            }
            else
            {
                direction = Vector3.back;
            }
        }

        return direction;
    }

    private bool CanMove(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 1f))
        {
            //Layer mask, merge in one script to use on player and enemy
            if (hit.collider.tag != "Wall" && hit.collider.tag != "Enemy" && hit.collider.tag != "Treasure")
                return true;
        }

        return false;
    }

}
