using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class JumpScript : MonoBehaviour
{//so this little script makes it so when the player tries to jump a text popup randomizes and says "jump", or something similar,
 //instead of the character jumping. Maybe the player will have a slight chance to actually jump.
    private float rotate;
    private int texty;
    private int colory;
    private float size;
    private float duration;
    private int version;
    private int xlocal;
    private int ylocal;
    private List<TMP_FontAsset> fonts;
    public TMP_FontAsset one;
    public TMP_FontAsset two;
    public TMP_FontAsset three;
    public TMP_FontAsset four;
    public TMP_FontAsset five;
    public TMP_FontAsset six;
    public TMP_FontAsset seven;
    public TMP_FontAsset eight;
    public TMP_FontAsset nine;
    public TMP_FontAsset ten;
    public TMP_FontAsset eleven;
    public TMP_FontAsset twelve;
    public TMP_FontAsset thirteen;
    public TMP_FontAsset fourteen;
    public TMP_FontAsset fifteen;
    public TMP_FontAsset sixteen;
    public TMP_FontAsset seventeen;
    public TMP_FontAsset eightteen;
    public GameObject text;
    
   
    
    void Start()
    {
        colory = 0;
        text.gameObject.SetActive(false);
        rotate = 0;
        size = 0;
        duration = 0;
        version = 0;
                fonts = new List<TMP_FontAsset>
                {
                    one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve, thirteen, fourteen, fifteen, sixteen, seventeen, eightteen
                };
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            rotate = UnityEngine.Random.Range(-40, 40);
            size = UnityEngine.Random.Range(0.2f, 2f);
            duration = UnityEngine.Random.Range(1.5f, 4f);
            version = UnityEngine.Random.Range(0, 17);
            colory = UnityEngine.Random.Range(1, 6);
            texty = UnityEngine.Random.Range(1, 100);
            xlocal = UnityEngine.Random.Range(-100, 100);
            ylocal = UnityEngine.Random.Range(-100, 100);
  
            StartCoroutine(jump(rotate, size, duration, version, texty, xlocal, ylocal));
        }
    }
    private IEnumerator jump(float r, float s, float d, int v, int t, int x, int y)
    {
        FindObjectOfType<AudioManager>().Play("jump1");
        if (texty == 1) { text.GetComponent<TMP_Text>().SetText("i love you"); }
        if (texty > 1 && texty <= 20) { text.GetComponent<TMP_Text>().SetText("Skip!"); }
        if (texty > 20 && texty <= 50) { text.GetComponent<TMP_Text>().SetText("Hop!"); }
        if (texty > 50) { text.GetComponent<TMP_Text>().SetText("Jump!"); }
        if (colory == 1) { text.GetComponent<TMP_Text>().color = Color.blue; }
        else if(colory == 2) { text.GetComponent<TMP_Text>().color = Color.red; }
        else if(colory == 3) { text.GetComponent<TMP_Text>().color = Color.yellow; }
        else if(colory == 4) { text.GetComponent<TMP_Text>().color = Color.green; }
        else if(colory == 5) { text.GetComponent<TMP_Text>().color = Color.white; }
        else if(colory == 6) { text.GetComponent<TMP_Text>().color = Color.cyan; }
        text.transform.position = new Vector3(xlocal, ylocal, 0);
        text.transform.rotation = new Quaternion(1, 1, 1, r);
        text.transform.localScale = new Vector3(s, s, 1);
        //text.GetComponent<TextMeshPro>().fontSize = s;
        //text.GetComponent<TextMeshPro>().font = Resources.GetBuiltinResource(typeof(TMP_FontAsset), "AttackGraffiti-3zRBM.ttf") as TMP_FontAsset;
        text.GetComponent<TMP_Text>().font = fonts[v];
        //stay on screen for d seconds
        text.SetActive(true);
        yield return new WaitForSeconds(d);
        text.SetActive(false);
        
    }
}
