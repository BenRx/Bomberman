using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	public GameObject grid;
	public List<GameObject> players = new List<GameObject> ();

	public List<PlayerController> playerControllers = new List<PlayerController> ();
	public MapController mapController;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		mapController = grid.GetComponent<MapController> ();

		PreparePlayers ();
	}

	void PreparePlayers() {
		int id = 0;
		players.ForEach (delegate(GameObject player) {
			Vector3 startPos = new Vector3(-0.3f, 0, 0);

			switch (id) {
			case 0:
				startPos = new Vector3(-0.3f, 0, 0);
				break;
			case 1:
				startPos = new Vector3(7.7f, -8, 0);
				break;
			case 2:
				startPos = new Vector3(-0.3f, -8, 0);
				break;
			case 3:
				startPos = new Vector3(7.7f, 0, 0);
				break;
			}

			GameObject p = Instantiate(player, startPos, Quaternion.identity);
			PlayerController pc = p.GetComponentInChildren<PlayerController>();
			pc.id = id;

			playerControllers.Add(pc);
			id++;
		});
	}
}
