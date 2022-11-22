using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public enum Menu
    {
        Main,
        Levels,
        Controls,
        Credits
    };
    public Menu currentMenu;
    public GameObject[] menus;
    public Transform[] buttons;
    public Transform selector;
    public int rowSize;
    int selected;
    public bool allowHorizontal;
    InputMain controls;
    int xInput;
    int yInput;

    void Selected()
    {
        if (currentMenu == Menu.Main)
        {
            OpenMenu(selected + 1);
        }
        else if (currentMenu == Menu.Credits || currentMenu == Menu.Controls)
        {
            OpenMenu(0);
        }
        else if (currentMenu == Menu.Levels)
        {
            if (selected == buttons.Length - 1)
            {
                OpenMenu(0);
            }
            else
            {
                // Call level manager with build index
                GameObject.Find("LevelManger").GetComponent<LevelManager>().OpenLevel(selected + 1);
            }
        }
    }

    public void MouseButtonEnter(int buttonNum)
    {
        selected = buttonNum;
        MoveSelectIcon();
    }

    void MoveSelectIcon()
    {
        selector.position = buttons[selected].transform.position;
        if (currentMenu == Menu.Levels)
        {
            if (selected < buttons.Length - 1)
            {
                selector.GetComponent<Animator>().SetBool("Single", true);
            }
            else
            {
                selector.GetComponent<Animator>().SetBool("Single", false);
            }
        }
        selector.GetComponent<Animator>().SetTrigger("Pop");


    }

    void OpenMenu(int menuIndex)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        menus[menuIndex].SetActive(true);
    }


    //-----------------------------Input Actions Setup----------------------
    void Awake()
    {
        //----------------------Setup input events---------------------------
        controls = new InputMain();
        // Select Press
        controls.Menu.Select.performed += ctx => Selected();
        // Scroll Horizontal
        controls.Menu.HorizontalScroll.performed += ctx => {
            if (allowHorizontal && xInput != (int)Mathf.Sign(ctx.ReadValue<float>())) 
            {
                xInput = (int)Mathf.Sign(ctx.ReadValue<float>());
                selected = ((selected + xInput) % buttons.Length + buttons.Length) % buttons.Length;
                MoveSelectIcon();
            }
        };
        controls.Menu.HorizontalScroll.canceled += ctx => xInput = 0;
        // Scroll Vertical
        controls.Menu.VerticalScroll.performed += ctx => {
            if (yInput != (int)Mathf.Sign(ctx.ReadValue<float>()))
            {
                yInput = -(int)Mathf.Sign(ctx.ReadValue<float>());
                Debug.Log(yInput);
                selected = Mathf.Clamp(selected + yInput * rowSize, 0, buttons.Length - 1);
                MoveSelectIcon();
            }
        };
        controls.Menu.VerticalScroll.canceled += ctx => yInput = 0;
    }
    void OnEnable()
    {
        controls.Menu.Enable();
        MoveSelectIcon();
    }

    void OnDisable()
    {
        controls.Menu.Disable();
    }
}

