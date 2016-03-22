using UnityEngine;

public class GemLogicClick : MonoBehaviour
{
    void OnMouseDown()
    {
        GemLogic gl = GetComponentInParent<GemLogic>();
        gl.GemClick();
    }
    void OnMouseDrag()
    {
        GemLogic gl = GetComponentInParent<GemLogic>();
        gl.GemDrag();
    }
}