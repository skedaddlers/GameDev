using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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

    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }

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

    public void ToggleMessageHistory(){
        messageHistory.SetActive(!messageHistory.activeSelf);
        isMessageHistoryOpen = messageHistory.activeSelf;
    }


}
