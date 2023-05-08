using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider thrustSlider;

    public static UIController Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void SetThrustSlider(float value)
    {
        thrustSlider.value = value;
    }

}
