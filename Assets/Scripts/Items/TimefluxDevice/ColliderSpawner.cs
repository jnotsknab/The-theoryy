using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawner : MonoBehaviour
{

    private Collider _collider;
    private void Awake()
    {
        _collider = this.gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
