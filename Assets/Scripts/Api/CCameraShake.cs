using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraShake : MonoBehaviour
{
    public float Duration = .1f;
    public Shader shader;
    public Vector2 shakeSize;// = new Vector2(.009f, .009f);

    Vector2 _shake = Vector2.zero;

    float currentTime = 0;
    Material mat;

    bool doit = true;
	// Use this for initialization
	void Start ()
    {
        mat = new Material(shader);	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentTime <= 0)
        {
            Shake();
        }
    }
    
    public void Shake()
    {
        currentTime = 0.5f;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (currentTime > 0)
        {
            currentTime -= Duration;

            mat.SetTexture("_MainTex", source);
            mat.SetVector("_shake", new Vector2(Mathf.Cos(Random.value) * shakeSize.x, Mathf.Sin(Random.value) * shakeSize.y));
            shakeSize *= -1;

            Graphics.Blit(source, destination, mat);
        }
    }
}
