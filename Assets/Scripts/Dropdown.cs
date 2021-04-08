using UnityEngine;

public class Dropdown : MonoBehaviour
{
    public GameObject[] menus;
    private int currentMenu;
    private int dropdownVal;

    void Start()
    {
        currentMenu = 0;
    }

    public void MenuSwitcher(int val)
    {
        if(val == 0)
        {
            menus[currentMenu].SetActive(false);
            menus[0].SetActive(true);
            currentMenu = 0;
        }
        else if(val == 1)
        {
            menus[currentMenu].SetActive(false);
            menus[1].SetActive(true);
            currentMenu = 1;
        }
        else if (val == 2)
        {
            menus[currentMenu].SetActive(false);
            menus[2].SetActive(true);
            currentMenu = 2;
        }
    }
}
