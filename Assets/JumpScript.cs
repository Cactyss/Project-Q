using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JumpScript : MonoBehaviour
{//so this little script makes it so when the player tries to jump a text popup randomizes and says "jump", or something similar,
 //instead of the character jumping. Maybe the player will have a slight chance to actually jump.
    private float rotate;
    private float size;
    private float duration;
    private int version;

    public GameObject text;
   
    
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
            rotate = UnityEngine.Random.Range(-40, 40);
            size = UnityEngine.Random.Range(0.2f, 1f);
            duration = UnityEngine.Random.Range(1f, 4f);
            version = UnityEngine.Random.Range(1, 32);

            StartCoroutine(jump(rotate, size, duration, version));
        }
    }
    private IEnumerator jump(float r, float s, float d, int v)
    {
        text.transform.rotation = new Quaternion(0, 0, r, 0);
        //text.GetComponent<TextMeshPro>().fontSize = s;
        //text.GetComponent<TextMeshPro>().font = Resources.GetBuiltinResource(typeof(TMP_FontAsset), "AttackGraffiti-3zRBM.ttf") as TMP_FontAsset;
      

        //stay on screen for d seconds
        text.SetActive(true);
        yield return new WaitForSeconds(d);
        text.SetActive(false);
        Debug.Log(r);
        Debug.Log(d);
        Debug.Log(s);
        Debug.Log(v);
    }
}
