using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour {

	public GameObject primary;
	public GameObject secondary;
	public GameObject knife;

	public GameObject primaryImage;
	public GameObject secondaryImage;
	public GameObject meleeImage;

	// Use this for initialization
	void Start () {
		primaryImage.SetActive(true);
		secondaryImage.SetActive(false);
		meleeImage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) { //Primary
			primaryImage.SetActive(true);
			secondaryImage.SetActive(false);
			meleeImage.SetActive(false);
			//switch weapon then change image
		}
		else if (Input.GetKeyDown("2")) { //Secondary
			primaryImage.SetActive(false);
			secondaryImage.SetActive(true);
			meleeImage.SetActive(false);
			//switch weapon then change image
		}
		else if (Input.GetKeyDown("3")) { //Knife
			primaryImage.SetActive(false);
			secondaryImage.SetActive(false);
			meleeImage.SetActive(true);
			//switch weapon then change image
		}
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ammo")) {
			Destroy(other.gameObject);
            //myGun.GetComponent<RayCastShootComplete>().ammo += 10;        
        }
		else if (other.gameObject.CompareTag("Health")) {
			Destroy(other.gameObject);
			//GetComponent<Health>().dmg += 50;
		}
	}
}
