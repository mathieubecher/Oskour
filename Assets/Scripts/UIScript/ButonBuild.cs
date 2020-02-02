using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButonBuild : MonoBehaviour
{
    private BuildController bc;
    public Overlay overlay;
    private GameObject over;
    public Button button;

    void Start()
    {
       /* Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(fonctionDeBuildDeMathieux);
        Text t = this.transform.GetChild(0).GetComponent<Text>();
            t.text = bc.name;*/
    }

    public void AssociateBc()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(fonctionDeBuildDeMathieux);
        Text t = this.transform.GetChild(0).GetComponent<Text>();
        t.text = bc.name;
    }

    public void SetBuildController(BuildController b)
    {
        if (b == null)
        {
            Debug.Log("Null BC");
        }
        this.bc = b;
    }

    void fonctionDeBuildDeMathieux()
    {
        FindObjectOfType<CameraPointer>().PlaceBuilding(bc);
    }
 
    private void OnMouseOver()
    {
        over.SetActive(true);
    }
    void OnMouseExit()
    {
        over.SetActive(false);
    }
}