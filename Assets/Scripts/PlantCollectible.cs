using UnityEngine;

public class PlantCollectible : MonoBehaviour
{
    [SerializeField] private string plantType; 

    private void OnTriggerEnter(Collider other)
    {
       
        Debug.Log("Something touched the plant! It was named: " + other.gameObject.name + " with Tag: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            if (PlantTracker.Instance != null)
            {
                PlantTracker.Instance.CollectPlant(plantType);
                Destroy(gameObject); 
            }
            else
            {
                Debug.LogError("PlantTracker instance is missing from the scene!");
            }
        }
    }
}
