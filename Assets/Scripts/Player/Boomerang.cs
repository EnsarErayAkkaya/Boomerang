using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed;
    private int multiplier = 1;
    public float beforeThrowOffset;

    private Rigidbody2D rb;

    private Vector3 _velocity;

    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void SetBoomerang(BoomerangDetail detail)
    {
        spriteRenderer.sprite = detail.boomerangSprite;
        speed = detail.speed;
        transform.localScale = detail.scale;
    }
    
    public void ThrowBoomerang(Vector2 _velocity)
    {
        this._velocity = _velocity * speed;
        rb.velocity = this._velocity;
    }
    public void SetSpeed(int multiplier)
    {
        this.multiplier = multiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BoomerangController cc = collision.transform.GetComponent<BoomerangController>();
        if (cc != null && !cc.hasBumerang)
        {
            cc.GrabBoomerang();
        }
        else
            ReflectProjectile(collision.contacts[0].normal);
    }

    private void ReflectProjectile(Vector2 reflectVector)
    {
        _velocity = Vector2.Reflect(_velocity, reflectVector);
        rb.velocity = _velocity * multiplier;
    }
    public void SetBoomerangPosBeforeShooting(Vector2 player, Vector2 dir)
    {
        transform.position = player + (dir * beforeThrowOffset);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-45);
    }
}
