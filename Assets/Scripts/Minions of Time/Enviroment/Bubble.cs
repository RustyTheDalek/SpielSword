using UnityEngine;

public class Bubble : PlayerGrapple
{
    [Tooltip("Offset when attached to object")]
    public Vector3 offset;

    [Tooltip("Force of inital 'Bubbling'")]
    [Range(0, 10000)]
    public float bubbleForce;

    [Tooltip("Force of Bubbles constant lift")]
    [Range(0, 100)]
    public float liftForce;

    ConstantForce2D targetFloatForce, 
                    m_FloatForce;

    bool followingVillager = true;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        m_FloatForce = GetComponent<ConstantForce2D>();
  	}

    protected override void LateUpdate()
    {
        if (target && followingVillager)
        {
            transform.position = target.transform.position + offset;

            target.GetComponent<Rigidbody2D>().velocity = new Vector2(
                target.GetComponent<Rigidbody2D>().velocity.x * .75f,
                target.GetComponent<Rigidbody2D>().velocity.y);
        }

        base.LateUpdate();
    }

    protected override void Attack()
    {
        base.Attack();

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        targetFloatForce = target.gameObject.AddComponent<ConstantForce2D>();
        targetFloatForce.force = new Vector2(0, liftForce);
        m_FloatForce.force = new Vector2(0, liftForce);
        m_FloatForce.enabled = true;

        target.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(0, 1) * Time.deltaTime * bubbleForce,
            ForceMode2D.Impulse);
    }

    /// <summary>
    /// Depart from target
    /// </summary>
    public void Depart()
    {
        Destroy(targetFloatForce);
        followingVillager = false;
    }

    protected override void OnPlayerEscape()
    {
        base.OnPlayerEscape();
    }
}
