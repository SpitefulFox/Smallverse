using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject menu;
	public GameObject instructions;
	
	public void Instruct(){
		menu.SetActive(false);
		instructions.SetActive(true);
	}
	
	public void Back(){
		menu.SetActive(true);
		instructions.SetActive(false);
	}
	
	public void Game(){
		SceneManager.LoadScene("Multiverse");
	}
}
