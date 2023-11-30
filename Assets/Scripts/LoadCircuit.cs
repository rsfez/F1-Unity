using UnityEngine;

public class LoadGP : MonoBehaviour
{
    void Start()
    {
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            DriverController.Create(textAsset.name);
        }
    }
}
