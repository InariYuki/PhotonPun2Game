using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 3.5f;
    [SerializeField] float jump = 30f;
    [SerializeField] Vector3 groundOffset;
    [SerializeField] Vector3 groundSize;
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;
    bool isGround;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        if (!photonView.IsMine) enabled = false;
        photonView.RPC("RPCUpdateName", RpcTarget.All);
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
        Gizmos.DrawCube(transform.position + groundOffset , groundSize);
    }
    void Move()
    {
        float input = Input.GetAxis("Horizontal");
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
            PhotonNetwork.Destroy(collision.gameObject);
        }
    }
}
