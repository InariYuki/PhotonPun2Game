using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 3.5f;
    [SerializeField] float jump = 30f;
    [SerializeField] Vector3 groundOffset;
    [SerializeField] Vector3 groundSize;
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;
    TextMeshProUGUI chickenNumDisplay;
    bool isGround;
    int chickenCount , winCount = 3;
    CanvasGroup playerCanvas;
    Animator anim;
    TextMeshProUGUI winnerDispley;
    Button returnButton;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (!photonView.IsMine) enabled = false;
        photonView.RPC("RPCUpdateName", RpcTarget.All);
        chickenNumDisplay = transform.Find("PlayerCanvas/ChickenNum").GetComponent<TextMeshProUGUI>();
        playerCanvas = GameObject.Find("GameUI").GetComponent<CanvasGroup>();
        winnerDispley = GameObject.Find("Winner").GetComponent<TextMeshProUGUI>();
        returnButton = GameObject.Find("Back").GetComponent<Button>();
        returnButton.onClick.AddListener(() => {
            if (photonView.IsMine)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("¥D­n");
            }
        });
    }
    private void Start()
    {
        GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
    }
    private void Update()
    {
        Move();
        CheckGround();
        Jump();
        DetectPosition();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
        Gizmos.DrawCube(transform.position + groundOffset , groundSize);
    }
    void Move()
    {
        float input = Input.GetAxis("Horizontal");
        if(input != 0)
        {
            anim.SetBool("boolMove", true);
        }
        else
        {
            anim.SetBool("boolMove", false);
        }
        if(input > 0)
        {
            sprite.flipX = false;
        }
        else if(input < 0)
        {
            sprite.flipX = true;
        }
        rigidBody.velocity = new Vector2(input * speed , rigidBody.velocity.y);
    }
    void CheckGround()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize , 0);
        isGround = hit;
    }
    void Jump()
    {
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(new Vector2(0 , jump));
        }
    }
    [PunRPC]
    void RPCUpdateName()
    {
        transform.Find("PlayerCanvas/NameDisplay").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Chicken"))
        {
            Destroy(collision.gameObject);
            chickenNumDisplay.text = (++chickenCount).ToString();
            if(chickenCount >= winCount)
            {
                Win();
            }
        }
    }
    void Win()
    {
        playerCanvas.alpha = 1;
        playerCanvas.interactable = true;
        playerCanvas.blocksRaycasts = true;
        winnerDispley.text = "Winner:" + photonView.Owner.NickName;
        DestroyChickens();
    }
    void DestroyChickens()
    {
        Destroy(FindObjectOfType<SpawnChicken>().gameObject);
        GameObject[] chickens = GameObject.FindGameObjectsWithTag("G");
        for(int i = 0; i < chickens.Length; i++)
        {
            Destroy(chickens[i]);
        }
    }
    void DetectPosition()
    {
        if(transform.position.y < -15f)
        {
            transform.position = new Vector3(0 , 1.5f , 0);
            rigidBody.velocity = Vector3.zero;
        }
    }
}
