using UnityEngine;

public class CharaMoveS : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    Animator anim;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public Vector2 getAxis()
    {
        return movement;
    }
    // Update is called once per frame
    void Update()
    {
        if (GlobalVarS.Instance.isPlayerStartChat)
        {
            movement = Vector2.zero;
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
