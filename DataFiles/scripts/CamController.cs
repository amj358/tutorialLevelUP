using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamController : MonoBehaviour
{
	public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float moveCam = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

		transform.Translate (0, 0, moveCam);

        float rotateCam = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotateCam = rotateCam - 1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotateCam = rotateCam + 1;
        }

        transform.Rotate(0, rotateCam, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TREASURE"))
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
