using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Transform objectrotate;
    [SerializeField] private NavMeshSurface surface;

    private void Update()
    {
        objectrotate.localRotation = Quaternion.Euler(new Vector3(0, 15*Time.deltaTime, 0) + objectrotate.localRotation.eulerAngles);
        surface.BuildNavMesh();
    }
}
