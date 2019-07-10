using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	public GameObject explosionPrefab;
	public GameObject powerUpPrefab;

	public float countdown = 3f;
	public int explosionPower = 1;
	public int playerId = -1;

	public IExplosionListener explosionListener;

	private bool exploded = false;

	// Update is called once per frame
	void Update () {
		countdown -= Time.deltaTime;
		if (countdown <= 0) {
			Detonate ();
		}
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (!exploded && other.CompareTag ("Explosion")) {
			CancelInvoke("InitExplosion");
			Detonate ();
		}
	}

	void Detonate() {
		exploded = true;
		explosionListener.OnExplode ();
		InitExplosion (explosionPower);
		Destroy(gameObject);
	}

	void InitExplosion(int power) {
		Vector3Int oriCell = GameManager.Instance.mapController.GetCellPosition (transform.position);
		Explode (oriCell);
		for (int i = 1; i != power + 1; ++i) {
			if (!Explode (oriCell + new Vector3Int(i, 0, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!Explode (oriCell + new Vector3Int (0, i, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!Explode (oriCell + new Vector3Int (-i, 0, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!Explode (oriCell + new Vector3Int (0, -i, 0))) {
				break;
			}
		}
	}

	bool Explode(Vector3Int cellPos) {
		MapController.TileType tileType = GameManager.Instance.mapController.GetTileType (cellPos);

		if (tileType == MapController.TileType.NOT_DESTRUCTABLE) {
			return false;
		} else if (tileType == MapController.TileType.DESTRUCTABLE) {
			int randSpawn = Random.Range (0, 3);

			GameManager.Instance.mapController.SetTileIntoTilemap (cellPos, null);
			if (randSpawn == 1) {
				int randType = Random.Range (0, 3);
				PowerupController.PowerType powerType;
				switch (randType) {
				case 0:
					powerType = PowerupController.PowerType.BOMB;
					break;
				case 1:
					powerType = PowerupController.PowerType.POWER;
					break;
				case 2:
					powerType = PowerupController.PowerType.SPEED;
					break;
				default:
					powerType = PowerupController.PowerType.BOMB;;
					break;
				}
				PowerupController.Create (powerUpPrefab, GameManager.Instance.mapController.GetCellCenter(cellPos), powerType);
			}
		}

		Vector3 centerCellPos = GameManager.Instance.mapController.GetCellCenter (cellPos);
		Instantiate (explosionPrefab, centerCellPos, Quaternion.identity);

		return true;
	}
}
