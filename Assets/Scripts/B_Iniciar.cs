using UnityEngine;
using System.Collections;

public class B_Iniciar : MonoBehaviour {

	public GameObject loadingImage;

	public void LoadScene(int level)
	{
		loadingImage.SetActive(true);
		Application.LoadLevel(level);
	}
	public void doExitGame() {
		Application.Quit();
	}
}