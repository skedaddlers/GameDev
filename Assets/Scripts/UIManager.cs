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
    
    [Header("Player UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpSliderText;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private TextMeshProUGUI manaSliderText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expSliderText;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TextMeshProUGUI levelText;

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
    
    [Header("Escape Menu UI")]
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private bool isEscapeMenuOpen = false;
    
    [Header("Weapon UI")]
    [SerializeField] private GameObject weaponImage;
    [SerializeField] private Sprite defaultWeaponSprite;

    [Header("Player Information UI")]
    [SerializeField] private GameObject playerInformationMenu;
    [SerializeField] private bool isPlayerInformationMenuOpen = false;

    [Header("Level Up UI")]
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private GameObject levelUpMenuContent;
    [SerializeField] private bool isLevelUpMenuOpen = false;


    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }
    public bool IsMenuOpen { get => isMenuOpen; }
    public bool IsEscapeMenuOpen { get => isEscapeMenuOpen; }
    public bool IsPlayerInformationMenuOpen { get => isPlayerInformationMenuOpen; }
    public bool IsLevelUpMenuOpen { get => isLevelUpMenuOpen; }

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

    public void SetManaMax(int MaxMana){
        manaSlider.maxValue = MaxMana;
    }

    public void SetMana(int mana, int maxMana){
        manaSlider.value = mana;
        manaSliderText.text = $"Mana: {mana}/{maxMana}";
    }

    public void SetHealthMax(int MaxHp){
        hpSlider.maxValue = MaxHp;
    }

    public void SetHealth(int hp, int maxHp){
        hpSlider.value = hp;
        hpSliderText.text = $"HP: {hp}/{maxHp}";
    }

    public void SetExpMax(int MaxExp){
        expSlider.maxValue = MaxExp;
    }

    public void SetExp(int exp, int maxExp){
        expSlider.value = exp;
        expSliderText.text = $"Exp: {exp}/{maxExp}";
    }

    public void SetStaminaMax(int MaxStamina){
        staminaSlider.maxValue = MaxStamina;
    }


    public void SetStamina(int stamina, int maxStamina){
        // is stamina is full, then disable the stamina bar
        if(stamina == maxStamina){
            staminaSlider.gameObject.SetActive(false);
        }
        else{
            staminaSlider.gameObject.SetActive(true);
        }
        staminaSlider.value = stamina;
    }

    public void ToggleMenu(){
        if(isMenuOpen)
        {
            isMenuOpen = !isMenuOpen;

            switch (true)
            {
                case bool _ when isMessageHistoryOpen:
                    ToggleMessageHistory();
                    break;
                case bool _ when isInventoryOpen:
                    ToggleInventory();
                    break;
                case bool _ when isDropMenuOpen:
                    ToggleDropMenu();
                    break;
                case bool _ when isEscapeMenuOpen:
                    ToggleEscapeMenu();
                    break;
                case bool _ when isPlayerInformationMenuOpen:
                    TogglePlayerInformationMenu();
                    break;
                default:
                    break;
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

    public void ToggleEscapeMenu(){
        escapeMenu.SetActive(!escapeMenu.activeSelf);
        isMenuOpen = escapeMenu.activeSelf;
        isEscapeMenuOpen = escapeMenu.activeSelf;

        if(isMenuOpen){
            eventSystem.SetSelectedGameObject(escapeMenu.transform.GetChild(0).gameObject);
        }
    }

    public void TogglePlayerInformationMenu(Actor actor = null){
        playerInformationMenu.SetActive(!playerInformationMenu.activeSelf);
        isMenuOpen = playerInformationMenu.activeSelf;
        isPlayerInformationMenuOpen = playerInformationMenu.activeSelf;

        if(actor != null){
            playerInformationMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level: {actor.GetComponent<Player>().Level}";
            playerInformationMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Exp: {actor.GetComponent<Player>().Exp}/{actor.GetComponent<Player>().ExpNeeded}";
            playerInformationMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Power: {actor.GetComponent<Fighter>().Power}";
            playerInformationMenu.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Defense: {actor.GetComponent<Fighter>().Defense}";
            playerInformationMenu.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Shield: {actor.GetComponent<Fighter>().ShieldHp}";
            playerInformationMenu.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = $"Speed: {actor.GetComponent<Fighter>().MovementSpeed}";
            playerInformationMenu.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = $"Luck: {actor.GetComponent<Player>().Luck}";
        }
    }

    public void ToggleLevelUpMenu(Actor actor){
        levelUpMenu.SetActive(!levelUpMenu.activeSelf);
        isMenuOpen = levelUpMenu.activeSelf;
        isLevelUpMenuOpen = levelUpMenu.activeSelf;

        Fighter fighter = actor.GetComponent<Fighter>();
        int level = actor.GetComponent<Player>().Level;

        string[] buttonLabels = {
            $"Constitution (+5 HP, from {fighter.MaxHp} -> {fighter.MaxHp + 5})",
            $"Strength (+1 Power, from {fighter.Power} -> {fighter.Power + 1})",
            $"Resistance (+1 Defense, from {fighter.Defense} -> {fighter.Defense + 1})",
            $"Agility (+1 Movement Speed, from {fighter.MovementSpeed} -> {fighter.MovementSpeed + 1})",
            $"Fortune (+0.1 Luck, from {actor.GetComponent<Player>().Luck} -> {actor.GetComponent<Player>().Luck + 1})",
            $"Sustainibility (+10 Mana, from {actor.GetComponent<Player>().MaxMana} -> {actor.GetComponent<Player>().MaxMana + 10})"
        };

        int buttonIndex = 0;
        foreach(Transform child in levelUpMenuContent.transform){
            TextMeshProUGUI buttonText = child.GetComponent<TextMeshProUGUI>();
            Button button = child.GetComponent<Button>();

            buttonText.text = buttonLabels[buttonIndex];
            button.onClick.RemoveAllListeners();
            int captureIndex = buttonIndex;
            button.onClick.AddListener(() => {
                ApplyLevelUp(level, captureIndex, actor);
            });

            buttonIndex++;
        }
        eventSystem.SetSelectedGameObject(levelUpMenuContent.transform.GetChild(0).gameObject);
    }

    private void ApplyLevelUp(int level, int choiceIndex, Actor actor){
        Fighter fighter = actor.GetComponent<Fighter>();
        Player player = actor.GetComponent<Player>();

        switch(choiceIndex){
            case 0:
                fighter.MaxHp += 5;
                fighter.Hp += 5;
                AddMessage($"The Celestia blesses you with healthiness!", "#00FF00");
                break;
            case 1:
                fighter.Power += 1;
                AddMessage($"The Celestia blesses you with strength!", "#00FF00");
                break;
            case 2:
                fighter.Defense += 1;
                AddMessage($"The Celestia blesses you with resistance!", "#00FF00");
                break;
            case 3:
                fighter.MovementSpeed += 1;
                AddMessage($"The Celestia blesses you with agility!", "#00FF00");
                break;
            case 4:
                player.Luck += 0.1f;
                AddMessage($"The Celestia blesses you with fortune!", "#00FF00");
                break;
            case 5:
                player.MaxMana += 10;
                player.Mana = player.MaxMana;
                AddMessage($"The Celestia blesses you with sustainibility!", "#00FF00");
                break;
        }

        player.LevelUp();

        ToggleLevelUpMenu(actor);
    }

    public void Save(){
        SaveManager.Instance.SaveGame();
    }

    public void Load(){
        SaveManager.Instance.LoadGame();
        ToggleMenu();
    }

    public void Quit(){
        Application.Quit();
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

    public void UpdateWeapon(Actor player){
        if(isMenuOpen){
            weaponImage.SetActive(false);
        }
        else{
            weaponImage.SetActive(true);
        }
        if(player.GetComponent<Inventory>().Weapon != null){
            weaponImage.GetComponent<Image>().sprite = player.GetComponent<Inventory>().Weapon.GetComponent<SpriteRenderer>().sprite;
        }
        else{
            weaponImage.GetComponent<Image>().sprite = defaultWeaponSprite;
        }
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
                    if(skill == skillsContent.transform.GetChild(0).gameObject){
                        actor.GetComponent<Player>().UseSkill(0);
                    }  
                    else if(skill == skillsContent.transform.GetChild(1).gameObject){
                        actor.GetComponent<Player>().UseSkill(1);
                    }
                    else if(skill == skillsContent.transform.GetChild(2).gameObject){
                        actor.GetComponent<Player>().UseSkill(2);
                    }
                    else if(skill == skillsContent.transform.GetChild(3).gameObject){
                        actor.GetComponent<Player>().UseSkill(3);
                    }
                    else if(skill == skillsContent.transform.GetChild(4).gameObject){
                        actor.GetComponent<Player>().UseSkill(4);
                    }
                    else if(skill == skillsContent.transform.GetChild(5).gameObject){
                        actor.GetComponent<Player>().UseSkill(5);
                    }
                });
                skill.SetActive(true);
            }
        }
    }

    public void UpdateCooldown(int index, float cooldown){
        GameObject skill = skillsContent.transform.GetChild(index).gameObject;
        int cooldownInt = Mathf.CeilToInt(cooldown);
        skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{cooldownInt}";
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

    public bool ContainsSkillButton(Vector3 position){
        // Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        // Debug.Log(position);
        if(skills.GetComponent<RectTransform>().rect.Contains(position)){
            return true;
        }
        // for(int i = 0; i < skillsContent.transform.childCount; i++){
        //     GameObject skill = skillsContent.transform.GetChild(i).gameObject;
        //     Debug.Log(skillsContent.GetComponent<RectTransform>().rect);
        //     if(skill.GetComponent<RectTransform>().rect.Contains(position)){
        //         return true;
        //     }
        // }
        return false;
    }
}
