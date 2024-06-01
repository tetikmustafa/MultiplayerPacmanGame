using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    PhotonView pw;
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;
    public int ghostMultiplier { get; private set; } = 1; 
    public int score { get; private set; }
    public int lives { get; private set; }
    public static int ghostCount = 0;
    private void Start()
    {
        this.pw = GetComponent<PhotonView>();
        //NewGame();
    }
    private void Update()
    {
        if (this.lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }
    [PunRPC]
    public void AssignPacman(Pacman pacman)
    {
        this.pacman = pacman;
    }
    [PunRPC]
    public void AddGhost(Ghost ghost)
    {
        this.ghosts[ghostCount] = ghost;
        ghostCount++;
    }

    [PunRPC]
    public void StartNewGame()
    {
        NewGame();
    }
    [PunRPC]
    public void WaitforPlayers()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    private void NewGame()
    {
        SetScore(0);
        SetLives(1);
        NewRound();
    }
    private void NewRound()
    {
        gameOverText.enabled = false;
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    private void ResetState()
    {
        /*for (int i = 0; i < ghosts.Length; i++)
        {
            Debug.Log("Hata Kontrol:" + i);
            this.ghosts[i].ResetState();
        }*/
        if (GameObject.Find("Ghost_Blinky(Clone)") != null)
        {
            GameObject.Find("Ghost_Blinky(Clone)").GetComponent<PhotonView>().RPC("ResetGhost", RpcTarget.All, null);

        }
        if (GameObject.Find("Ghost_Inky(Clone)") != null)
        {
            GameObject.Find("Ghost_Inky(Clone)").GetComponent<PhotonView>().RPC("ResetGhost", RpcTarget.All, null);

        }
        if (GameObject.Find("Ghost_Clyde(Clone)") != null)
        {
            GameObject.Find("Ghost_Clyde(Clone)").GetComponent<PhotonView>().RPC("ResetGhost", RpcTarget.All, null);

        }
        //Debug.Log(GameObject.Find("Pacman(Clone)").GetComponent<PhotonView>().GetInstanceID());
        GameObject.Find("Pacman(Clone)").GetComponent<PhotonView>().RPC("ResetPacman", RpcTarget.All, null);
        //this.pacman.ResetState(); 
    }
    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);
    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');

    }
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }
    public void PacmanEaten()
    {
        //this.pacman.gameObject.SetActive(false);
        GameObject.Find("Pacman(Clone)").GetComponent<PhotonView>().RPC("Deactive", RpcTarget.All, null);
        SetLives(this.lives-1);
        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }
    public void PowerPelletEaten(PowerPellet pellet)
    {
        /*for(int i = 0;i< this.ghosts.Length;i++)
        {
            this.ghosts[i].gameObject.GetComponent<GhostFrightened>().Enable(pellet.duration);
            //this.ghosts[i].frightened.Enable(pellet.duration);
        }*/
        if (GameObject.Find("Ghost_Blinky(Clone)") != null)
        {
            GameObject.Find("Ghost_Blinky(Clone)").GetComponent<PhotonView>().RPC("GetFrightened", RpcTarget.All, pellet.duration);

        }
        if (GameObject.Find("Ghost_Inky(Clone)") != null)
        {
            GameObject.Find("Ghost_Inky(Clone)").GetComponent<PhotonView>().RPC("GetFrightened", RpcTarget.All, pellet.duration);

        }
        if (GameObject.Find("Ghost_Clyde(Clone)") != null)
        {
            GameObject.Find("Ghost_Clyde(Clone)").GetComponent<PhotonView>().RPC("GetFrightened", RpcTarget.All, pellet.duration);

        }
        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
