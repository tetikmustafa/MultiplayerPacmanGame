using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviourPunCallbacks
{
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public Rigidbody2D rigidbody1 { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }
    PhotonView pw;

    private void Awake()
    {
        this.rigidbody1 = GetComponent<Rigidbody2D>();
        this.pw = GetComponent<PhotonView>();
        this.startingPosition = this.transform.position;
    }
    private void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.gameObject.GetComponent<Transform>().SetPositionAndRotation(startingPosition,Quaternion.identity);
        //this.transform.position = this.startingPosition;
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        //this.rigidbody1.isKinematic = false;
        this.enabled = true;
    }
    private void Update()
    {
        if(this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }
    private void FixedUpdate()
    {
        Vector2 position = this.rigidbody1.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
        this.rigidbody1.MovePosition(position + translation);
    }
    public void SetDirection(Vector2 direction,bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction=direction;
            this.nextDirection=Vector2.zero;
        }
        else { this.nextDirection=direction;}
    }
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        return hit.collider != null;
    }
}
