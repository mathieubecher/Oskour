using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonTiersFolder : MonoBehaviour
{
    public Button button;
    
    public ButonBuild b;
    public float height;
    public float weight;
    private List <GameObject> bb = new List<GameObject>();
    public float margY = 10;
    public float margX = 40;
    public int tier;
    private bool active = false;
    public float taille;
    public PapaBoutton pb;

    void Start()
    {
        BuildController[] bc = ((GameManager)FindObjectOfType<GameManager>()).Build;
        int l = 0;
        foreach (BuildController c in bc)
            if (c.tier == tier)
                l++;
        /*float*/ taille = ((l - 1) * (height+margY))/2;
        int i = 0;
        foreach (BuildController c in bc)
        {
            if (c.tier == tier)
            {
                b.SetBuildController(c);
                b.AssociateBc();
                GameObject go = GameObject.Instantiate(b.gameObject);
                go.SetActive(false);
                go.GetComponent<RectTransform>().anchoredPosition = button.GetComponent<RectTransform>().anchoredPosition;
                go.GetComponent<RectTransform>().position = new Vector2(margX, taille - i * height - i * margY);
                go.GetComponent<RectTransform>().SetParent(button.GetComponent<RectTransform>(), false);
                bb.Add(go);
                ++i;
            }
        }
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void inactivate()
    {
        foreach (GameObject go in bb)
        {
            go.SetActive(false);
        }
        active = false;
    }

    void TaskOnClick()
    {
        active = !active;
        pb.noticePapa(this);
        foreach (GameObject go in bb)
        {
            go.SetActive(active);
        }
    }
}
