using System.Collections;
using OdinSerializer;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] private int currentFloor = 0;
    [SerializeField] private string saveFileName = "saveThe.Furina";
    [SerializeField] private SaveData save = new SaveData();
    public int CurrentFloor { get => currentFloor; set => currentFloor = value; }
    public SaveData Save { get => save; set => save = value; }

    private void Awake()
    {
        if(SaveManager.Instance == null)
        {
            SaveManager.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public bool HasSaveAvailable(){
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if(File.Exists(path)){
            return true;
        }
        return false;
    }

    public void SaveGame(){
        save.SavedFloor = currentFloor;

        bool hasScene = save.Scenes.Find(x => x.FloorNumber == currentFloor) is not null;
        if(hasScene){
            UpdateScene(SaveState());
        }
        else{
            AddScene(SaveState());
        }

        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        byte[] saveJson = SerializationUtility.SerializeValue(save, DataFormat.JSON);
        File.WriteAllBytes(path, saveJson);
    }

    public void LoadGame() {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        byte[] saveJson = File.ReadAllBytes(path); //Load the state from the file
        save = SerializationUtility.DeserializeValue<SaveData>(saveJson, DataFormat.JSON); //Deserialize the state from JSON

        currentFloor = save.SavedFloor;

        if (SceneManager.GetActiveScene().name is not "Dungeon") {
            SceneManager.LoadScene("Dungeon");
        } else {
            SceneState sceneState = save.Scenes.Find(x => x.FloorNumber == currentFloor);
            if (sceneState is not null) {
                LoadState(sceneState);
            } else {
                Debug.LogError("No save data for this floor");
            }
        }
    }

    public void DeleteSave() {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.Delete(path);
    }

    public void AddScene(SceneState sceneState) => save.Scenes.Add(sceneState);

    public void UpdateScene(SceneState sceneState) => save.Scenes[currentFloor - 1] = sceneState;

    public SceneState SaveState() => new SceneState(
        currentFloor,
        GameManager.Instance.SaveState(),
        MapManager.Instance.SaveState()
    );

    public void LoadState(SceneState sceneState) {
        MapManager.Instance.LoadState(sceneState.MapState);
        GameManager.Instance.LoadState(sceneState.GameState);
    }
}

[System.Serializable]
public class SaveData {
    [SerializeField] private int savedFloor;

    [SerializeField] private List<SceneState> scenes;

    public int SavedFloor { get => savedFloor; set => savedFloor = value; }
    public List<SceneState> Scenes { get => scenes; set => scenes = value; }

    public SaveData() {
        savedFloor = 0;
        scenes = new List<SceneState>();
    }
}

[System.Serializable]
public class SceneState {
    [SerializeField] private int floorNumber;
    [SerializeField] private GameState gameState;
    [SerializeField] private MapState mapState;
    public int FloorNumber { get => floorNumber; set => floorNumber = value; }
    public GameState GameState { get => gameState; set => gameState = value; }
    public MapState MapState { get => mapState; set => mapState = value; }

    public SceneState(int floorNumber, GameState gameState, MapState mapState) {
        this.floorNumber = floorNumber;
        this.gameState = gameState;
        this.mapState = mapState;
    }
}

