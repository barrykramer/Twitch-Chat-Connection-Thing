using UnityEngine;

public class Food : MonoBehaviour
{
    public int rotSpeed = 15;

    public GameObject parent;
    public GameObject[] food;
    private int currentFood = 0;

    void Update()
    {
        // bananas ROTAT E
        this.transform.Rotate(rotSpeed * -1.5f * Time.deltaTime, 0, 0);
        this.transform.Rotate(0, rotSpeed * 5 * Time.deltaTime, 0);
        this.transform.Rotate(0, 0, rotSpeed * 0.2f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            rotSpeed += 50;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            rotSpeed -= 50;
        }

        FoodSwitcher();
    }

    void FoodSwitcher()
    {
        // Next food in list
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentFood < food.Length - 1)
            {
                food[currentFood].SetActive(false);
                food[currentFood + 1].SetActive(true);
                currentFood++;
            }
            else
            {
                food[currentFood].SetActive(false);
                food[0].SetActive(true);
                currentFood = 0;
            }          
        }

        // Previous food in list
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentFood == 0)
            {
                food[currentFood].SetActive(false);
                food[food.Length - 1].SetActive(true);
                currentFood = food.Length - 1;
            }
            else
            {
                food[currentFood].SetActive(false);
                food[currentFood - 1].SetActive(true);
                currentFood--;
            }
        }

        // Toggle food on/off
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            parent.SetActive(!parent.activeInHierarchy);
        }
    }
}
