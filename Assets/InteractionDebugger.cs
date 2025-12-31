using UnityEngine;

public class InteractionDebugger : MonoBehaviour
{
    public void LogClick()
    {
        Debug.Log("Success! The button was clicked in VR.");
    }

    public void LogHover()
    {
        Debug.Log("The VR laser is pointing at the button.");
    }
}