using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private const float ZeroIndexXCoord = -6.5f;
    private const float ZeroIndexYCoord = 1f;
    
    private enum MainMenuState
    {
        StartGame,
        Quit
    }

    [SerializeField] private GameObject cursor;
    private GameObject _cursorObj;

    private MainMenuState _menuState;
    
    // Have it animate via code
    //   hover up and down
    //     david mentioned using a sine wave that is slightly offset between x and y
    //   slightly change size? expand/contract?
    //   Check what minish-cap/link-to-the-past or whatever does and emulate that

    // Start is called before the first frame update
    void Start()
    {
        _cursorObj = Instantiate(cursor);
        _cursorObj.transform.position = new Vector3(ZeroIndexXCoord, ZeroIndexYCoord, 0f);
        _menuState = MainMenuState.StartGame;
        // TODO: Animate cursor. Use an Invoke?
        //  If use invoke, will need to cancel said invoke when transitioning scene
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            TransitionMenuOption(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TransitionMenuOption(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // TODO: Add options here, like "Options" and switch Play to "New Game" and "Load Save"
            switch (_menuState)
            {
                case MainMenuState.StartGame:
                    GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
                    gameController.TransitionScene(1);
                    break;
                case MainMenuState.Quit:
                    // This is a preprocessor directive
                    // The UnityEditor.blabla code will only be included if being compiled in the unity editor
                    // which is important as UnityEditor is not available in a built game and building with this line
                    // present will cause a compilation failure. Similarly, Application.Quit does nothing when called
                    // in the editor and only works in a built game, so we need the UnityEditor line when 
                    // running this switch case in the editor
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    Application.Quit(0);
                    break;
            }
        }
    }

    private void TransitionMenuOption(int movementDownMenu)
    {
        var currentMenuOption = (int)_menuState;
        var nextItem = currentMenuOption + movementDownMenu;
        // If we try to move up from the 0th option, roll to the bottom of the list
        if (nextItem < 0)
        {
            nextItem = (int)MainMenuState.Quit;
        }
        // If we try to move down from the last option, roll back to the top
        if (!Enum.IsDefined(typeof(MainMenuState), nextItem))
            nextItem = 0;
        
        // Set our menu state to the resolved nextItem and move the cursor to said option
        _menuState = (MainMenuState)nextItem;
        MoveCursorToOption();
    }

    private void MoveCursorToOption()
    {
        float newY = ZeroIndexYCoord - 1.65f * (float)_menuState;
        _cursorObj.transform.position = new Vector3(ZeroIndexXCoord, newY, 0f);
    }
}
