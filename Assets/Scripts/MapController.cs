using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour {
	public Tilemap tilemap;
	public Tile wallTile;
	public Tile destructableTile;

	public enum TileType {
		DESTRUCTABLE,
		NOT_DESTRUCTABLE,
		OTHER
	};

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public TileType GetTileType(Vector3Int cellPos) {
		Tile tile = tilemap.GetTile<Tile> (tilemap.WorldToCell (cellPos));

		if (tile == GameManager.Instance.mapController.GetWallTile ()) {
			return TileType.NOT_DESTRUCTABLE;
		} else if (tile == GameManager.Instance.mapController.GetDestructableTile ()) {
			return TileType.DESTRUCTABLE;
		}
		return TileType.OTHER;
	}

	public void SetTileIntoTilemap(Vector3Int cellpos, Tile tile) {
		tilemap.SetTile (cellpos, tile);
	}

	public Vector3Int GetCellPosition(Vector3 worldPosition) {
		return tilemap.WorldToCell (worldPosition);
	}

	public Vector3 GetCellCenter(Vector3Int cellPosition) {
		return tilemap.GetCellCenterWorld (cellPosition);
	}

	public Tilemap GetTilemap() {
		return tilemap;
	}

	public Tile GetTile(Vector3 cellPos) {
		return tilemap.GetTile<Tile> (tilemap.WorldToCell (cellPos));
	}

	public Tile GetWallTile() {
		return wallTile;
	}

	public Tile GetDestructableTile() {
		return destructableTile;
	}
}
