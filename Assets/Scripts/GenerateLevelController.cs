using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevelController : MonoBehaviour
{
    public List<Material> materials = new List<Material>();

    public List<Ropes> ropesList = new List<Ropes>();
    // Start is called before the first frame update
    void Start()
    {
        _upRopeLevels();
    }

    private void _upRopeLevels()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("CurrentLevel")-10; i++)
        {
            int random=Random.Range(0, 25);
            if (ropesList[random].ropeColliderScript.ropeLevel<7)
            {
                ropesList[random].meshRenderer.material = materials[ropesList[random].ropeColliderScript.ropeLevel];
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}

[System.Serializable]
public class Ropes
{
    public RopeColliderScript ropeColliderScript;
    public MeshRenderer meshRenderer;
}

