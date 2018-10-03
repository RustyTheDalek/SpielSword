using UnityEngine;

/// <summary>
/// Tracks Minion 'parts' helpful with minions that break into bits on death
/// </summary>
public class MinionPartTimeObject : RigidbodyTimeObject
{
    Transform parent;

    Vector3 startPos;

    Collider2D m_Collider;

    protected override void Awake()
    {
        base.Awake();

        m_Collider = GetComponent<Collider2D>();

        parent = transform.parent;
        startPos = transform.localPosition;

        OnStartPlayback += ResetObject;
        OnStartReverse += OnPartStartReverse;

        tObjectState = TimeObjectState.Present;
    }

    private void ResetObject()
    {
        bFrames.Clear();
        sFrames.Clear();

        tObjectState = TimeObjectState.Present;

        finishFrame = 0;

        m_Rigidbody2D.bodyType = RigidbodyType2D.Static;

        m_Collider.enabled = false;
        transform.SetParent(parent);
        transform.localPosition = startPos;

        gameObject.layer = LayerMask.NameToLayer("Minion");

        m_Sprite.enabled = true;
    }

    public void Throw(Vector2 direction)
    {
        m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

        Vector2 throwforce = (new Vector2(
                direction.x * Random.Range(2f, 10f),
                direction.y * Random.Range(1f, 5f)) * (m_Rigidbody2D.mass * m_Rigidbody2D.mass));

        Debug.DrawRay(transform.position, throwforce, Color.red, 5f);

        m_Rigidbody2D.AddForce(throwforce, ForceMode2D.Impulse);
        m_Collider.enabled = true;
        transform.SetParent(null);

        gameObject.layer = LayerMask.NameToLayer("Bits");
    }

    protected void OnPartStartReverse()
    {
        transform.SetParent(null);
    }

    private void OnDestroy()
    {
        OnStartPlayback -= ResetObject;
    }
}
