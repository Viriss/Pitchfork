using UnityEngine;

class oState_Begin : oStateMachine
{
    private GameObject _titleScreen;
    private SpriteRenderer _sprite;
    private int _fadeDelay = 1; //2;
    private float _fadeTime = 1.0f;
    private float _time;
    private bool _startFade = false;

    public override void Entry()
    {
        _time = 0;
        _titleScreen = GameObject.Find("TitleScreen");
        _sprite = _titleScreen.GetComponent<SpriteRenderer>();

        _sprite.sortingOrder = 10;
    }
    public override void Active()
    {
        if (_time >= _fadeDelay)
        {
            _startFade = true;
            _time = 0;
        }
        if (_startFade)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1 - _time / _fadeTime);
        }
        _time += Time.deltaTime;

        if (_startFade && _time >= _fadeTime)
        {
            GlobalCombat.GM.AddState(typeof(oState_BeginCombat));
            State = MachineState.Exit;
        }
    }
    public override void Exit()
    {

    }
}
