using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStar : MonoBehaviour
{
    private Vector2 dir;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 0.1f;
    public float minSize = 0.1f;
    public float maxSize = 0.3f;
    public float sizeSpeed = 1f;
    public float colorSpeed = 1f;
    public Color[] colors;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dir = new Vector2(Random.Range(-1f, -1f), Random.Range(-1f, -1f));
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(size, size);
        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * moveSpeed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime);

        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(spriteRenderer.color.a, 0, Time.deltaTime * colorSpeed);
        spriteRenderer.color = color;

        if (spriteRenderer.color.a <= 0.01f)
            Destroy(gameObject);
    }
}
