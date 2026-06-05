using UnityEngine;

public class Slash : MonoBehaviour
{
    
    [SerializeField]
    Player_Sword player_Sword;


    public static Slash slashInstance { get; private set; }

    private void Awake()
    {
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (slashInstance != null && slashInstance != this)
        {
            
            Destroy(slashInstance.gameObject);
        }

        slashInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //player_Sword
    }
}
