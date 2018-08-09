using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// THE VARIABLES
    /// Frankie and Ro
    /// </summary>
    public GameObject player;
    public float health;
    public float moveSpeed = 50.0f; //the movement speed
    public float force = 10.0f;
    public float minDistance;
    public Rigidbody rb;
 	public GameObject ouchie;
    public Text damageTaken;
    public Text deathStatus; 
    public GameObject dying; //death overlay for blood spatter
    public GameObject arms;
    //private CapsuleCollider collide;
	public Inventory inventory; 
	public GameObject hud;
	public GameObject startPanel;
	private bool inventoryUp;
    public AudioClip scream;
    public AudioSource screamSource;

	public AudioClip enemy;//when the player comes in contact with the Junk Room Enemy

	GameManager gameManager; 





    void Start()
    {
        //collide = GetComponent<CapsuleCollider>();
        screamSource.clip = scream;
        Cursor.lockState = CursorLockMode.Locked;
        dying.SetActive(false);
        ouchie.SetActive(false); 
		hud.SetActive (false); 
		startPanel.SetActive (false); 
		StartCoroutine (Panel ()); 

		gameManager = GameManager.instance; 		//get the GameManager script
    }

    void Update()
    {
      ///INVENTORY SCRIPTING
        if (Input.GetKeyDown("escape"))
        {
			float stop = 0.0f; 
			float stopTwo = 0.0f; 

            Cursor.lockState = CursorLockMode.None;
			transform.Translate (stop, 0, stopTwo); 
        }


		if (Input.GetKeyDown(KeyCode.I)){

			inventoryUp = !inventoryUp; 

			if (inventoryUp == true) {
				hud.SetActive (true);


			} else {
				hud.SetActive (false); 
			}

		}

		if (Input.GetKeyDown(KeyCode.P)){

			startPanel.SetActive (false);
			Time.timeScale = 1; 

		}

    }
    private void FixedUpdate()
    {
        ///MOVEMENT FOR PLAYER
      float translation = Input.GetAxis("Vertical") * moveSpeed;
        float straffe = Input.GetAxis("Horizontal") * moveSpeed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;
        transform.Translate(straffe, 0, translation);


    }

    /// <summary>
    /// PLAYER REACTION WHEN HIT BY HAUNT
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Item")
        {
            health += 20;
            moveSpeed = 25.0f;
            
            damageTaken.text = "Your damage taken is: " + health;
            
        }

        if (collision.gameObject.name == "TheHaunt")
        {
            //GetComponent<AudioSource>().PlayOneShot(enemy);

            //damaged
            rb.velocity = Vector3.zero; //the velocity goes to zero
            health--;
            damageTaken.text = "Your Health is " + health;
            StartCoroutine(Booboo());

            //print("Your Health is " + health);

            //death
            if (health == 0)
            {
                dying.SetActive(true);
                deathStatus.text = "You Died!";
                StartCoroutine(Death());
            }
        }

        ///WHEN A PLAYER HITS A WALL
        if (gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;

        }

		if (collision.gameObject.name == "Enemy")
		{
			//damaged
			rb.velocity = Vector3.zero;
			health--;

			//play Junk Room Enemy sound
			GetComponent<AudioSource> ().PlayOneShot (enemy); 

			damageTaken.text = "Your Health is " + health;
			StartCoroutine(Booboo());

			//print("Your Health is " + health);

			//death
			if (health == 0)
			{
				dying.SetActive(true);
				deathStatus.text = "You Died!";
				StartCoroutine(Death());
			}


		}

		IInventoryItem item = collision.collider.GetComponent<IInventoryItem> (); 
		if (item != null) {
			inventory.AddItem (item); 

		}
	    //WHEN A PLAYER HITS A DOOR OBJECT
		if (collision.gameObject.tag == "Door") {
			Debug.Log ("door"); 
			GameManager.instance.ReturnFromScene();
			Cursor.lockState = CursorLockMode.None;			//fixing the cursor that it reappears when going back to game scene, because no longer FP
			GameManager.instance.LoadScene(); 
		}

    }

    //flashing the ouchie overlay when damage occurs
    IEnumerator Booboo() {
        ouchie.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        ouchie.SetActive(false);
    }

    /// <summary>
    /// THE DEATH METHOD
    /// </summary>
    /// <returns></returns>
     IEnumerator Death() {
        
        print("You Died!");
        screamSource.Play();
        damageTaken.enabled = false;
        arms.SetActive(false);
        Cursor.lockState = CursorLockMode.None; //unlocks the cursor so it works when you go back to the menu
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

	IEnumerator Panel(){
		yield return new WaitForSeconds (5); 
		startPanel.SetActive (true); 
		Time.timeScale = 0; 
	}
}
