using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class AgentController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform target2;
    [SerializeField] private int pelletCount;
    [SerializeField] private int lowPelletCount;
    [SerializeField] private GameObject pellet;
    [SerializeField] private GameObject lowPellet;
    [SerializeField] private List<GameObject> spawnedPelletsList = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedLowPelletsList = new List<GameObject>();
    [SerializeField] private Transform environmentLocation;
    private List<Vector3> collidedPoints = new List<Vector3>();
    private List<float> distance1 = new List<float>();
    [SerializeField] private GameManager gameManager;
    public Movement movement { get; private set; }
    private float pointsremaining;
    public new Rigidbody2D rigidbody { get; private set; }
    float xv;
    float yv;
    private float previousDistance;
    private float[] xpositions = { -12.5f, -7.5f, -4.5f, -1.5f, 1.5f, 4.5f, 7.5f, 12.5f };
    private float[] ypositions = { -14.5f, -11.5f, -8.5f, -5.5f, 6.5f, 9.5f, 13.5f };
    //private float[] xpositions = { 13.2f , 5.7f , -4.7f};
    //private float[] ypositions = {-12.7f, -9.75f, -5.7f, 1.3f};
    public LayerMask layerMask;
    private void Awake()
    {
        movement = GetComponent<Movement>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /*private void Start()
    {
        previousDistance = Vector3.Distance(transform.position, target.position);
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, target.position);
        if(currentDistance > previousDistance)
        {
            AddReward(-1f/MaxStep);
        }

        if (currentDistance < previousDistance)
        {
            AddReward(1f/MaxStep);
        }
        previousDistance = currentDistance;
    }*/
    private void CreatePellet()
    {
        int counter = 0;
        bool ok;
        
        if (spawnedPelletsList.Count() != 0)
        {
            RemovePellet(spawnedPelletsList);
        }
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject newPellet = Instantiate(pellet);
            newPellet.transform.parent = environmentLocation;
            Vector3 pelletLocation = new Vector3(xpositions[Random.Range(0,xpositions.Length)], ypositions[Random.Range(0,ypositions.Length)], 0f);

            if (spawnedPelletsList.Count != 0)
            {
                for (int j = 0; j < spawnedPelletsList.Count; j++)
                {
                    if (counter < 10)
                    {
                        ok = CheckOverlap(pelletLocation, spawnedPelletsList[j].transform.localPosition, 5f);
                        if (ok == false)
                        {
                            pelletLocation = new Vector3(xpositions[Random.Range(0,xpositions.Length)], ypositions[Random.Range(0,ypositions.Length)], 0f);
                            j--;
                        }
                        counter++;
                    }
                    else
                    {
                        j = spawnedPelletsList.Count;
                    }
                }
            }
            
            newPellet.transform.localPosition = pelletLocation;
            spawnedPelletsList.Add(newPellet);
        }
    }

    private void RemovePellet(List<GameObject> toBeDeleted)
    {
        foreach (GameObject i in toBeDeleted)
        {
            Destroy(i.gameObject);
        }
        toBeDeleted.Clear();
    }

    private bool CheckOverlap(Vector3 objnotOverlapping, Vector3 existingObject, float minimumDistanceBetweenobj)
    {
        float DistanceBetweenObjects = Vector3.Distance(objnotOverlapping, existingObject);
        if (minimumDistanceBetweenobj <= DistanceBetweenObjects)
        {
            return true;
        }

        return false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(target.position);
        sensor.AddObservation(transform.position);
        //sensor.AddObservation(Vector3.Distance(transform.localPosition, target.localPosition));
        sensor.AddObservation(pointsremaining);
        foreach (var i in spawnedPelletsList)
        {
            sensor.AddObservation(i.transform.localPosition);
        }

        /*foreach (var q in spawnedPelletsList)
        {
            sensor.AddObservation(Vector3.Distance(transform.localPosition, q.transform.localPosition));
            //Debug.Log(Vector3.Distance(transform.position, q.transform.position));
        }*/
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.05f, layerMask);
        sensor.AddObservation(hit);
    }
    
    public override void OnEpisodeBegin()
    {
        int randomIndexX = Random.Range(0, xpositions.Length);
        int randomIndexY = Random.Range(0, ypositions.Length);
        transform.localPosition = new Vector3(0f, -8.5f, -5f);
        //transform.localPosition = new Vector3(2.5f, -9.7f, -5f);
        CreatePellet();
        collidedPoints.Clear();
        pointsremaining = 4;
        gameManager.ResetState();
        //target.localPosition = new Vector3(xpositions[Random.Range(0,xpositions.Length)], ypositions[Random.Range(0,ypositions.Length)], 0f);
        //target.localPosition = new Vector3(8.2f, -9.7f, 0f);
        //previousDistance = Vector2.Distance(transform.position, target.position);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        /*discreteAction[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        discreteAction[1] = Mathf.RoundToInt(Input.GetAxis("Vertical"));*/
        var discreteActionsOut = actionsOut.DiscreteActions;
        //discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    public void MoveAgent(int action)
    {
        switch (action)
        {
            case 1:
                movement.SetDirection(Vector3.up);
                break;
            case 2:
                movement.SetDirection(Vector3.down);
                break;
            case 3:
                movement.SetDirection(Vector3.right);
                break;
            case 4:
                movement.SetDirection(Vector3.left);
                break;
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        /*float moveX = actions.DiscreteActions[0];
        float moveY = actions.DiscreteActions[1];
        switch (moveX)
        {
            case 0: xv = 0f; break;
            case 1: xv = -1f;
                break;
            case 2: xv = +1f;
                break;
        }
        switch (moveY)
        {
            case 0: yv = 0f; break;
            case 1: yv = -1f;
                break;
            case 2: yv = +1f;
                break;
        }
        //Debug.Log(new Vector2(xv,yv));
        movement.SetDirection(new Vector2(xv,yv));*/
        int action = actions.DiscreteActions[0];
        MoveAgent(action);
        AddReward(-1f / MaxStep);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pellet"))
        {
            //spawnedPelletsList.Remove(other.gameObject);
            //Destroy(other.gameObject);
            if (!collidedPoints.Contains(other.gameObject.transform.position))
            {
                collidedPoints.Add(other.gameObject.transform.position);
                AddReward(1/pointsremaining);
                pointsremaining -= 1;
                //Debug.Log(pointsremaining);
            }

            //Debug.Log(GetCumulativeReward());
            Vector3 otherpelletloc = FindUncollidedPelletPosition();
            other.gameObject.transform.position = otherpelletloc;
            //Debug.Log(other.gameObject.transform.position);
            //Debug.Log(ok);
            //Debug.Log(collidedPoints);
            if (collidedPoints.Count == 4)
            {
                //RemovePellet(spawnedPelletsList);
                SetReward(10f);
                Invoke("EndEpisodeWithDelay", 0.0f);
            }
            else if (StepCount >= 30000)
            {
                Invoke("EndEpisodeWithDelay", 1f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            AddReward(-5.5f);
            EndEpisode();
        }
    }

    private Vector3 FindUncollidedPelletPosition()
    {
        foreach (var pellet in spawnedPelletsList)
        {
            if (!collidedPoints.Contains(pellet.transform.position))
            {
                return pellet.transform.position;
            }
        }
        return Vector3.zero; 
    }
    private void EndEpisodeWithDelay()
    {
        collidedPoints.Clear();
        EndEpisode();
    }
}
