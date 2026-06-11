using UnityEngine;

public class Enemy_Slash : MonoBehaviour
{
    public static Enemy_Slash e_SlashInstance { get; private set; }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (e_SlashInstance != null && e_SlashInstance != this)
        {

            Destroy(e_SlashInstance.gameObject);
        }

        e_SlashInstance = this;
    }

}
