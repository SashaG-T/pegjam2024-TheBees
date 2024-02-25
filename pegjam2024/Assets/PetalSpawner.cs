using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                                new Color(1f, 0.9373f, 0.2588f),
                                new Color(0.2667f, 0.25882f, 1f),
                                new Color(0.6196f, 0.9922f, 1f)
    };

    [SerializeField]
    GameObject petal;
    [SerializeField]
    Transform parentTransform;
    [SerializeField]
    private int numberOfPetals = 1;
    [SerializeField]
    private AudioSource petalCollect;

    List<GameObject> petals = new();


    /// <summary>
    /// Spawns flower petals in even intervals around the flowers core, each flower is assigned a random color for the petals
    /// </summary>
    void Start()
    {
        int j = 0;
        if (numberOfPetals > 8)
        {
            numberOfPetals = 8;
        }
        int difFromEight = 8 - numberOfPetals;
        List<int> petalsToSkip = new List<int>();
        while (j < difFromEight)
        {
            bool isUniqueIndex = true;
            int petalToSkipIndex = Random.Range(0, 7);
            foreach(int i in petalsToSkip)
            {
                if (i == petalToSkipIndex)
                {
                    isUniqueIndex = false;
                    continue;
                }
            }
            if (!isUniqueIndex) { continue; }
            petalsToSkip.Add(petalToSkipIndex);
            j++;
        }
        int colorIndex = Random.Range(0, colors.Length);
        for (int i = 0; i < 8; i++) 
        {
            bool isSkipableIndex = false;
            foreach(int p in petalsToSkip)
            {
                if (p == i)
                {
                    isSkipableIndex = true;
                    break;
                }
            }
            if(isSkipableIndex) { continue; }

            GameObject newPetal = Instantiate(petal, parentTransform);
            newPetal.transform.localEulerAngles = new Vector3(90 , 0, 0);
            newPetal.transform.Rotate(0, (360 / 8) * i, 0);
            newPetal.gameObject.GetComponent<Renderer>().material.color = colors[colorIndex];
            newPetal.transform.localPosition = Vector3.zero;
            petals.Add(newPetal);
        }
    }

    public bool hasPetals()
    {
        return petals.Count > 0;
    }

    public void removePetal()
    {
        if(petals.Count > 0)
        {
            petals[0].SetActive(false);
            petals.RemoveAt(0);
            petalCollect.Play();
        }
    }
}
