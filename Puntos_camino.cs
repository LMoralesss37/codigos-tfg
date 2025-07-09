using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntos_camino : MonoBehaviour
{
    public float TamanoPuntoCamino = 1f;
    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, TamanoPuntoCamino);
        }

        Gizmos.color = Color.red;
        for(int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        return null;
    }

}
