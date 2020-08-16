using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using ExtensionMethods;
using UnityEngine.InputSystem;


// https://stackoverflow.com/a/79903/3399416
public abstract class MenuController<TEnum> : MonoBehaviour
    where TEnum : struct, IConvertible, IComparable, IFormattable
{
    public TMP_Text menuText;
    public ECase menuOptionsCase;

    public AudioMultiSource audioMultiSource;

    public TEnum initialState;

    protected TEnum[] menuOptions;
    private int menuCursor = 0;

    private InputMaster controls;

    private void Awake() {
        controls = InputMasterSingleton.Get();
        controls.Menu.moveUp.performed += _ => MoveCursor(-1);
        controls.Menu.moveDown.performed += _ => MoveCursor(1); 
        controls.Menu.Confirm.performed += _ => ConfirmChoice();
        controls.Menu.Enable();
        menuOptions = (TEnum[])Enum.GetValues(typeof(TEnum));
    }

    private void OnEnable() {
        controls.Menu.Enable();
    }

    private void OnDisable() {
        // controls.Menu.Disable();
    }

    // Update is called once per frame
    private void Update() {
        DrawMenu();
    }

    public void ConfirmChoice() {
        if (!isActiveAndEnabled) return;
        HandleOption(menuOptions[menuCursor]);
        audioMultiSource.PlayYesSound();
    }

    public virtual void GoBack() {
        audioMultiSource.PlayNoSound();
    }

    public void MoveCursor(int cursorMoveOffset) {
        if (!isActiveAndEnabled) return;
        menuCursor += cursorMoveOffset;
        audioMultiSource.PlayMoveSound();
        try {
            menuCursor = MathUtils.InverseClamp(menuCursor, 0, menuOptions.Length - 1);
        } catch (Exception e) {
            Debug.Log(menuOptions);
            throw e;
        }
    }

    public void MoveCursor(InputAction.CallbackContext ctx, int cursorMoveOffset) {
        if (!isActiveAndEnabled) return;
        menuCursor += cursorMoveOffset;
        audioMultiSource.PlayMoveSound();
        menuCursor = MathUtils.InverseClamp(menuCursor, 0, menuOptions.Length - 1);
    }

    protected void DrawMenu() {
        menuText.text = string.Join("\n", menuOptions.Select((opt, ind) => DrawOption(opt, ind == menuCursor)));
    }

    protected string DrawOption(TEnum option, bool isSelected) {
        char cursor = isSelected ? '>' : ' ';
        string optionStr = option.ToString().Replace('_', ' ').FormatCase(menuOptionsCase);
        return cursor + optionStr;
    }

    protected abstract void HandleOption(TEnum menuOption);
}
