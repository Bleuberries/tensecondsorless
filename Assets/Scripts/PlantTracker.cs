using System.Collections.Generic;
using UnityEngine;

public class PlantTracker : MonoBehaviour
{
    public static PlantTracker Instance;

    private Dictionary<string, int> plantInventory = new Dictionary<string, int>();
    private int totalCollected = 0;

    [SerializeField] private int plantsNeeded = 5;
    [SerializeField] private float messageDuration = 3f; 

    private string popupMessage = "";
    private float popupTimer = 0f;
    private GUIStyle popupStyle;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectPlant(string type)
    {
        if (plantInventory.ContainsKey(type))
        {
            plantInventory[type]++;
        }
        else
        {
            plantInventory.Add(type, 1);
        }

        totalCollected++;

        popupMessage = "Picked up: " + type + "  (" + totalCollected + "/" + plantsNeeded + ")";
        popupTimer = messageDuration;

        Debug.Log(popupMessage);

        if (totalCollected >= plantsNeeded)
        {
            popupMessage = "You collected all " + plantsNeeded + " plants! Game over!";
            popupTimer = messageDuration;
            Debug.Log("Game over!");
          
        }
    }

    private void Update()
    {
        if (popupTimer > 0f)
        {
            popupTimer -= Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        if (popupTimer <= 0f) return;

        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.label);
            popupStyle.fontSize = 28;
            popupStyle.alignment = TextAnchor.MiddleCenter;
            popupStyle.normal.textColor = Color.white;
        }

        float height = 60f;
        Rect box = new Rect(0, Screen.height - height - 20f, Screen.width, height);
        GUI.Box(box, GUIContent.none);
        GUI.Label(box, popupMessage, popupStyle);
    }
}
