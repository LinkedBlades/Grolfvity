using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingSprite : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Color lerpedColor = Color.gray;
    private float pulsingChange;

    [Header("Scale same as transform")] 
    [SerializeField] float _scale;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = lerpedColor;
    }

    // Update is called once per frame
    void Update()
    {
        lerpedColor = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0.3f), new Color(0.5f, 0.5f, 0.5f, 0.6f), Mathf.Abs(Mathf.Sin(Time.time)));
        spriteRenderer.color = lerpedColor;

        pulsingChange = Mathf.Sin(Time.deltaTime);
        transform.localScale = new Vector3(_scale + pulsingChange * 0.15f, _scale + pulsingChange * 0.15f, 1);

    }
}
