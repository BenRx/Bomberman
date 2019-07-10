using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestructionController : MonoBehaviour {
	
	private static MapDestructionController instance;
	public static MapDestructionController Instance { get { return instance; } }

	public Tilemap tilemap;
	public Tile wallTile;
	public Tile destructableTile;
	public GameObject explosion;

	public void Explode(Vector2 worldPos) {
		Vector3Int oriCell = tilemap.WorldToCell (worldPos);
		initExplosion (oriCell, 1);
	}

	void initExplosion(Vector3Int oriCell, int power) {
		ExplodeCell (oriCell);
		for (int i = 1; i != power + 1; ++i) {
			if (!ExplodeCell (oriCell + new Vector3Int(i, 0, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!ExplodeCell (oriCell + new Vector3Int (0, i, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!ExplodeCell (oriCell + new Vector3Int (-i, 0, 0))) {
				break;
			}
		}
		for (int i = 1; i != power + 1; ++i) {
			if (!ExplodeCell (oriCell + new Vector3Int (0, -i, 0))) {
				break;
			}
		}
	}

	bool ExplodeCell(Vector3Int cellPos) {
		Tile tile = tilemap.GetTile<Tile> (cellPos);

		if (tile == wallTile) {
			return false;
		} else if (tile == destructableTile) {
			tilemap.SetTile (cellPos, null);
		}
		Vector3 centerCellPos = tilemap.GetCellCenterWorld (cellPos);

		Instantiate (explosion, centerCellPos, Quaternion.identity);
		return true;
	}

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}
}
