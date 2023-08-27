using UnityEngine;

public class UISwitcher : MonoBehaviour{
    [SerializeField] private Transform defaultSubUI;

    private Transform currentActivatedUI;

    private void Start(){
        foreach (Transform child in transform){
            if (child.parent == transform){
                child.gameObject.SetActive(false);
            }
        }

        SetActiveUI(defaultSubUI);
    }

    public void SetActiveUI(Transform newActiveUI){
        if (newActiveUI == currentActivatedUI) return;

        if (currentActivatedUI != null){
            currentActivatedUI.gameObject.SetActive(false);
        }

        newActiveUI.gameObject.SetActive(true);
        currentActivatedUI = newActiveUI;
    }
}