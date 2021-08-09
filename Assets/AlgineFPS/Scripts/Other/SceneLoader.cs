using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Algine {

    public class SceneLoader : MonoBehaviour
    {
        public Image loadingImage;
        public TextMeshProUGUI loadingText;
        public GameObject LoadingParent;
        public GameObject continueUi;

        // Start is called before the first frame update

        public void LoadScene()
        {
            StartCoroutine(LoadAsynchronously(1));
            continueUi.SetActive(false);
        }

        IEnumerator LoadAsynchronously(int sceneIndex)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
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
