using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public Image fillImg;
    float remainingAmt = 300;
    public float dmg = 0;


    private void Start()
    {
        dmg = remainingAmt;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dmg < 0.0)
        {
            toDeath();
        }
		fillImg.fillAmount = dmg / remainingAmt;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Shootable"))
        {
			Debug.Log("Enemy attacked the player");
            dmg -= 20;
            fillImg.fillAmount = dmg / remainingAmt;
        }
    }

    void toDeath()
    {
        SceneManager.LoadScene("Die");
    }
    

}
