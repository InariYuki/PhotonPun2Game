using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    private void Awake()
    {
        InitPlayer();
    }
    void InitPlayer()
    {
        Vector3 pos = new Vector3(Random.Range(-5f , 5f) , 5f , 0);
        PhotonNetwork.Instantiate(playerPrefab.name , pos , Quaternion.identity);
    }
}
