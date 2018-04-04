using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var sr = GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.Log("sprite renderer is NULL");
            return;
        }
        
        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector2 p = new Vector2
        {
            x = worldScreenWidth / width,
            y = worldScreenHeight / height
        };

        transform.localScale = p;
        Debug.Log("scale: " + p);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
