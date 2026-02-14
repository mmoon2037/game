using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    
    public Light myLight;

    void Start()
    {
        
        if (myLight == null)
        {
            myLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            if (myLight != null)
            {
                myLight.enabled = !myLight.enabled;
            }
        }
    }
}
