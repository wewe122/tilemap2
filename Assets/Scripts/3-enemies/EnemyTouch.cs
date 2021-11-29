using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouch : MonoBehaviour

{
    [SerializeField] Transform enemy1 = null;
    [SerializeField] Transform enemy2 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if(transform.position==enemy1.position || transform.position == enemy2.position)
        {
            transform.position = new Vector3(1.16f, 0.47f, 0);
        }
        
    }
}
