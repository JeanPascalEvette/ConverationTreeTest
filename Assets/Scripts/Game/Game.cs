using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Game : MonoBehaviour {

    public static Game instance = null;
    private Profile cCurrentProfile = null;
    private Chapter cCurrentChapter = null;

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
	void Update () {
    }

    public void OnGUI()
    {
        if (cCurrentChapter != null)
        {
            GameObject cGOPrompt = GameObject.Find("/GameScene/Prompt");
            Text cTxtPrompt = cGOPrompt.GetComponent<Text>();
            cTxtPrompt.text = cCurrentChapter.GetCurrentNode().sName;
        }
    }
}
