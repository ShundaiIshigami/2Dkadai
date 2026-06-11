using UnityEngine;

public class Slash : MonoBehaviour
{
    
    public static Slash slashInstance { get; private set; }

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (slashInstance != null && slashInstance != this)
        {
            
            Destroy(slashInstance.gameObject);
        }

        slashInstance = this;
    }

    
}
