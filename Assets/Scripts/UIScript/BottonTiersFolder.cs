using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonTiersFolder : MonoBehaviour
{
    public Button button;
    public ButonBuild b;
    private List <GameObject> bb;
    public float margY = 10;
    public float margX = 40;
    public int tier;
    private bool active = false;

    void Start()
    {
        BuildController[] bc = ((GameManager)FindObjectOfType<GameManager>()).Build;
        int l = bc.Length;
        float height = b.gameObject.transform.lossyScale.y;
        float weight = b.gameObject.transform.lossyScale.x;
        float taille = (height + margY) * l - margY;
        for (int i = 0; i<l;++i)
        {
            if (bc[i].tier == tier)
            {
                b.SetBuildController(bc[i]);
                bb.Add(GameObject.Instantiate(b.gameObject, new Vector3(margX, i * taille + i * margY), Quaternion.identity, this.gameObject.transform));
                bb[bb.Count - 1].SetActive(false);
            }
        }
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        active = !active;
        foreach (GameObject go in bb)
        {
            go.SetActive(active);
        }
    }
}
