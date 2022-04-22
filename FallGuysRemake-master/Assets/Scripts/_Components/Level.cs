using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform paintableWall;

    [SerializeField] private Transform spawnPointContainer;
    [SerializeField] private Transform AISpawnPointContainer;
    [SerializeField] private Transform AITargetContainer;
    
    private List<Transform> spawnPositions = new List<Transform>();
    private List<Transform> AISpawnPositions = new List<Transform>();
    private List<Transform> AITargetPositions = new List<Transform>();

    [HideInInspector] public static Level instance = null;

    private List<Transform> AIs = new List<Transform>();
    private Transform player = null;

    private CrownController crown;
    private Transform winner = null;

    private bool start = false;
    private bool finish = false;

    public int finishedPlayers = 0;
    private void Awake()
    {
        if (instance == null) instance = this;

        for (int i = 0; i < spawnPointContainer.childCount; i++) { spawnPositions.Add(spawnPointContainer.GetChild(i)); }
        for (int i = 0; i < AISpawnPointContainer.childCount; i++) { AISpawnPositions.Add(AISpawnPointContainer.GetChild(i)); }
        for (int i = 0; i < AITargetContainer.childCount; i++) { AITargetPositions.Add(AITargetContainer.GetChild(i)); }

    }
    private void Start()
    {
        foreach(Transform tr in AISpawnPositions) { Transform ai = Instantiate(Resources.Load<GameObject>("AI"),transform).transform; ai.position = tr.position; ai.rotation = tr.rotation; AIs.Add(ai); }

        player = Instantiate(Resources.Load<GameObject>("Player"), transform).transform;
        player.position = spawnPositions[0].position;
        player.rotation = spawnPositions[0].rotation;

        crown = Instantiate(Resources.Load<GameObject>("Crown"), transform).GetComponent<CrownController>();

        CameraController.instance.SetTarget(player, player);
        LevelManager.instance.startEvent.AddListener(() => { start = true; });
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (!start || finish) return;

        if(winner == null) { winner = player; }

        float closestDistance = Mathf.Abs(winner.position.z - paintableWall.position.z);
        float playerDistance = Mathf.Abs(player.position.z - paintableWall.position.z);
        int currentPlacement = 1;

        Transform temp = null;
        foreach (Transform tr in AIs)
        {
            float currentDistance = Mathf.Abs(tr.position.z - paintableWall.position.z);
            if (currentDistance < closestDistance)
            {
                temp = tr;
                closestDistance = currentDistance;
            }
            if (currentDistance < playerDistance) currentPlacement++;
        }

        winner = closestDistance > playerDistance ? player : (temp == null ? winner : temp);

        UIManager.instance.gamePanel.placementText.text = currentPlacement.ToString();

        if (finishedPlayers > 0) { crown.gameObject.SetActive(false); return; }

        if (crown.target == null || crown.target != winner)
        {
            crown.target = winner;
        }
    }
    public Transform GetSpawnPoint(Vector3 _position)
    {
        Transform point = null;
        float closestDistance = Mathf.Infinity;
        
        foreach(Transform tr in spawnPositions)
        {
            if (_position.z > tr.position.z && tr.GetComponent<SpawnPointController>().colliders.Count == 0)
            {
                float currentDistance = Vector3.Distance(tr.position, _position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    point = tr;
                }
            }
        }
        return point != null ? point : spawnPositions[0];
    }
    public Transform GetAITarget(Vector3 _position)
    {
        Transform point = null;
        List<Transform> choiceList = new List<Transform>();
        float closestDistance = Mathf.Infinity;

        foreach (Transform tr in AITargetPositions)
        {
            if (_position.z < tr.position.z - 2) 
            {
                float currentDistance = Mathf.Abs(_position.z - tr.position.z);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    point = tr;
                }
            }
        }

        if(point != null) foreach(Transform tr in AITargetPositions) { if (Mathf.Abs(tr.position.z - _position.z) == Mathf.Abs(point.position.z - _position.z)) choiceList.Add(tr); }

        return choiceList.Count != 0 ? choiceList[Random.Range(0,choiceList.Count)] : AITargetPositions[0];
    }
}
