using UnityEngine;

/// <summary> A tile on some map. </summary>
[System.Serializable]
public class TileData {
    [SerializeField] private string name;
    public string Name { get => name; set => name = value; }

    public TileData(string name) {
        this.name = name;
    }
}