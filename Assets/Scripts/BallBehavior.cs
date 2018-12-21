using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float Speed;
	public CurrentPlayer CurrentPlayer;
	public CurrentPlayer IsThrownBy;
	public Animator Animator;

    private GameObject _linkedPlayer;
    private GameObject _gameManager;
    private GameObject _camera;
    private float _spaceYFromPlayer = 1.55f;
	private float _spaceXFromPlayer = 0.125f;
	private int _catchCount;

	void Start ()
	{
	    // Initial Velocity
	    //GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        _gameManager = GameObject.Find("$GameManager");
        _camera = GameObject.Find("Camera");
		_catchCount = -1; //-1 because the first collision counts when serving
	    IsThrownBy = CurrentPlayer.None;
	}

    void Update()
    {
		if (CurrentPlayer != CurrentPlayer.None)
		{
			Animator.SetBool ("IsRotating", false);
			if (_linkedPlayer == null)
				_linkedPlayer = GetLinkedPlayer ();
			float spaceYFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? _spaceYFromPlayer * -1
				: _spaceYFromPlayer;
			float spaceXFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? _spaceXFromPlayer
				: _spaceXFromPlayer * -1;
			transform.position = new Vector3 (_linkedPlayer.transform.position.x + spaceXFromPlayer, spaceYFromPlayer);
		}
		else
			Animator.SetBool ("IsRotating", true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _linkedPlayer = col.gameObject;
			CurrentPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player;
			_linkedPlayer.GetComponent<PlayerBehavior>().GetTheDisc();
			if (++_catchCount % 2 == 0 && _catchCount != 0) // "_catchcount != 0" because it starts at -1
				Speed += 0.5f;
            IsThrownBy = CurrentPlayer.None;
        }
        else if (col.gameObject.tag == "Goal")
        {
            _camera.GetComponent<CameraBehavior>().GoalHit();
			col.gameObject.GetComponent<GoalBehavior> ().GoalHit ();
            _gameManager.GetComponent<GameManagerBehavior>().NewSet(
                col.gameObject.GetComponent<GoalBehavior>().Player);
            Destroy(gameObject);
        }
		else if (col.gameObject.tag == "Wall")
		{
            _camera.GetComponent<CameraBehavior>().WallHit();
			col.gameObject.GetComponent<WallBehavior>().WallHit();
		}
    }

    private GameObject GetLinkedPlayer()
    {
		if (CurrentPlayer == CurrentPlayer.PlayerOne)
            return GameObject.Find("PlayerOne");
		else if (CurrentPlayer == CurrentPlayer.PlayerTwo)
            return GameObject.Find("PlayerTwo");
        return null;
    }

	public void Throw(Vector2 direction, CurrentPlayer throwingPlayer, float addedPower)
    {
		IsThrownBy = throwingPlayer;
		CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (Speed + addedPower) / 4;
		float customSpeed = (Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
        GetComponent<Rigidbody2D>().velocity = direction * customSpeed;
    }
}
