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
    }
    
    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void PacmanEaten()
    {
            Invoke(nameof(ResetState), 0.0f); 
    }

    public void PelletEaten(Pellet pellet)
    {
        SetScore(this.score + pellet.points);
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
