
using UnityEngine;

public class FishRoam : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    private Vector3 moveSpot;
    [SerializeField] private Transform moveSpot1;
    [SerializeField] private Transform moveSpot2;
    public bool canMove = true;

    SpriteRenderer spriteRenderer;

    private float waitTime;
    public float startWaitTime = 5f;
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        waitTime = startWaitTime;
        moveSpot = moveSpot1.position;
    }

    private void Update()
    {
        if (canMove)
        {
            gameObject.transform.position =
               Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    waitTime = startWaitTime;
                    ToggleMove();
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }
    void ToggleMove()
    {
        if (Vector2.Distance(transform.position, moveSpot1.position) < 0.2f)
        {
            moveSpot = moveSpot2.position;
            spriteRenderer.flipX = true;
        }
        else
        {
            moveSpot = moveSpot1.position;
            spriteRenderer.flipX = false;
        }
    }
}

