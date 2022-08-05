using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private LayerMask platformsLayerMask;
	[SerializeField] private LayerMask enemyLayerMask;
	[SerializeField] private GameObject healthUI;
	[SerializeField] public GameObject enemy;
	[SerializeField] public float invincibilityDurationSeconds;

	public float speed = 4.5f;
	public float jump = 3.0f;
	public bool invincible = false;
	public int health = 10;

	private SpriteRenderer _renderer;
	private BoxCollider2D _box;
	private Rigidbody2D _body;
	private Animator _anim;
	

	// Use this for initialization
	void Start()
	{
		_box = GetComponent<BoxCollider2D>();
		_body = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>(); //will use when implementing animations
		_renderer = GetComponent<SpriteRenderer>();
		healthUI.GetComponent<Text>().text = "Health: " + health; //initial health
	}

	// Update is called once per frame
	void Update()
	{

		TakeDamageOnHit();

		//left and right movement
		float deltaX = Input.GetAxis("Horizontal") * speed;
		if (Input.GetAxis("Horizontal") < 0) {
			_renderer.flipX = true;
		} else if (Input.GetAxis("Horizontal") > 0 ) {
			_renderer.flipX = false;
		}
		Vector2 movement = new Vector2(deltaX, _body.velocity.y);
		_body.velocity = movement;

		//jump if on surface
		if (IsGrounded()) 
		{
			if(Input.GetKeyDown(KeyCode.W)) {
				_anim.SetTrigger("jumped");
				_body.velocity = Vector2.up*jump;
			}
			else {
				_anim.SetTrigger("grounded");
			}
		}
	}

    private bool IsGrounded()
    {
		return (Physics2D.BoxCast(_box.bounds.center, _box.bounds.size, 0f, Vector2.down, .1f, platformsLayerMask)).collider != null;
    }

	private void TakeDamageOnHit()
    {
		if (!invincible && _box.IsTouchingLayers(enemyLayerMask))
        {
			health -= 1;
			healthUI.GetComponent<Text>().text = "Health: " + health;
			StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

	private IEnumerator BecomeTemporarilyInvincible()
	{
		Debug.Log("Player turned invincible!");
		invincible = true;
		yield return new WaitForSeconds(invincibilityDurationSeconds);

		invincible = false;
		Debug.Log("Player is no longer invincible!");
	}
}
