using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButonBuild : MonoBehaviour
{
    public BuildController bc;
    public Text overlay;
    private GameObject go;
    public Button button;
    public float margX = 70;
    private CameraPointer _cameraPointer;

    void Start()
    {
        _cameraPointer = FindObjectOfType<CameraPointer>();
    }

    public void AssociateBc()
    {
        
        Text t = this.transform.GetChild(0).GetComponent<Text>();
        t.text = bc.name;
        t = this.transform.GetChild(1).GetComponent<Text>();
        t.text = bc.name + "\n\n" + bc.description;
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(fonctionDeBuildDeMathieux);
    }

    public void associateOver()
    {
       
        /*go = GameObject.Instantiate(overlay.gameObject, button.GetComponent<RectTransform>());
        go.GetComponent<RectTransform>().anchoredPosition = button.GetComponent<RectTransform>().anchoredPosition;
        go.GetComponent<RectTransform>().position = new Vector2(margX, 0);*/
        //overlay.gameObject.SetActive(false);
    }
     
    public void SetBuildController(BuildController b)
    {
        if (b == null)
        {
            Debug.Log("Null BC");
        }
        this.bc = b;
    }

    public void fonctionDeBuildDeMathieux()
    {
        _cameraPointer.PlaceBuilding(bc);
        //StartCoroutine(waiter());
        
    }

    private IEnumerator waiter()
    {
        
        yield return new WaitForSecondsRealtime(1);
        _cameraPointer.PlaceBuilding(bc);
    }

    private void OnMouseOver()
    {
        overlay.gameObject.SetActive(true);
    }
    void OnMouseExit()
    {
        //overlay.gameObject.SetActive(false);
    }
}