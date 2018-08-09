using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HauntController : MonoBehaviour {
    /// <summary>
    /// Frankie
    /// </summary>
    public Transform player; //the player object
    public float minDistance;
    public float maxDistance;
    public float speed;
    public Rigidbody rb;
    private CapsuleCollider collide;
    public float health = 0.0f;
    public Text winCondition;
    public GameObject TheHaunt;
    public Animator anim;
    public Text hauntHealth;

    // Use this for initialization
    void Start () {

        collide = GetComponent<CapsuleCollider>();
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
        if(Vector3.Distance(player.position, this.transform.position) < maxDistance)
        {
            Vector3 face = (player.position - this.transform.position) * Time.deltaTime * speed;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(face), 0.1f);
            anim.SetTrigger("Attacking");


            if (face.magnitude > minDistance)
            {
                this.transform.Translate(0, 0, 0.5f);
            }

        }

        
	}
    void Update() {
        
        if (Input.GetKeyDown(KeyCode.L)) {

            SceneManager.LoadScene(0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero; //reduce the movement when hit to zero

        //needs attack player
        if (collision.gameObject.CompareTag("Player"))
        {
            //health goes down if hit by player
            health--;
            hauntHealth.text = ("Haunt Health is " + health);

            //The Win Condition
            if (health == 0)
            {
                StartCoroutine(Win());
            }
        }
        else if (collision.gameObject.CompareTag("Hands"))
        {
            health -= 5;
            Debug.Log("Haunt Health is " + health);

        }
    }
    /// <summary>
    /// Win condition totally needs to be a separate script but whatever!
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        anim.SetTrigger("Dead");
        winCondition.text = "The creature perishes in a bloody heap leaving nothing but the stench of regret behind. Amelia takes a deep breath, feeling finally at peace in this house ...this time.";
        
        yield return new WaitForSeconds(5);
        TheHaunt.SetActive(false);
        Cursor.lockState = CursorLockMode.None; //unlocks the cursor so it works when you go back to the menu
        SceneManager.LoadScene(0);
        



    }
}
