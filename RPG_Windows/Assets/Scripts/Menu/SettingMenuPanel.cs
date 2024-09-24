using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingMenuPanel : MonoBehaviour
{
    public bool OnSettingPanel = false;

    [SerializeField] protected SettingData currSettingData;
    [SerializeField] protected TextMeshProUGUI containerTitleText;
    [SerializeField] protected Color selectedBtnColor;
    [SerializeField] protected int selectedContainerIndex = 0;
    [SerializeField] protected List<GameObject> btns = new List<GameObject>();
    [SerializeField] protected List<GameObject> containerCanvas = new List<GameObject>();
    public UnityEvent OnInitCallback;

    [Header("Screen Setting")]
    [SerializeField] protected TMP_Dropdown resolutionDropdown;
    [SerializeField] protected TMP_Dropdown displayDropdown;
    [SerializeField] protected SwitchToggle fpsToggle;
    [SerializeField] protected SwitchToggle verticalSyncToggle;

    [Header("Audio Setting")]
    [SerializeField] protected Slider BgmVolumeSlider;
    [SerializeField] protected Slider SEVolumeSlider;
    [SerializeField] protected Slider SpeechVolumeSlider;
    
    
    private void OnEnable() {
        Debug.Log("SettingMenuPanel");
        OnInitCallback.Invoke();
        InitSettingMenuPanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    #region Canvas Actions 
    
    public void InitSettingMenuPanel() {
        // set default container
        containerCanvas[0].SetActive(true);
        // set select button color
        btns[0].GetComponent<Image>().color = selectedBtnColor;
        SetSelectedContainerIndex(0);
        // set default title text
        var btnText = btns[0].GetComponentInChildren<TextMeshProUGUI>();
        containerTitleText.text = btnText.text;
        
        RefreshSettingShownValue();
    }

    public void RefreshSettingShownValue() {
        resolutionDropdown.value = currSettingData.screen_Resolution;
        resolutionDropdown.RefreshShownValue();
        displayDropdown.value = currSettingData.screen_Display;
        resolutionDropdown.RefreshShownValue();
        fpsToggle.IsOn = currSettingData.screen_FPS == 0 ? false : true;
        verticalSyncToggle.IsOn = currSettingData.screen_VerticalSynchronization == 0 ? false : true;

        BgmVolumeSlider.value = currSettingData.audio_Volume_BGM;
        SEVolumeSlider.value = currSettingData.audio_Volume_SE;
        SpeechVolumeSlider.value = currSettingData.audio_Volume_Speech;
    }

    public void SetContainerTitle(TextMeshProUGUI textUGUI) {
        containerTitleText.text = textUGUI.text;
    }

    public void SetSelectedContainerIndex(int selectedIndex) {
        selectedContainerIndex = selectedIndex;
        btns.Where((btn, index) => index != selectedContainerIndex).ToList()
            .ForEach(item =>
            {
                item.GetComponent<Image>().color = Color.white;
            });

        btns[selectedIndex].GetComponent<Image>().color = selectedBtnColor;

    }

    public void ActivateContainer(GameObject container) {
        DeactivateAllContainer();
        container.SetActive(true);
    }

    public void DeactivateAllContainer() {
        foreach(GameObject container in containerCanvas)
            container.SetActive(false);
    }

    #endregion

    #region Data Storage Actions

    public void ApplyCurrentSettingData() {
        ApplyScreenSetting();
        ApplyAudioSetting();

        // TODo there will be more...

        // switch(selectedContainerIndex) {
        //     case 0:
        //         ApplyScreenSetting();
        //         break;
        //     case 1:
        //         ApplyAudioSetting();
        //         break;
        //     case 2:
        //         break;
        //     default:
        //         Debug.Log("ApplyCurrentSettingData Default");
        //         break;
        // }
    }

    
    public void ApplyScreenSetting() {
         int resolutionType = resolutionDropdown.value;
         int displayType = displayDropdown.value;
         int fpsType = fpsToggle.IsOn ? 1 : 0;
         int vsType = verticalSyncToggle.IsOn ? 1 : 0;

        GameSettingUtils.ScreenSet(currSettingData, resolutionType, displayType, fpsType, vsType);
    }

    public void ApplyAudioSetting() {
        float bgm = BgmVolumeSlider.value;
        float se = SEVolumeSlider.value;
        float speech = SpeechVolumeSlider.value;

        GameSettingUtils.AudioSet(currSettingData, bgm, se, speech);
    }

    // TODo there will be more...

    #endregion



}
