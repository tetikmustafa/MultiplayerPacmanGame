using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class GhostEyes : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Movement movement { get; private set; }
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    PhotonView pw;
    private void Awake()
    {
        this.pw = GetComponent<PhotonView>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
       if(this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = this.up;
        } else if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = this.down;
        } else if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = this.left;
        } else if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = this.right;
        }
    }
}
