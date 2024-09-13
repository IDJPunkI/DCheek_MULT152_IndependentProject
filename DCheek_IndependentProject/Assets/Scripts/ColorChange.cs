using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Material fireMaterial;
    public Material waterMaterial;
    public Material earthMaterial;

    private bool fire = true;
    private bool water = false;
    private bool earth = false;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        InvokeRepeating("ChangeMaterial", 5.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeMaterial()
    {
        if (fire)
        {
            GetComponent<Renderer>().material = waterMaterial;
            water = true;
            fire = false;
        }
        else if (water)
        {
            GetComponent<Renderer>().material = earthMaterial;
            earth = true;
            water = false;
        }
        else if (earth)
        {
            GetComponent<Renderer>().material = fireMaterial;
            fire = true;
            earth = false;
        }    
    }
}
