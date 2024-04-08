using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private bool isMenuOpen = false;
    
    [Header("Health UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpSliderText;

    [Header("Message UI")]
    [SerializeField] private int sameMessageCount = 5;
    [SerializeField] private string lastMessage;
    [SerializeField] private bool isMessageHistoryOpen = false;
    [SerializeField] private GameObject messageHistory;
    [SerializeField] private GameObject messageHistoryContent;
    [SerializeField] private GameObject lastFiveMessagesContent;

    [Header("Inventory UI")]
    [SerializeField] private bool isInventoryOpen = false;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventoryContent;

    [Header("Drop Menu UI")]
    [SerializeField] private bool isDropMenuOpen = false;
    [SerializeField] private GameObject dropMenu;
    [SerializeField] private GameObject dropMenuContent;

    [Header("Skills UI")]
    [SerializeField] private GameObject skills;
    [SerializeField] private GameObject skillsContent;

    [Header("Enemy HP Bar")]
    [SerializeField] private Dictionary<Actor, Slider> enemyHpSliders = new Dictionary<Actor, Slider>();

    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }
    public bool IsMenuOpen { get => isMenuOpen; }

    public static UIManager Instance;
    // Start is called before the first frame update
    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Start() => AddMessage("Welcome to the game!", "#FFFFFF");

     public void AddMessage(string newMessage, string colorHex) {
        if (lastMessage == newMessage) {
            TextMeshProUGUI messageHistoryLastChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lastFiveHistoryLastChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            messageHistoryLastChild.text = $"{newMessage} (x{++sameMessageCount})";
            lastFiveHistoryLastChild.text = $"{newMessage} (x{sameMessageCount})";
            return;
        } else if (sameMessageCount > 0) {
            sameMessageCount = 0;
        }

        lastMessage = newMessage;

        TextMeshProUGUI messagePrefab = Instantiate(Resources.Load<TextMeshProUGUI>("Message")) as TextMeshProUGUI;
        messagePrefab.text = newMessage;
        messagePrefab.color = GetColorFromHex(colorHex);
        messagePrefab.transform.SetParent(messageHistoryContent.transform, false);

        for (int i = 0; i < lastFiveMessagesContent.transform.childCount; i++) {
            if (messageHistoryContent.transform.childCount - 1 < i) {
                return;
            }

            TextMeshProUGUI lastFiveHistoryChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1 - i).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI messageHistoryChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount - 1 - i).GetComponent<TextMeshProUGUI>();
            lastFiveHistoryChild.text = messageHistoryChild.text;
            lastFiveHistoryChild.color = messageHistoryChild.color;
        }
    }

    private Color GetColorFromHex(string v) {
        Color color;
        if (ColorUtility.TryParseHtmlString(v, out color)) {
            return color;
        } else {
            Debug.Log("GetColorFromHex: Could not parse color from string");
            return Color.white;
        }
    }

    public void SetHealthMax(int MaxHp){
        hpSlider.maxValue = MaxHp;
    }

    public void SetHealth(int hp, int maxHp){
        hpSlider.value = hp;
        hpSliderText.text = $"HP: {hp}/{maxHp}";
    }

    public void ToggleMenu(Actor actor = null){
        if(isMenuOpen)
        {
            isMenuOpen = !isMenuOpen;

            if(isMessageHistoryOpen){
                ToggleMessageHistory();
            }
            else if(isInventoryOpen){
                ToggleInventory();
            }
            else if(isDropMenuOpen){
                ToggleDropMenu();
            }
        }
    }

    public void ToggleMessageHistory(){
        messageHistory.SetActive(!messageHistory.activeSelf);
        isMessageHistoryOpen = messageHistory.activeSelf;
        skills.SetActive(!skills.activeSelf);
    }

    public void ToggleInventory(Actor actor = null){
        inventory.SetActive(!inventory.activeSelf);
        isMenuOpen = inventory.activeSelf;
        isInventoryOpen = inventory.activeSelf;

        if(isMenuOpen){
            UpdateMenu(actor, inventoryContent);
            // skills.SetActive(false);
        }
    }

    public void ToggleDropMenu(Actor actor = null){
        dropMenu.SetActive(!dropMenu.activeSelf);
        isMenuOpen = dropMenu.activeSelf;
        isDropMenuOpen = dropMenu.activeSelf;

        if(isMenuOpen){
            UpdateMenu(actor, dropMenuContent);
            // skills.SetActive(false);
        }
    }

    private void UpdateMenu(Actor actor, GameObject menuContent) {
        for (int i = 0; i < menuContent.transform.childCount; i++) {
            GameObject menuContentChild = menuContent.transform.GetChild(i).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            menuContentChild.GetComponent<Button>().onClick.RemoveAllListeners();
            menuContentChild.SetActive(false);
        }

        char c = 'a';
        for (int i = 0; i < actor.Inventory.Items.Count; i++) {
            GameObject menuContentChild = menuContent.transform.GetChild(i).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"({c++}) {actor.Inventory.Items[i].name}";
            menuContentChild.GetComponent<Button>().onClick.AddListener(() => {
                if (menuContent == inventoryContent) 
                {
                    Debug.Log("Use item");
                    Action.UseAction(actor, i - 1);
                } else if (menuContent == dropMenuContent) 
                {
                    Debug.Log("Drop item");
                    Action.DropAction(actor, actor.Inventory.Items[i - 1]);
                }
                UpdateMenu(actor, menuContent);
            });
            menuContentChild.SetActive(true);
        }
        eventSystem.SetSelectedGameObject(menuContent.transform.GetChild(0).gameObject);
    }

    public void UpdateSkills(Actor actor){
        if(skills.activeSelf){
            for(int i = 0; i < skillsContent.transform.childCount; i++){
                GameObject skill = skillsContent.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                skill.GetComponent<Button>().onClick.RemoveAllListeners();
                skill.SetActive(false);
            }

            for(int i = 0; i < 6; i++){
                GameObject skill = skillsContent.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text ="";
                skill.GetComponent<Button>().onClick.AddListener(() => {
                    if(skill == skillsContent.transform.GetChild(0).gameObject)
                        Debug.Log("Using skill: 0");
                    else if(skill == skillsContent.transform.GetChild(1).gameObject)
                        Debug.Log("Using skill: 1");
                    else if(skill == skillsContent.transform.GetChild(2).gameObject)
                        Debug.Log("Using skill: 2");
                    else if(skill == skillsContent.transform.GetChild(3).gameObject)
                        Debug.Log("Using skill: 3");
                    else if(skill == skillsContent.transform.GetChild(4).gameObject)
                        Debug.Log("Using skill: 4");
                    else if(skill == skillsContent.transform.GetChild(5).gameObject)
                        Debug.Log("Using skill: 5");
                });
                skill.SetActive(true);
            }
        }
    }

    public void UpdateEnemyHealthBar(Actor enemy){
        if (!enemy.IsAlive) {
            if (enemyHpSliders.ContainsKey(enemy)) {
                Destroy(enemyHpSliders[enemy].gameObject);
                enemyHpSliders.Remove(enemy);
            }
            return;
        }

        if(enemyHpSliders.ContainsKey(enemy)){
            enemyHpSliders[enemy].value = enemy.GetComponent<Fighter>().Hp;
            enemyHpSliders[enemy].transform.position = GetHealthBarPosition(enemy.GetComponent<Fighter>().transform.position); // Set position above the enemy
            // Debug.Log(enemyHpSliders[enemy].transform.position);
        }
        else{
            Slider enemyHpSlider = Instantiate(Resources.Load<Slider>("EnemyHpSlider")) as Slider;
            enemyHpSlider.transform.SetParent(GameObject.Find("Canvas").transform, false);
            enemyHpSlider.maxValue = enemy.GetComponent<Fighter>().MaxHp;
            enemyHpSlider.value = enemy.GetComponent<Fighter>().Hp;
            enemyHpSlider.transform.position = GetHealthBarPosition(enemy.GetComponent<Fighter>().transform.position); // Set position above the enemy
            enemyHpSliders.Add(enemy, enemyHpSlider);
        }    
    }


    public Vector3 GetHealthBarPosition(Vector3 position){
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        return new Vector3(position.x, position.y + 1, 0);
    }
}
