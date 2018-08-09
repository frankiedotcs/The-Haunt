using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

    /// <summary>
    /// This script is to help animate objects when they are reacted upon
    /// Frankie wrote but didn't have time to implement
    /// </summary>
    /// 

    //animator variable
    public Animator reactionAn;

    /// <summary>
    ///the state of the object
    /// </summary>
    public enum State { 
        touch,
        idle

    }

    public State state;
    // Use this for initialization
    void Start() {
        reactionAn = GetComponent<Animator>();
        state = InteractObject.State.idle;
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void onMouseEnter() {

        Debug.Log("Open");
    }

    public void OnMouseExit() {

        Debug.Log("Grab");

        state = InteractObject.State.touch;

        if (state == InteractObject.State.touch)
        {
            Touch();
        }
        else {
           Idle();
        }
    }

    public void OnMouseUp()
    {
       
    }

    /// <summary>
    /// The animation for when you touch the object
    /// </summary>
    private void Touch() {
        reactionAn.Play("Grab");

    }
    /// <summary>
    /// the animation for when you stop touching an object
    /// </summary>
    private void Idle() {

        reactionAn.Play("Idle");
    }
}
