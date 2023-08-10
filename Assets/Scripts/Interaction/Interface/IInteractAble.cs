using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.Interaction
{
    public interface IInteractAble 
    {
        Vector3 UiOffset { get;}
        
        Vector3 Pos{get;}
    }
}
