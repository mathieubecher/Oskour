using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapaBoutton : MonoBehaviour
{
    public BottonTiersFolder[] enfant;
    
    public void noticePapa (BottonTiersFolder act)
    {
        foreach (BottonTiersFolder e in enfant)
            if (e != act)
                e.inactivate();
    }
}
