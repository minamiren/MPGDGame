using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    public GameObject[] dropPrefabs;

    public void DropItem(Vector3 dropPosition)
    {
        if (dropPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, dropPrefabs.Length);

        Instantiate(dropPrefabs[randomIndex], dropPosition, Quaternion.identity);
        
    }
}
