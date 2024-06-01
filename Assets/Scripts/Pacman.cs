using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent (typeof(Movement))]
public class Pacman : MonoBehaviourPunCallbacks
{
    PhotonView pw;
    public Movement movement { get; private set; }
    private void Awake()
    {
        this.pw = GetComponent<PhotonView> ();
        this.movement = GetComponent<Movement>();
    }
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
            float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
            this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
    }
    [PunRPC]
    public void ResetPacman()
    {
        //this.gameObject.SetActive(true);
        ResetState();
    }

    [PunRPC]
    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
    [PunRPC]
    public void Active()
    {
        this.gameObject.SetActive(true);
    }
    [PunRPC]
    public void ResetState()
    {
        //this.gameObject.GetComponent<Movement>().ResetState();
        this.movement.ResetState();

    }
}
