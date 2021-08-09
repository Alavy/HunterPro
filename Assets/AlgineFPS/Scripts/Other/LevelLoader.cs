using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Algine {

    public class LevelLoader : MonoBehaviour
    {
        public Image loadingImage;
        public TextMeshProUGUI loadingText;
        public GameObject LoadingParent;
        public GameObject continueUi;

        // Start is called before the first frame update

        public void LoadScene()
        {
            StartCoroutine(LoadAsynchronously(LevelInfo.Level_to));
            continueUi.SetActive(false);
        }

        IEnumerator LoadAsynchronously(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            LoadingParent.SetActive(true);
            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / .9f);
                loadingImage.fillAmount = progress;
                loadingText.text =  (progress * 100).ToString("f2")+"%";
                yield return null;
            }
        }
    }

}
