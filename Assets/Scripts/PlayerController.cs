using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IExplosionListener {
	public GameObject bombPrefab; 
	public float speed = 3.0f;
	public int power = 1;
	public int bombNbr = 1;

	public int id = -1;

	private Animator anim;
	private Rigidbody2D rigidBody;

	private bool facingLeft;
	private bool facingRight;

	private bool isWalking;

	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	public void OnTriggerEnter2D(Collider2D other) {
		print (other.tag);
		if (other.CompareTag ("Explosion")) {
			killPlayer ();
		} else if (other.CompareTag ("PowerUp")) {
			PowerupController puc = other.GetComponent<PowerupController> ();
			switch (puc.powerType) {
			case PowerupController.PowerType.BOMB:
				bombNbr++;
				break;
			case PowerupController.PowerType.POWER:
				power++;
				break;
			case PowerupController.PowerType.SPEED:
				speed += 1.0f;
				break;
			}
			puc.DestroyGo ();
		}
	}

	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		if (Input.GetKey ("s")) {
			anim.Play ("WalkFrontAnimation");
			isWalking = true;
		} else if (Input.GetKey ("z")) {
			anim.Play ("WalkBottomAnimation");
			isWalking = true;
		} else if (Input.GetKey ("d")) {
			anim.Play ("WalkSideAnimation");

			if (facingLeft) {
				facingRight = true;
				facingLeft = false;
				anim.transform.Rotate (0, 180, 0);
			} else if (!facingLeft) {
				facingRight = true;
			}
			isWalking = true;
		} else if (Input.GetKey ("q")) {
			anim.Play ("WalkSideAnimation");

			if (!facingRight && !facingLeft) {
				facingLeft = true;
				anim.transform.Rotate (0, -180, 0);
			} else {
				if (facingRight) {
					facingRight = false;
					facingLeft = true;
					anim.transform.Rotate (0, 180, 0);
				} else if (!facingRight) {
					facingLeft = true;
				}
			}
			isWalking = true;
		}

		rigidBody.velocity = isWalking ? new Vector2 (h * speed, v * speed) : Vector2.zero;
	}

	void Update() {
		if (Input.GetKeyUp ("s")) {
			anim.Play ("IdleAnimation");
			isWalking = false;
		}if (Input.GetKeyUp ("z")) {
			anim.Play ("IdleBottomAnimation");
			isWalking = false;
		} else if (Input.GetKeyUp ("d")) {
			anim.Play ("IdleSideAnimation");
			isWalking = false;
		} else if (Input.GetKeyUp ("q")) {
			anim.Play ("IdleSideAnimation");
			isWalking = false;
		}


		if (bombNbr > 0 && Input.GetKeyDown ("space")) {
			bombNbr--;
			DropBomb ();
		}
	}

	void DropBomb() {
		Vector3Int cellpos = GameManager.Instance.mapController.GetCellPosition(transform.position);
		Vector3 cellCenterPos = GameManager.Instance.mapController.GetCellCenter (cellpos);

		GameObject bomb = Instantiate (bombPrefab, cellCenterPos, Quaternion.identity);
		BombController bc = bomb.GetComponent<BombController> ();
		bc.explosionPower = power;
		bc.playerId = id;
		bc.explosionListener = this;

	}

	void DestroyGo() {
		Destroy (gameObject);
	}

	void killPlayer() {
		anim.Play ("DeadAnimation");
	}

	#region IExplosionListener implementation
	public void OnExplode () {
		bombNbr++;
	}
	#endregion
}
