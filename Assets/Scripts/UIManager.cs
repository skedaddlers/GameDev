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
    [SerializeField] private GameObject playerUI;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpSliderText;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private TextMeshProUGUI manaSliderText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expSliderText;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TextMeshProUGUI moraText;

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
    // [SerializeField] private GameObject skillPanel;
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

    [Header("Shop UI")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject shopMenuContentSkill;
    [SerializeField] private GameObject shopMenuContentWeapon;
    [SerializeField] private bool isShopMenuOpen = false;

    [Header("Slash UI")]
    [SerializeField] private GameObject slashSprite;
    [SerializeField] private bool isSlashActive = false;
    [SerializeField] private float slashDuration = 0.2f;
    [SerializeField] private float slashTimer = 0f;

    [Header("Victory/Defeat UI")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;



    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }
    public bool IsMenuOpen { get => isMenuOpen; }
    public bool IsEscapeMenuOpen { get => isEscapeMenuOpen; }
    public bool IsPlayerInformationMenuOpen { get => isPlayerInformationMenuOpen; }
    public bool IsLevelUpMenuOpen { get => isLevelUpMenuOpen; }
    public bool IsShopMenuOpen { get => isShopMenuOpen; }

    public static UIManager Instance;
    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Start() => AddMessage("Welcome to the game!", "#FFFFFF");

    public void Update(){
        if(isSlashActive){
            slashTimer += Time.deltaTime;
            if(slashTimer >= slashDuration){
                slashSprite.SetActive(false);
                slashTimer = 0f;
            }
        }
    }

    public void DrawFanSprite(Vector3 position, Vector3 direction, float fanAngle, float area){
        slashSprite.SetActive(!slashSprite.activeSelf);
        isSlashActive = true;
        
        Vector3 placingPosition = new Vector3(position.x, position.y, 0) + direction.normalized;
        slashSprite.transform.position = placingPosition;
        
        slashSprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg) ;
        slashSprite.transform.localScale = new Vector3(18 * area, 18 * area, 0);
    }

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

    public void SetMora(int mora){
        moraText.text = $"{mora}";
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
                case bool _ when isShopMenuOpen:
                    ToggleShopMenu(null);
                    break;
                default:
                    break;
            }
        }
    }

    public void ToggleMessageHistory(){
        messageHistory.SetActive(!messageHistory.activeSelf);
        isMessageHistoryOpen = messageHistory.activeSelf;
        // skills.SetActive(!skills.activeSelf);
        // playerUI.SetActive(!playerUI.activeSelf);
        // skillPanel.SetActive(!skillPanel.activeSelf);
    }

    public void ToggleInventory(Actor actor = null){
        inventory.SetActive(!inventory.activeSelf);
        isMenuOpen = inventory.activeSelf;
        isInventoryOpen = inventory.activeSelf;

        if(isMenuOpen){
            UpdateMenu(actor, inventoryContent);
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
            $"Strength (+2 Power, from {fighter.Power} -> {fighter.Power + 2})",
            $"Resistance (+2 Defense, from {fighter.Defense} -> {fighter.Defense + 2})",
            $"Agility (+1 Movement Speed, from {fighter.MovementSpeed} -> {fighter.MovementSpeed + 1})",
            $"Fortune (+1 Luck, from {actor.GetComponent<Player>().Luck} -> {actor.GetComponent<Player>().Luck + 1})",
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

    public void ToggleShopMenu(Seller seller){
        shopMenu.SetActive(!shopMenu.activeSelf);
        isMenuOpen = shopMenu.activeSelf;
        isShopMenuOpen = shopMenu.activeSelf;

        if(isMenuOpen){
            DisplayShopMenuContent(seller);
        }
    }

    private void DisplayShopMenuContent(Seller seller){
        if(seller != null){
            for(int i = 0; i < shopMenuContentSkill.transform.childCount; i++){
                GameObject skill = shopMenuContentSkill.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                skill.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                
                skill.GetComponent<Button>().onClick.RemoveAllListeners();
                skill.SetActive(false);
            }

            for(int i = 0; i < shopMenuContentSkill.transform.childCount; i++){
                int index = i;
                GameObject skill = shopMenuContentSkill.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{seller.SkillsForSale[i].SkillName} - {seller.SkillsForSale[i].Cost} mora";
                skill.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{seller.SkillsForSale[i].Description}";
                if(seller.GetSoldOutSkill(index)){
                    skill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sold Out!";
                }
                else{
                    skill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                }
                skill.GetComponent<Button>().onClick.AddListener(() => {
                    if(skill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text == "Sold Out!"){
                        AddMessage("This skill is sold out!", "#FF0000");
                        return;
                    }
                    Actor player = GameManager.Instance.Actors[0];
                    if(player.GetComponent<Player>().Mora < seller.SkillsForSale[index].Cost){
                        AddMessage("You don't have enough mora!", "#FF0000");
                        return;
                    }
                    foreach(Skill skill1 in SkillManager.Instance.Skills){
                        if(skill1.SkillName == seller.SkillsForSale[index].SkillName){
                            AddMessage("You already have this skill!", "#FF0000");
                            return;
                        }
                    }
                    Debug.Log($"Buying skill {index}");
                    Action.BuySkill(player, seller.SkillsForSale[index]);
                    DisplayShopMenuContent(seller);
                    seller.SetSoldOutSkill(index, true);
                    // disable the skill button
                    skill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sold Out!";
                });
                SpriteRenderer spriteRenderer = seller.SkillsForSale[i].GetComponent<SpriteRenderer>();
                Sprite sprite = spriteRenderer.sprite;
                skill.GetComponent<Image>().sprite = sprite;
                skill.SetActive(true);
            }

            for(int i = 0; i < shopMenuContentWeapon.transform.childCount; i++){
                GameObject weapon = shopMenuContentWeapon.transform.GetChild(i).gameObject;
                weapon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                weapon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                weapon.GetComponent<Button>().onClick.RemoveAllListeners();
                weapon.SetActive(false);
            }

            for(int i = 0; i < shopMenuContentWeapon.transform.childCount; i++){
                int index = i;
                GameObject weapon = shopMenuContentWeapon.transform.GetChild(i).gameObject;
                weapon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{seller.WeaponsForSale[i].WeaponName} - {seller.WeaponsForSale[i].Cost} mora";
                weapon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{seller.WeaponsForSale[i].Description}";
                if(seller.GetSoldOutWeapon(index)){
                    weapon.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sold Out!";
                }
                else{
                    weapon.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                }
                weapon.GetComponent<Button>().onClick.AddListener(() => {
                    if(weapon.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text == "Sold Out!"){
                        AddMessage("This weapon is sold out!", "#FF0000");
                        return;
                    }
                    Actor player = GameManager.Instance.Actors[0];
                    if(player.GetComponent<Player>().Mora < seller.WeaponsForSale[index].Cost){
                        AddMessage("You don't have enough mora!", "#FF0000");
                        return;
                    }
                    if(player.GetComponent<Inventory>().Weapon != null){
                        if(player.GetComponent<Inventory>().Weapon.WeaponName == seller.WeaponsForSale[index].WeaponName){
                            AddMessage("You already have this weapon equipped!", "#FF0000");
                            return;
                        }
                    }
                    Debug.Log($"Buying weapon {index}");
                    Action.BuyWeapon(player, seller.WeaponsForSale[index]);
                    DisplayShopMenuContent(seller);
                    // disable the weapon button
                    seller.SetSoldOutWeapon(index, true);
                    weapon.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sold Out!";
                });
                SpriteRenderer spriteRenderer = seller.WeaponsForSale[i].GetComponent<SpriteRenderer>();
                Sprite sprite = spriteRenderer.sprite;
                weapon.GetComponent<Image>().sprite = sprite;
                weapon.SetActive(true);
            }

        }
        eventSystem.SetSelectedGameObject(shopMenuContentSkill.transform.GetChild(0).gameObject);
    }

    private void ApplyLevelUp(int level, int choiceIndex, Actor actor){
        Fighter fighter = actor.GetComponent<Fighter>();
        Player player = actor.GetComponent<Player>();

        switch(choiceIndex){
            case 0:
                fighter.MaxHp += 5;
                fighter.Hp += 5;
                AddMessage($"You are blessed with more healthiness by The Tsaritsa!", "#32E9F1");
                break;
            case 1:
                fighter.Power += 2;
                AddMessage($"You are blessed with more strength by Murata!", "#F14A32");
                break;
            case 2:
                fighter.Defense += 2;
                AddMessage($"You are blessed with more resistance by Morax!", "#F1B432");
                break;
            case 3:
                fighter.MovementSpeed += 1;
                AddMessage($"You are blessed with more agility by Barbatos!", "#32F1A3");
                break;
            case 4:
                player.Luck += 1;
                AddMessage($"You are blessed with more fortune by Buer!", "#8CF132");
                break;
            case 5:
                player.MaxMana += 10;
                player.Mana = player.MaxMana;
                AddMessage($"You are blessed with more sustainibility by Beelzebul!", "#CA32F1");
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

    public void UpdateWeapon(Actor player, Weapon wp = null){
        if(isMenuOpen){
            weaponImage.gameObject.SetActive(false);
        }
        else{
            weaponImage.gameObject.SetActive(true);
        }
        if(player.GetComponent<Inventory>().Weapon != null){
            weaponImage.GetComponent<Image>().sprite = player.GetComponent<Inventory>().Weapon.GetComponent<SpriteRenderer>().sprite;
        }
        else{
            if(wp != null){
                weaponImage.GetComponent<Image>().sprite = wp.GetComponent<SpriteRenderer>().sprite;
            }
            else
            weaponImage.GetComponent<Image>().sprite = defaultWeaponSprite;
        }
    }

    public void UpdateSkills(Actor actor, List<Skill> skillList){
        if(skills.activeSelf){
            for(int i = 0; i < skillsContent.transform.childCount; i++){
                GameObject skill = skillsContent.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                skill.GetComponent<Button>().onClick.RemoveAllListeners();
                skill.SetActive(false);
            }

            for(int i = 0; i < skillList.Count; i++){
                int index = i;
                GameObject skill = skillsContent.transform.GetChild(i).gameObject;
                skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                skill.GetComponent<Button>().onClick.AddListener(() => {
                    Debug.Log($"Using skill {index}");
                    SkillManager.Instance.UseSkill(index);
                    UpdateSkills(actor, skillList);
                });
                SpriteRenderer spriteRenderer = skillList[i].GetComponent<SpriteRenderer>();
                Sprite sprite = spriteRenderer.sprite;
                skill.GetComponent<Image>().sprite = sprite;
                skill.SetActive(true);
            }
        }
        eventSystem.SetSelectedGameObject(skillsContent.transform.GetChild(0).gameObject);
    }

    public void UpdateCooldown(int index, float cooldown){
        GameObject skill = skillsContent.transform.GetChild(index).gameObject;
        int cooldownInt = Mathf.CeilToInt(cooldown);
        if(cooldownInt <= 0){
            skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            return;
        }
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

        if(skills.GetComponent<RectTransform>().rect.Contains(position)){
            return true;
        }
        return false;
    }

    public void ShowVictoryScreen(){
        victoryScreen.SetActive(true);
    }

    public void ShowDefeatScreen(){
        defeatScreen.SetActive(true);
    }
}