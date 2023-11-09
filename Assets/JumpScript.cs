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
    private float colory;
    private float size;
    private float duration;
    private int version;
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
            size = UnityEngine.Random.Range(0.2f, 1f);
            duration = UnityEngine.Random.Range(1.5f, 4f);
            version = UnityEngine.Random.Range(0, 17);
            colory = UnityEngine.Random.Range(1, 6);

            StartCoroutine(jump(rotate, size, duration, version));
        }
    }
    private IEnumerator jump(float r, float s, float d, int v)
    {
        if (colory == 1) { text.GetComponent<TextMeshPro>().color = Color.blue; }
        if (colory == 2) { text.GetComponent<TextMeshPro>().color = Color.red; }
        if (colory == 3) { text.GetComponent<TextMeshPro>().color = Color.yellow; }
        if (colory == 4) { text.GetComponent<TextMeshPro>().color = Color.green; }
        if (colory == 5) { text.GetComponent<TextMeshPro>().color = Color.white; }
        if (colory == 6) { text.GetComponent<TextMeshPro>().color = Color.cyan; }
        text.transform.rotation = new Quaternion(1, 1, 1, r);
        text.transform.localScale = new Vector3(s, s, 1);
        //text.GetComponent<TextMeshPro>().fontSize = s;
        //text.GetComponent<TextMeshPro>().font = Resources.GetBuiltinResource(typeof(TMP_FontAsset), "AttackGraffiti-3zRBM.ttf") as TMP_FontAsset;
        text.GetComponent<TMP_Text>().font = fonts[v];
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
