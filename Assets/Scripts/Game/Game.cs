using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Game : MonoBehaviour {

    public static Game instance = null;
    private Profile cCurrentProfile = null;
    private Chapter cCurrentChapter = null;

    public Text cTxtPrompt;
    public InputField cTextInput;

    public Sprite incorrectInputGraphic;
    public Sprite correctInputGraphic;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    // Use this for initialization
    void InitGame() {
        string sGameSave = Path.Combine(Application.persistentDataPath, "Game.sav");
        if (File.Exists(sGameSave))
        {
            using (StreamReader streamReader = File.OpenText(sGameSave))
            {
                string jsonString = streamReader.ReadToEnd();
                cCurrentProfile = JsonUtility.FromJson<Profile>(jsonString);
            }
        }
        else
        {
            cCurrentProfile = new Profile();
        }

        cCurrentChapter = new Chapter(cCurrentProfile);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cTextInput.isFocused)
            cTextInput.ActivateInputField();

        if(cCurrentChapter != null)
        {
            if (Input.anyKeyDown)
            {
                int iOutcomeIdx = cCurrentChapter.ValidateString(cTextInput.text);
                if (iOutcomeIdx >= 0)
                {
                    cTextInput.image.sprite = correctInputGraphic;
                }
                else
                {
                    cTextInput.image.sprite = incorrectInputGraphic;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    cCurrentChapter.ChooseOutcome(iOutcomeIdx);
                    cTextInput.text = "";
                    cTextInput.image.sprite = incorrectInputGraphic;
                }
            }

        }
    }

    public void OnGUI()
    {
        if (cCurrentChapter != null)
        {
            cTxtPrompt.text = cCurrentChapter.GetCurrentNode().sText;
        }
    }
}
