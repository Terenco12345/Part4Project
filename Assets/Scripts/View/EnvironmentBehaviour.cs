using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBehaviour : MonoBehaviour
{
    public GameObject environment;

    public void Start()
    {
        environment = Instantiate(ModelManager.Instance.environments[0], transform);
    }

    public void SwapEnvironment(GameObject environmentPrefab)
    {
        // Destroy the previous environment
        Destroy(environment);

        // Create a new environment
        environment = Instantiate(environmentPrefab, transform);
    }
}
