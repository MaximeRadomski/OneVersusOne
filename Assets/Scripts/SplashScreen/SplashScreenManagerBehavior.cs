using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManagerBehavior : MonoBehaviour
{
	private GameObject _borderTop, _borderBot;
	//private GameObject _scanLines;
	private GameObject _touchToStart;
	private GameObject _gameName;
	private GameObject _abjectPresents;
	private GameObject _movingBackground;
	private GameObject _blackBackground; 
	private GameObject _camera;

	private bool _isDuringStart = false;
	private bool _isDuringTVON = false;
	private bool _isDuringAbject = false;
	private bool _isDuringLogo = false;

	void Start ()
	{
		AndroidNativeAudio.makePool();
		_borderTop = GameObject.Find ("BorderTop");
		_borderBot = GameObject.Find ("BorderBot");
		//_scanLines = GameObject.Find ("ScanLines");
		_touchToStart = GameObject.Find ("TouchToStart");
		_gameName = GameObject.Find ("GameName");
		_abjectPresents = GameObject.Find ("AbjectPresents");
		_movingBackground = GameObject.Find ("MovingBackground");
		_blackBackground = GameObject.Find ("BlackBackground");
		_camera = GameObject.Find("Camera");

		_isDuringStart = true;

		Invoke ("SwitchTVON", 1.0f);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0))
		{
			if (_isDuringLogo)
				PlayTitleAnimation ();
			else if (_isDuringAbject)
				DisplayName ();
			else if (_isDuringTVON)
				DisplayAbject ();
			else if (_isDuringStart)
				SwitchTVON ();
		}
			
	}

	private void SwitchTVON()
	{
		if (_isDuringTVON)
			return;
		_isDuringTVON = true;
		AndroidNativeAudio.play(GameObject.Find("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>().SwitchTVONFileID);
		_blackBackground.SetActive (false);
		_movingBackground.GetComponent<MeshRenderer> ().enabled = true;
		_movingBackground.GetComponent<MenuBackgroundBehavior> ().ScrollSpeed = 0.05f;
		Invoke ("DisplayAbject", 1.0f);
	}

	private void DisplayAbject()
	{
		if (_isDuringAbject)
			return;
		_isDuringAbject = true;
		_abjectPresents.GetComponent<Animator> ().Play ("FadeInAndOut");
		Invoke ("DisplayName", 3.5f);
	}

	private void DisplayName()
	{
		if (_isDuringLogo)
			return;
		_isDuringLogo = true;
		AndroidNativeAudio.play(GameObject.Find("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>().NamePresentationFileID);
		_abjectPresents.SetActive (false);
		_gameName.GetComponent<SpriteRenderer> ().enabled = true;
		_touchToStart.GetComponent<UnityEngine.UI.Text> ().enabled = true;
	}

	public void PlayTitleAnimation()
	{
		_camera.GetComponent<CameraBehavior>().WallHit();
		AndroidNativeAudio.play(GameObject.Find("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>().BorderMovementFileID);
		_movingBackground.GetComponent<MenuBackgroundBehavior> ().PlayAudio ();
		_touchToStart.SetActive (false);
		_borderTop.GetComponent<Animator> ().Play ("BorderGoesUp");
		_borderBot.GetComponent<Animator> ().Play ("BorderGoesDown");
		_gameName.GetComponent<Animator> ().Play ("GameNameGoesUp");
		//_scanLines.GetComponent<Animator> ().Play ("FadeOut");
		_movingBackground.GetComponent<MenuBackgroundBehavior> ().ScrollSpeed = -1.0f;
		Invoke ("GoToTitle", 0.5f);
	}

	private void GoToTitle()
	{
		_camera.GetComponent<CameraBehavior>().WallHit();
		_movingBackground.GetComponent<MenuBackgroundBehavior> ().ScrollSpeed = 0.25f;
		SceneManager.LoadScene("TitleScene");
	}
}
