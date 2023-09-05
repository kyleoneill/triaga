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
    private Rigidbody2D _cursorRigidbody;
    private MainMenuState _menuState;

    // Start is called before the first frame update
    void Start()
    {
        _cursorObj = Instantiate(cursor);
        _menuState = MainMenuState.StartGame;
        MoveCursorToOption();
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
    }

    private void AnimateCursor()
    {
        // Give the cursor a downwards velocity and then start an invoke which moves it up and down, animating it
        _cursorRigidbody = _cursorObj.GetComponent<Rigidbody2D>();
        _cursorRigidbody.velocity = new Vector2(0, -0.5f);
        InvokeRepeating("FlipAnimationVelocity", 0.5f, 1f);
    }
    
    private void FlipAnimationVelocity()
    {
        var currentYVelocity = _cursorRigidbody.velocity.y;
        _cursorRigidbody.velocity = new Vector2(0, currentYVelocity * -1);
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
                    CancelInvoke();
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
        CancelInvoke();
        _cursorObj.transform.position = new Vector3(ZeroIndexXCoord, newY, 0f);
        AnimateCursor();
    }
}
