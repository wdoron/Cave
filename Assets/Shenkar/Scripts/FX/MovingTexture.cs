using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingTexture : MonoBehaviour
{
    private Renderer rndr;
    public Vector2 offsetPerSec;
    public string textureId = "_MainTex";
    // Start is called before the first frame update
    void Awake()
    {
        rndr = GetComponent<Renderer>();
    }

    //private void OnDrawGizmosSelected() {
    //    if (rndr == null) Awake();
    //    Update();
    //} 

    void Update()
    {

        var offset = rndr.sharedMaterial.GetTextureOffset(textureId);
        rndr.sharedMaterial.SetTextureOffset(textureId, offset + offsetPerSec * Time.deltaTime);
    }
}
