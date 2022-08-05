using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
	[SerializeField] private bool _controlMidair = true;
	[SerializeField] private LayerMask _walkableLayer;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private Transform _ceilingCheck;

	const float _groundCheckRadius = .2f;
	private bool _grounded;
	private Rigidbody2D _rigidBody;
	private bool _facingRight = true;
	private Vector3 _currentVelocity = Vector3.zero;
	private Animator _anim;

	[Header("Events")]
	[Space]

	public UnityEvent OnLanding;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
		
		if (OnLanding == null)
			OnLanding = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _grounded;
		_grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundCheckRadius, _walkableLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLanding.Invoke();
					_anim.SetTrigger("grounded");
			}
		}
	}


	public void Move(float move, bool jump)
	{

		if (_grounded || _controlMidair)
		{

			Vector3 targetVelocity = new Vector2(move * 10f, _rigidBody.velocity.y);

			_rigidBody.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);

			if (move > 0 && !_facingRight)
			{

				Flip();
			}
			else if (move < 0 && _facingRight)
			{
				Flip();
			}
		}
		if (_grounded && jump)
		{
			_grounded = false;
			_anim.SetTrigger("jumped");
			_rigidBody.AddForce(new Vector2(0f, _jumpForce));
		}
	}

	private void Flip()
	{
		_facingRight = !_facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}