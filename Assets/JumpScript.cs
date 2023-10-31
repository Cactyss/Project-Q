using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class JumpScript : MonoBehaviour
{//so this little script makes it so when the player tries to jump a text popup randomizes and says "jump", or something similar,
 //instead of the character jumping. Maybe the player will have a slight chance to actually jump.
    private int rotate;
    private int size;
    private int duration;
    private int version;

    public TextMeshPro text;
    
    void Start()
    {
        text.gameObject.SetActive(false);
        rotate = 0;
        size = 0;
        duration = 0;
        version = 0;

        List<Font> fonts = new List<Font>
        {
            Resources.Load<Font>("AllahMuhammad2022-axnpx")
        };

        Font one = Resources.Load<Font>("Baloo");
        TMP_FontAsset asset = TMP_FontAsset.CreateFontAsset(one);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            rotate = Random.Range();
            size = Random.Range();
            duration = Random.Range();
            version = Random.Range();

            StartCoroutine(jump(rotate, size, duration, version));
        }
    }
    private IEnumerator jump(int r, int s, int d, int v)
    {
        text.fontSize = s;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
       
        //stay on screen for d seconds
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(d);
        text.gameObject.SetActive(false);
    }
}
