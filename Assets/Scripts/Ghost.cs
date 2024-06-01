using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

public class Ghost : MonoBehaviourPunCallbacks
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehaviour initialBehavior;
    public Transform target;
    PhotonView pw;

    public int points = 200;
    private void Update()
    {
        if (pw.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.movement.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.movement.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.movement.SetDirection(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.movement.SetDirection(Vector2.left);
            }
        }
    }
    public void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.frightened = GetComponent<GhostFrightened>();
        this.pw = GetComponent<PhotonView>();
    }
    private void Start()
    {
        ResetState();
    }
    [PunRPC]
    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
    [PunRPC]
    public void ResetGhost()
    {
        ResetState();
    }
    [PunRPC]
    public void GetFrightened(float duration)
    {
        this.frightened.Enable(duration);
    }
    public void ResetState()
    {
        this.gameObject.SetActive(true);
        //this.gameObject.GetComponent<Movement>().ResetState();
        this.movement.ResetState();
        //this.gameObject.GetComponent<GhostFrightened>().Disable();
        this.frightened.Disable();
        if(this.home != this.initialBehavior)
        {
            //this.gameObject.GetComponent<GhostHome>().Disable();
            this.home.Disable();
        }
        if(this.initialBehavior != null)
        {
            //this.gameObject.GetComponent<GhostBehaviour>().Enable();
            this.initialBehavior.Enable();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();

            }
        }
    }
}
