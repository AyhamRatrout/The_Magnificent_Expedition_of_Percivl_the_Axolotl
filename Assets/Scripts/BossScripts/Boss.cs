using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

	public Transform player;
	public bool _facingLeft = true;

	[SerializeField] public float movementDelay = 1;
	[SerializeField] public float speed = 2;
	private bool canMove = true;


    void Start()
    {
        
    }

    void Update()
    {
		LookAtPlayer();
		MovePerDelay();
		StartCoroutine(DisableMovementTemporarily());
	}



    public void LookAtPlayer()
	{
		if (transform.position.x < player.position.x && _facingLeft)
		{

			Flip();
		}
		else if (transform.position.x > player.position.x && !_facingLeft)
		{
			Flip();
		}
	}

	private void Flip()
	{
		_facingLeft = !_facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void MovePerDelay()
    {
		if (canMove)
        {
			Vector2 target = new Vector2(player.position.x, player.position.y);
			Vector2 newPos = Vector2.MoveTowards(this.GetComponent<Rigidbody2D>().position, target, speed * Time.fixedDeltaTime);
			this.GetComponent<Rigidbody2D>().MovePosition(newPos);
		}

	}

	private IEnumerator DisableMovementTemporarily()
	{
		if (canMove)
        {
			yield return new WaitForSeconds(movementDelay);
			canMove = false;
		}
		else if (!canMove)
        {
			yield return new WaitForSeconds(movementDelay/2);
			canMove = true;
		}
	}


}
