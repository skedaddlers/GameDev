using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button continueButton;
    void Start()
    {
        if(SaveManager.Instance.HasSaveAvailable()){
            // continueButton.interactable = true;
            eventSystem.SetSelectedGameObject(continueButton.gameObject);
        }
        else{
            continueButton.interactable = false;
        }
    }

    public void NewGame(){
        if(SaveManager.Instance.HasSaveAvailable()){
            SaveManager.Instance.DeleteSave();
        }

        SaveManager.Instance.CurrentFloor = 1;
        SceneManager.LoadScene("Dungeon");
    }

    public void ContinueGame(){
        SaveManager.Instance.LoadGame();
    }

    public void QuitGame(){
        Application.Quit();
    }
}
