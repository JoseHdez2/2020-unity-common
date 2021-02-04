using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ArpgAction : MonoBehaviour {
    public string actionName;
    private bool couldDoInPrevFrame;
    public bool canDo;
    public List<KeyCode> keys;
    private List<HudButton> hudBtns;
    [SerializeField] public UnityEvent action;

    public void SetAction(UnityAction unityAction){
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(unityAction);
        action = unityEvent;
    }
    
    private void Start() {
        hudBtns = FindObjectsOfType<HudButton>().ToList()
                    .Where(hudBtn => keys.Contains(hudBtn.keyCode)).ToList();
    }

    private void Update() {
        if(couldDoInPrevFrame && !canDo){
            hudBtns.ForEach(hudBtn => hudBtn.hudBtnTextAction.text = "");
        } else if(!couldDoInPrevFrame && canDo){
            hudBtns.ForEach(hudBtn => hudBtn.hudBtnTextAction.text = actionName);
        }
        if(canDo && keys.Any(k => Input.GetKeyDown(k))){
            action.Invoke(); // do the action
            // log the action
        }
        couldDoInPrevFrame = canDo;
    }
}