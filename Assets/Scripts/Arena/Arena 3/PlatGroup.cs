using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatGroup : MonoBehaviour
{
    [SerializeField] private float deathDistance = 51f; // Three lengths away
    private Transform _centerScreen;
    void Start()
    {
        _centerScreen = GameObject.Find("Center Screen").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Destruction of a group is based on distance from center screen rather than time
        // Thus, we can avoid having to try and fruitlessly time out and match spawn times to destroy times
        if (Vector2.Distance(this.transform.position, _centerScreen.transform.position) > deathDistance && this.transform.position.y < _centerScreen.transform.position.y)
        {
            Destroy(gameObject);
        }
    }

}
