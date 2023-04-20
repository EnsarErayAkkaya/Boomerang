using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private float beforeThrowOffset;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private GameObject collisionParticle;


    private int multiplier = 1;
    private Rigidbody2D rb;
    private float speed;
    private Vector3 _velocity;
    private GameObject instance;

    public Collider2D Collider => collider;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (isAutomatic)
        {
            speed = 5;

            ThrowBoomerang(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-.5f, .5f)).normalized);
        }
        else
        {
            collider.enabled = false;
        }
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
        else if (!collision.collider.CompareTag("Player"))
            ReflectProjectile(collision.contacts[0].normal);

        if (collision.transform.CompareTag("Wall"))
        {
            OnCollidedWithWall(collision.contacts[0].point);
        }
    }


    private void OnCollidedWithWall(Vector2 contactPoint)
    {
        instance = null;

        instance = Instantiate(collisionParticle, contactPoint, Quaternion.identity);

        instance.transform.position = contactPoint;

        instance.GetComponent<ParticleSystem>().Clear();

        instance.GetComponent<ParticleSystem>().Play();

        Destroy(instance, 4f);
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
