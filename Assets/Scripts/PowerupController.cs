using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {

	public enum PowerType {
		BOMB,
		POWER,
		SPEED
	};

	public Sprite powerIcon;
	public Sprite bombIcon;
	public Sprite speedIcon;

	public PowerType powerType;

	private SpriteRenderer spriteRenderer;

	public static void Create(GameObject prefab, Vector3 position, PowerType powerType) {
		GameObject powerup = Instantiate (prefab, position, Quaternion.identity);
		PowerupController controller = powerup.GetComponent<PowerupController> ();

		controller.powerType = powerType;
	}

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		switch (powerType) {
		case PowerType.BOMB:
			spriteRenderer.sprite = bombIcon;
			break;
		case PowerType.POWER:
			spriteRenderer.sprite = powerIcon;
			break;
		case PowerType.SPEED:
			spriteRenderer.sprite = speedIcon;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyGo() {
		Destroy (gameObject);
	}
}
