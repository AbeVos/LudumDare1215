using UnityEngine;
using System.Collections;

public class MoveBG : MonoBehaviour
{
    [SerializeField]
    private Material mat;
    [SerializeField]
    private float speed=1;
    private float counter;

	void Start ()
    {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        mat.mainTextureOffset = new Vector2(counter += Time.deltaTime*speed, 0);
	}
}
