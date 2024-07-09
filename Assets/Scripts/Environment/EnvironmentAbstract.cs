using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAbstract : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _levelObjects;

    public void SetLevelObject(int level)
    {
        for (int i = 0; i < _levelObjects.Count; i++)
        {
            if (i == (level - 1))
            {
                _levelObjects[i].SetActive(true);
                continue;
            }
            _levelObjects[i].SetActive(false);
        }
    }

}
