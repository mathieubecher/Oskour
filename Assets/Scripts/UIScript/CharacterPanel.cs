using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public Slider ox;
    public Slider st;
    public Slider hg;
    public Button bt;
    public CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        Button btn = bt.GetComponent<Button>();
        btn.onClick.AddListener(fonctionDeBuildDeMathieux);
    }

    void fonctionDeBuildDeMathieux()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            cc.manager.ResetSelect();
        }
        cc.manager.Select(cc);
    }

    // Update is called once per frame
    void Update()
    {
        ox.value = cc.oxygen;
        st.value = cc.energy;
        hg.value = cc.food;
    }
}
