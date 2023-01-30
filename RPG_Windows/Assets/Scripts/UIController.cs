using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private GameObject _interface;

    [SerializeField] private ModalPanel modalPanel;
    //Getter
    public ModalPanel ModalPanel => modalPanel;

    [SerializeField] private LoadingPanel loadingPanel;
    //Getter
    public LoadingPanel LoadingPanel => loadingPanel;

    private void Start() {
        if(instance == null) {
            instance = this;
            Debug.Log(instance);
        }
        
        ClearUI();
    }

    public void ClearUI() {
        Image[] screens = _interface.GetComponentsInChildren<Image>().Where(x => x.gameObject.layer == 5).ToArray();
        Debug.Log("screens count: "+screens.Length);
        foreach(var screen in screens) {
            Debug.Log("screen name: "+screen.gameObject.name);
            screen.gameObject.SetActive(false);
        }
    }

}
