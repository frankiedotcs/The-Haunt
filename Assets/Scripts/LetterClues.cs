using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterClues : MonoBehaviour
{
    /// <summary>
    /// Frankie's Letter Clue's Script
    /// </summary>
    public GameObject letterUi; //the letter sprite
    public GameObject paper;
   
    // Use this for initialization
    void Start()
    {
        letterUi.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X)){

            letterUi.SetActive(false);
            Time.timeScale = 1;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        StartCoroutine(CluesUI());
         
    }

    IEnumerator CluesUI()
    {
        letterUi.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(1);
        Destroy(paper);
    }
}
