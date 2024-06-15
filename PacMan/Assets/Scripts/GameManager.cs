using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    //public Pacman pacman;
    //De reactivat pacman pentru celelalte functii
    public Transform pellets;
    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(1);
        NewRound();
    }

    private void Update()
    {
        if (this.lives <= 0)
        {
            NewRound();
        }
    }
    private void NewRound()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }

    public void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }
        //pacman.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        //pacman.gameObject.SetActive(false);
    }
    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        SetScore(this.score + (ghost.points * this.ghostMultiplier));
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        //this.pacman.gameObject.SetActive(false);
        /*SetLives(this.lives - 1);
        if (this.lives <= 0)
        {*/
            Invoke(nameof(ResetState), 0.0f); 
            //Putem folosi NewRound pentru a reseta si pozitia punctelor
        /*}*/
        /*else
        {
            GameOver();
        }*/
    }

    public void PelletEaten(Pellet pellet)
    {
        //pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        /*if (!HasRemainingPellets())
        {
            //pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 0.5f);
        }*/
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);
        //CancelInvoke();
        //Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
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
