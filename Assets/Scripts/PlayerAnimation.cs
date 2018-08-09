using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    /// <summary>
    /// Frankie
    /// </summary>
    public Animator anim;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attacking");
        }

        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Grabbing");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "TheHaunt")
        {

            anim.SetTrigger("Hit");
        }
    }
}

