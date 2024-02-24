using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalSpawner : MonoBehaviour
{
    private Color[] colors = { new Color(0.611f, 0.1568f, 0.1568f, 1), 
                                new Color(1, 0.2588f, 0.2588f, 1), 
                                new Color(1, 0.6196f, 0.6196f), 
                                new Color(0.40784f, 1f, 0.2588f),
                                new Color(1f, 0.6627f, 0.2588f),
                                new Color(0.6196f, 0.9921f, 1f),
                                new Color(0.6275f, 0.6196f, 1f),
                                new Color(1f, 0.9373f, 0.2588f)
    };

    [SerializeField]
    GameObject petal;
    [SerializeField]
    Transform parentTransform;
    [SerializeField]
    private int numberOfPetals = 1;

    /// <summary>
    /// Spawns flower petals in even intervals around the flowers core, each flower is assigned a random color for the petals
    /// </summary>
    void Start()
    {
        int colorIndex = Random.Range(0, colors.Length);
        for (int i = 0; i < numberOfPetals; i++) 
        {
            GameObject newPetal = Instantiate(petal, parentTransform);
            newPetal.transform.localEulerAngles = new Vector3(90 , 0, 0);
            newPetal.transform.Rotate(0, (360 / numberOfPetals) * i, 0);
            newPetal.gameObject.GetComponent<Renderer>().material.color = colors[colorIndex];
            newPetal.transform.localPosition = Vector3.zero;

        }
    }
}
