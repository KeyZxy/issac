using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextdoor : MonoBehaviour
{
    public Room room;
    GameObject nextdoor;
    // Start is called before the first frame update

    private void Start()
    {
        if(room.PlayerInRoom == true && room.HasExploed == true)
        {
            transform.Find("nextdoor").gameObject.SetActive(true);
            transform.Find("Dnextdoor").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("Level2");
            }
        }
    
}
