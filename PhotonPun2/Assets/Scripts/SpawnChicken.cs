using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnChicken : MonoBehaviour
{
    [SerializeField] GameObject chickenPrefab;
    [SerializeField] float spawnInterval = 2.5f;
    [SerializeField] Transform[] spawnPoints;
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("Spawn" , 0 , spawnInterval);
        }
    }
    void Spawn()
    {
        int random = Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate(chickenPrefab.name , spawnPoints[random].position , Quaternion.identity);
    }
}
