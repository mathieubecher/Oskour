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
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(fonctionDeBuildDeMathieux);
        
    }

    public void SetBuildController(BuildController b)
    {
        this.bc = b;
    }

    void fonctionDeBuildDeMathieux()
    {

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