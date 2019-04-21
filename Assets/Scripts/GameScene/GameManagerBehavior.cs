using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;
    public bool IsGoal;
	public int ScorePlayerOne, ScorePlayerTwo;
	public int SetPlayerOne, SetPlayerTwo;

	public AudioSource StageMusic;
	public GameObject PopupPause;
	public Sprite[] CharactersSprites;

	public bool IsHowToPlay;

    private string _playerName;
	private GameObject _scoreP1, _scoreP2;
	private GameObject _scoreP1_1, _scoreP1_2, _scoreP2_1, _scoreP2_2;
	private GameObject _setP1, _setP2;
	private GameObject _setP1_1, _setP1_2, _setP2_1, _setP2_2;
	private GameObject _winner, _loser, _draw01, _draw02;
	private GameObject _playerOne, _playerTwo;
	private GameObject _tmpPopup;
	private bool _isPaused;

	private int _maxScore;
	private int _maxSets;
	private float _playerTwoXAxisUnder20;
	private float _playerTwoXAxisOver20;
	private float _playerTwoXAxisEquals1;

	// ---- AUDIOS ---- //
	public int CastSPAudioFileID;
	public int CatchAudioFileID;
	public int DashAudioFileID;
	public int GoalAudioFileID;
	public int LiftAudioFileID;
	public int QuickEffectAudioFileID;
	public int ThrowAudioFileID;
	public int WallHitAudioFileID;
	public int MenuBipSelectAudioFileID;
	public int MenuBipConfirmAudioFileID;
	public int MenuBipReturnAudioFileID;
	public int MenuBipGoToAudioFileID;

	private int _pointAudioFileID;
	private int _setAudioFileID;
	private int _slideAudioFileID;
	// ---- AUDIOS ---- //

	void Start ()
	{
		if (IsHowToPlay) {
			PlayerPrefs.SetInt ("SelectedMap", 1);
		}
		AndroidNativeAudio.makePool();
		if (!IsHowToPlay)
			Destroy(GameObject.FindGameObjectWithTag ("MenuBackground"));
		if (PlayerPrefs.GetInt ("Music") == 0 && StageMusic != null)
			StageMusic.volume = 0.0f;
		if (StageMusic != null)
			StageMusic.Play ();
		ScorePlayerOne = 0;
		ScorePlayerTwo = 0;
		SetPlayerOne = 0;
		SetPlayerTwo = 0;
		_maxScore = PlayerPrefs.GetInt ("MaxScore");
		_maxSets = PlayerPrefs.GetInt ("MaxSets");
		_playerTwoXAxisUnder20 = 0.78f;
		_playerTwoXAxisOver20 = 1.2f;
		_playerTwoXAxisEquals1 = 0.39f;
		_playerName = CurrentPlayer.PlayerOne.ToString();

		_scoreP1 = GameObject.Find ("ScoreP1");
		_scoreP2 = GameObject.Find ("ScoreP2");
		_scoreP1_1 = GameObject.Find ("ScoreP1-1");
		_scoreP1_2 = GameObject.Find ("ScoreP1-2");
		_scoreP2_1 = GameObject.Find ("ScoreP2-1");
		_scoreP2_2 = GameObject.Find ("ScoreP2-2");

		_setP1 = GameObject.Find ("SetP1");
		_setP2 = GameObject.Find ("SetP2");
		_setP1_1 = GameObject.Find ("SetP1-1");
		_setP1_2 = GameObject.Find ("SetP1-2");
		_setP2_1 = GameObject.Find ("SetP2-1");
		_setP2_2 = GameObject.Find ("SetP2-2");

		_winner = GameObject.Find ("Winner");
		_loser = GameObject.Find ("Loser");
		_draw01 = GameObject.Find ("Draw01");
		_draw02 = GameObject.Find ("Draw02");

		// ---- AUDIOS ---- //
		CastSPAudioFileID = AndroidNativeAudio.load("CastSP.mp3");
		CatchAudioFileID = AndroidNativeAudio.load("Catch.mp3");
		DashAudioFileID = AndroidNativeAudio.load("Dash.mp3");
		GoalAudioFileID = AndroidNativeAudio.load("Goal.mp3");
		LiftAudioFileID = AndroidNativeAudio.load("Lift.mp3");
		QuickEffectAudioFileID = AndroidNativeAudio.load("QuickEffect.mp3");
		ThrowAudioFileID = AndroidNativeAudio.load("Throw.mp3");
		WallHitAudioFileID = AndroidNativeAudio.load("WallHit.mp3");
		MenuBipSelectAudioFileID = AndroidNativeAudio.load("MenuBipSelect.mp3");
		MenuBipConfirmAudioFileID = AndroidNativeAudio.load("MenuBipConfirm.mp3");
		MenuBipReturnAudioFileID = AndroidNativeAudio.load("MenuBipReturn.mp3");
		MenuBipGoToAudioFileID = AndroidNativeAudio.load("MenuBipGoTo.mp3");

		_pointAudioFileID = AndroidNativeAudio.load("Point.mp3");
		_setAudioFileID = AndroidNativeAudio.load("Set.mp3");
		_slideAudioFileID = AndroidNativeAudio.load("Slide.mp3");
		// ---- AUDIOS ---- //

		//_playerOne = GameObject.Find ("PlayerOne");
		//_playerTwo = GameObject.Find ("PlayerTwo");

		_playerOne = CreateCharacter(CurrentPlayer.PlayerOne, 1);

		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode ())
			DestroyP2Objects ();

		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Wall.GetHashCode ())
			_playerTwo = CreateCharacter (CurrentPlayer.PlayerTwo, 2);
		else
			CreateWall ();
		_isPaused = false;

		if (!IsHowToPlay)
			PlaceBall();
	}

	private GameObject CreateCharacter(CurrentPlayer player, int playerNb)
	{
		int characterNb = PlayerPrefs.GetInt ("P" + playerNb + "Character");
		GameObject.Find ("Player"+playerNb.ToString("D2")+"Image").GetComponent<SpriteRenderer>().sprite = CharactersSprites[characterNb];
		int multiplier = player == CurrentPlayer.PlayerOne ? -1 : 1;
		bool rotation = player == CurrentPlayer.PlayerOne ? false : true;

		var tmpPlayerInstance = Resources.Load<GameObject> ("Prefabs/Player"+characterNb.ToString("D2"));
		var tmpPlayer = Instantiate (tmpPlayerInstance, new Vector3(0.0f, 1.805f * multiplier, 0.0f), tmpPlayerInstance.transform.rotation);
		if (rotation)
			tmpPlayer.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
		tmpPlayer.transform.name = player.ToString ();
		tmpPlayer.GetComponent<PlayerBehavior> ().Player = player;
		tmpPlayer.GetComponent<AI> ().Player = player;
		if (player == CurrentPlayer.PlayerTwo && PlayerPrefs.GetInt("Opponent") == Opponent.AI.GetHashCode())
			tmpPlayer.GetComponent<AI>().enabled = true;
		return tmpPlayer;
	}

	private void CreateWall ()
	{
		var mapNb = PlayerPrefs.GetInt ("SelectedMap");
		var tmpWallInstance = Resources.Load<GameObject> ("Prefabs/TrainingWall"+mapNb.ToString("D2"));
		var tmpWall = Instantiate (tmpWallInstance, tmpWallInstance.transform.position, tmpWallInstance.transform.rotation);
		if (IsHowToPlay)
			tmpWall.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	private void DestroyP2Objects()
	{
		Destroy (GameObject.Find("P2SPCoolDown"));
		Destroy (GameObject.Find("LeftP2"));
		Destroy (GameObject.Find("RightP2"));
		Destroy (GameObject.Find("LiftP2"));
		Destroy (GameObject.Find("ThrowP2"));
		Destroy (GameObject.Find("SuperP2"));
		Destroy (GameObject.Find("AiP2"));

		if (PlayerPrefs.GetInt ("Opponent") == Opponent.Wall.GetHashCode ())
		{
			Destroy (GameObject.Find("ShadowP2"));
			GameObject.Find ("NoPlayerBannerTitle").GetComponent<UnityEngine.UI.Text> ().text = "TRAINING";
			if (IsHowToPlay)
				GameObject.Find ("NoPlayerBannerTitle").GetComponent<UnityEngine.UI.Text> ().text = "\t\t\t\t\t\t\tHOW TO PLAY";
			GameObject.Find ("Player02Image").GetComponent<SpriteRenderer> ().enabled = false;
		}
		GameObject.Find ("NoPlayerBanner").transform.position += new Vector3 (5.0f, 0.0f, 0.0f);

	}

    public void PlaceBall()
    {
		if (BallAlreadyExists())
			return;
        var currentPlayer = GameObject.Find(_playerName);
		currentPlayer.GetComponent<PlayerBehavior>().IsEngaging = true;
        var currentBall = Instantiate(Ball, new Vector3(0.0f, 0.0f, 0.0f), Ball.transform.rotation);
        currentBall.transform.name = "Ball";
		currentPlayer.GetComponent<PlayerBehavior> ().Ball = currentBall;
		currentBall.GetComponent<BallBehavior> ().CurrentPlayer = currentPlayer.GetComponent<PlayerBehavior> ().Player;
		//currentPlayer.GetComponent<PlayerBehavior>().CatchTheDisc();
		_scoreP1.GetComponent<Animator> ().Play ("StartingState");
		_scoreP2.GetComponent<Animator> ().Play ("StartingState");
		_setP1.GetComponent<Animator> ().Play ("StartingState");
		_setP2.GetComponent<Animator> ().Play ("StartingState");
		_winner.GetComponent<Animator> ().Play ("StartingState");
		_loser.GetComponent<Animator> ().Play ("StartingState");
		_playerOne.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
		_playerOne.GetComponent<PlayerBehavior> ().ConsecutiveHit = 0;
		if (_playerTwo != null)
			_playerTwo.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
    }

	public bool BallAlreadyExists()
	{
		var ballTab = GameObject.FindGameObjectsWithTag ("Disc");
		return ballTab.Length > 0;
	}

	private void EndGame()
	{
		SceneManager.LoadScene("TitleScene");
	}

	private void CheckIfGame()
	{
		bool gameEnd = false;

		if (SetPlayerOne >= _maxSets && SetPlayerOne > SetPlayerTwo) {
			_winner.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			_loser.transform.eulerAngles  = new Vector3(0.0f, 0.0f, 180.0f);
			_winner.GetComponent<Animator> ().Play ("DisplayFromBottom");
			_loser.GetComponent<Animator> ().Play ("DisplayFromTop");
			_playerOne.GetComponent<Animator> ().Play ("Victory");
			_playerTwo.GetComponent<Animator> ().Play ("Defeat");
			gameEnd = true;
		} else if (SetPlayerTwo >= _maxSets && SetPlayerTwo > SetPlayerOne) {
			_winner.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
			_loser.transform.eulerAngles  = new Vector3(0.0f, 0.0f, 0.0f);
			_winner.GetComponent<Animator> ().Play ("DisplayFromTop");
			_loser.GetComponent<Animator> ().Play ("DisplayFromBottom");
			_playerOne.GetComponent<Animator> ().Play ("Defeat");
			_playerTwo.GetComponent<Animator> ().Play ("Victory");
			gameEnd = true;
		}
		else if (SetPlayerOne >= _maxSets && SetPlayerTwo >= _maxSets && SetPlayerOne == SetPlayerTwo)
		{
			_draw01.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
			_draw02.transform.eulerAngles  = new Vector3(0.0f, 0.0f, 0.0f);
			_draw01.GetComponent<Animator> ().Play ("DisplayFromTop");
			_draw02.GetComponent<Animator> ().Play ("DisplayFromBottom");
			_playerOne.GetComponent<Animator> ().Play ("Defeat");
			_playerTwo.GetComponent<Animator> ().Play ("Defeat");
			gameEnd = true;
		}

		if (gameEnd == true)
		{
			/*SetPlayerOne = 0;
			SetPlayerTwo = 0;
			if (SetPlayerTwo == 0) {
				_setP1_2.transform.position = new Vector3 (_playerTwoXAxisUnder20 + 5.5f, _setP1_2.transform.position.y, _setP1_2.transform.position.z);
				_setP2_2.transform.position = new Vector3 (-_playerTwoXAxisUnder20 - 5.5f, _setP2_2.transform.position.y, _setP2_2.transform.position.z);
			}
			ChangeAllSets ();*/
			Invoke("EndGame", 5.0f);
		}
		else
			PlaceBall ();
	}

	private void CheckIfSet()
	{
		bool reset = false;

		if (ScorePlayerOne >= _maxScore && ScorePlayerOne > ScorePlayerTwo) {
			++SetPlayerOne;
			_setP1.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win ();
			_setP2.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Loose ();
			reset = true;
		} else if (ScorePlayerTwo >= _maxScore && ScorePlayerTwo > ScorePlayerOne) {
			++SetPlayerTwo;
			_setP1.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Loose ();
			_setP2.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win ();
			reset = true;
		} else if (ScorePlayerOne >= _maxScore && ScorePlayerTwo >= _maxScore && ScorePlayerOne == ScorePlayerTwo) {
			++SetPlayerOne;
			++SetPlayerTwo;
			_setP1.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win ();
			_setP2.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win ();
			reset = true;
		}

		if (reset) {
			ScorePlayerOne = 0;
			ScorePlayerTwo = 0;
			if (SetPlayerTwo == 0) {
				_scoreP1_2.transform.position = new Vector3 (_playerTwoXAxisUnder20 + 5.5f, _scoreP1_2.transform.position.y, _scoreP1_2.transform.position.z);
				_scoreP2_2.transform.position = new Vector3 (-_playerTwoXAxisUnder20 - 5.5f, _scoreP2_2.transform.position.y, _scoreP2_2.transform.position.z);
			}
			ChangeAllScores ();
			DisplaySets();
			Invoke("CheckIfGame", 3.0f);
		}
		else
			PlaceBall ();
	}

	public void NewBall(CurrentPlayer looser, int points, bool moreThanOneBall)
    {
		if (PlayerPrefs.GetInt ("SelectedMap") == 4)
			this.gameObject.GetComponent<ChangingGoalsBehavior> ().SomeoneScored (looser);
		if (PlayerPrefs.GetInt ("Opponent") == Opponent.Wall.GetHashCode ())
		{
			Invoke("PlaceBall", 0.5f);
			return;
		}
		_playerName = looser.ToString ();
		if (looser == CurrentPlayer.PlayerOne)
		{
			ScorePlayerTwo += points;
			if (!moreThanOneBall) {
				_scoreP1.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Loose();
				_scoreP2.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Win();
			}
		}
		else
		{
			ScorePlayerOne += points;
			if (!moreThanOneBall) {
				_scoreP2.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Loose ();
				_scoreP1.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Win ();
			}
		}
		if (!moreThanOneBall) {
			Invoke ("DisplayScores", 0.5f);
			Invoke ("CheckIfSet", 3.0f);
		}
    }

	private void DisplayScores()
	{
		CustomAudio.PlayEffect(_slideAudioFileID);
		_playerOne.GetComponent<PlayerBehavior> ().Recenter ();
		if (_playerTwo != null)
			_playerTwo.GetComponent<PlayerBehavior> ().Recenter ();
		_scoreP1.GetComponent<Animator> ().Play ("DisplayScore01");
		_scoreP2.GetComponent<Animator> ().Play ("DisplayScore02");
		Invoke ("ChangeAllScores", 0.75f);
	}

	private void DisplaySets()
	{
		CustomAudio.PlayEffect(_slideAudioFileID);
		_setP1.GetComponent<Animator> ().Play ("DisplayScore01");
		_setP2.GetComponent<Animator> ().Play ("DisplayScore02");
		Invoke ("ChangeAllSets", 0.75f);
	}

	private void ChangeAllScores()
	{
		if (ScorePlayerOne > 0 || ScorePlayerTwo > 0)
			CustomAudio.PlayEffect(_pointAudioFileID);

		if (ScorePlayerTwo >= 20) {
			_scoreP1_2.transform.position = new Vector3 (_playerTwoXAxisOver20, _scoreP1_2.transform.position.y, _scoreP1_2.transform.position.z);
			_scoreP2_2.transform.position = new Vector3 (-_playerTwoXAxisOver20, _scoreP2_2.transform.position.y, _scoreP2_2.transform.position.z);
		} else if (ScorePlayerTwo != 0) {
			_scoreP1_2.transform.position = new Vector3 (_playerTwoXAxisUnder20, _scoreP1_2.transform.position.y, _scoreP1_2.transform.position.z);
			_scoreP2_2.transform.position = new Vector3 (-_playerTwoXAxisUnder20, _scoreP2_2.transform.position.y, _scoreP2_2.transform.position.z);
		}

		ChangeScore (ScorePlayerOne, _scoreP1_1);
		ChangeScore (ScorePlayerTwo, _scoreP1_2);
		ChangeScore (ScorePlayerOne, _scoreP2_1);
		ChangeScore (ScorePlayerTwo, _scoreP2_2);
	}

	private void ChangeAllSets()
	{
		if (SetPlayerOne > 0 || SetPlayerTwo > 0)
			CustomAudio.PlayEffect(_setAudioFileID);

		if (SetPlayerTwo >= 20) {
			_setP1_2.transform.position = new Vector3 (_playerTwoXAxisOver20, _setP1_2.transform.position.y, _setP1_2.transform.position.z);
			_setP2_2.transform.position = new Vector3 (-_playerTwoXAxisOver20, _setP2_2.transform.position.y, _setP2_2.transform.position.z);
		} else if (SetPlayerTwo == 1) {
			_setP1_2.transform.position = new Vector3 (_playerTwoXAxisEquals1, _setP1_2.transform.position.y, _setP1_2.transform.position.z);
			_setP2_2.transform.position = new Vector3 (-_playerTwoXAxisEquals1, _setP2_2.transform.position.y, _setP2_2.transform.position.z);
		} else {
			_setP1_2.transform.position = new Vector3 (_playerTwoXAxisUnder20, _setP1_2.transform.position.y, _setP1_2.transform.position.z);
			_setP2_2.transform.position = new Vector3 (-_playerTwoXAxisUnder20, _setP2_2.transform.position.y, _setP2_2.transform.position.z);
		}

		ChangeScore (SetPlayerOne, _setP1_1);
		ChangeScore (SetPlayerTwo, _setP1_2);
		ChangeScore (SetPlayerOne, _setP2_1);
		ChangeScore (SetPlayerTwo, _setP2_2);
	}

	private void ChangeScore(int score, GameObject scoreGameobject, bool isScore = true)
	{
		scoreGameobject.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? '0' : 'A');
		scoreGameobject.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'A' : 'a');
		scoreGameobject.transform.GetChild (2).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'K' : 'A');
		scoreGameobject.transform.GetChild (3).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'a' : 'a');
		scoreGameobject.transform.GetChild (4).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'k' : 'A');
	}

	private string GetFormatedString(int score, char baseCharacter)
	{
		string tmpStr = "";
		int tmpScore = score;
		bool firstZero = score == 0 ? true : false;
		while (tmpScore != 0 || firstZero)
		{
			int digit = tmpScore % 10;
			int tmpIntChar = (int)baseCharacter + digit;
			char c = (char)tmpIntChar;
			tmpStr = c.ToString () + tmpStr;
			tmpScore = tmpScore / 10;
			firstZero = false;
		}
		return tmpStr;
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Escape))
		{
			if (_isPaused) {
				PopupPauseReturn ();
			} else
				DisplayPopupPause ();
		}
	}

	private void DisplayPopupPause()
	{
		if (StageMusic != null)
			StageMusic.Pause ();
		Time.timeScale = 0.0f;
		_isPaused = true;
		CustomAudio.PlayEffect (MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupPause, new Vector3(0.0f, 0.0f, 0.0f), PopupPause.transform.rotation);
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupPauseReturn;
		//GameObject.Find ("Button02Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupPauseReturn;
		GameObject.Find ("Button03Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PrepareLoadScene;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupPauseReturn;
		var tmpCurrentScore = "SCORE: " + ScorePlayerOne.ToString ("D2") + "/" + ScorePlayerTwo.ToString ("D2");
		GameObject.Find ("Score01Text").GetComponent<UnityEngine.UI.Text>().text = tmpCurrentScore;
		GameObject.Find ("Score02Text").GetComponent<UnityEngine.UI.Text>().text = tmpCurrentScore;
		var tmpCurrentSets = "SETS: " + SetPlayerOne.ToString ("D2") + "/" + SetPlayerTwo.ToString ("D2");
		GameObject.Find ("Set01Text").GetComponent<UnityEngine.UI.Text>().text = tmpCurrentSets;
		GameObject.Find ("Set02Text").GetComponent<UnityEngine.UI.Text>().text = tmpCurrentSets;
	}

	private void PopupPauseReturn()
	{
		if (StageMusic != null)
			StageMusic.Play ();
		Time.timeScale = 1.0f;
		Destroy (_tmpPopup);
		_isPaused = false;
		CustomAudio.PlayEffect (MenuBipGoToAudioFileID);
	}

	private void PrepareLoadScene()
	{
		Time.timeScale = 1.0f;
		CustomAudio.PlayEffect (MenuBipReturnAudioFileID);
		SceneManager.LoadScene("TitleScene");
	}

	void OnDestroy()
	{
		AndroidNativeAudio.unload(CastSPAudioFileID);
		AndroidNativeAudio.unload(CatchAudioFileID);
		AndroidNativeAudio.unload(DashAudioFileID);
		AndroidNativeAudio.unload(GoalAudioFileID);
		AndroidNativeAudio.unload(LiftAudioFileID);
		AndroidNativeAudio.unload(QuickEffectAudioFileID);
		AndroidNativeAudio.unload(ThrowAudioFileID);
		AndroidNativeAudio.unload(WallHitAudioFileID);
		AndroidNativeAudio.unload(MenuBipSelectAudioFileID);
		AndroidNativeAudio.unload(MenuBipConfirmAudioFileID);
		AndroidNativeAudio.unload(MenuBipReturnAudioFileID);
		AndroidNativeAudio.unload(MenuBipGoToAudioFileID);

		AndroidNativeAudio.unload(_pointAudioFileID);
		AndroidNativeAudio.unload(_setAudioFileID);
		AndroidNativeAudio.unload(_slideAudioFileID);
		AndroidNativeAudio.releasePool();
	}
}

/*
 * Idées musiques :
 * - Zadok - Myrone
 * - Tires on Fire - Coda
 */