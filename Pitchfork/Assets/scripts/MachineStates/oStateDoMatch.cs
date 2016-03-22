using System.Collections;
using UnityEngine;

public class oStateDoMatch : oStateMachine
{
    public int GemIndexA;
    public int GemIndexB;
    private GlobalCombat _gm;
    private bool _isStarted;
    private bool _isComplete;

    public override void Entry()
    {
        _gm = GlobalCombat.GM;
        _isStarted = false;
        _isComplete = false;
    }
    public override void Active()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            StartCoroutine(DoSwap());
        }
        if (_isComplete )
        {
            _gm.AddState(typeof(oStateTop));
            State = MachineState.Exit;
        }
    }
    public override void Exit()
    {

    }

    IEnumerator DoSwap()
    {
        oGridPoint PointA;
        oGridPoint PointB;
        Hashtable _score;

        if (_gm.Grid.IsNextTo(GemIndexA, GemIndexB))
        {
            _gm.HintTimer = 0;
            _gm.Grid.SwapTiles(GemIndexA, GemIndexB, _gm.Grid.Points);

            PointA = _gm.GetPointByIndex(GemIndexA);
            PointB = _gm.GetPointByIndex(GemIndexB);
            _gm.UpdateGemPosition(PointA);
            _gm.UpdateGemPosition(PointB);

            yield return new WaitForSeconds(_gm.TravelTime);
            _gm.LockMoving();

            if (_gm.Grid.FindSolutions().Count > 0)
            {
                while (_gm.Grid.FindSolutions().Count > 0)
                {
                    _gm.HintTimer = 0;
                    _gm.Grid.BoomSolutions();

                    _score = _gm.Grid.GetScoreHash();
                    //_gm.Score += _gm.Grid.GetScore();

                    foreach (ColorType colorKey in _score.Keys)
                    {
                        if ((int)_score[colorKey] > 3)
                        {
                            _gm.HasExtraTurn = true;
                            break;
                        }
                    }
//                    if (_gm.Grid.GetScore() > 12)
//                   {
//                        _gm.HasExtraTurn = true;
//                   }

                    if (_gm.TeamTurn == "Good")
                    {
                        _gm.Players[0].AddEnergy(_score);
                    }
                    if (_gm.TeamTurn == "Bad")
                    {
                        _gm.Players[1].AddEnergy(_score);
                    }
                    _gm.UpdateTeam();

                    CreateExplosions();
                    _gm.PlaySound();
                    _gm.Grid.DestroyBooms();

                    _gm.UpdateTileColors();
                    yield return new WaitForSeconds(0.1f);

                    _gm.Grid.ShiftDown();
                    _gm.UpdateTileColors();
                    _gm.UpdateGemPositions();
                    yield return new WaitForSeconds(_gm.TravelTime);
                    _gm.LockMoving();

                    //yield return new WaitForSeconds(0.1f);

                    _gm.ShiftEmptyUp();
                    _gm.Grid.FillEmpty();
                    _gm.UpdateTileColors();
                    yield return new WaitForSeconds(_gm.TravelTime);
                    _gm.LockMoving();
                }
            }
            else
            {
                //no match
                _gm.Grid.SwapTiles(GemIndexA, GemIndexB, _gm.Grid.Points);
                _gm.UpdateGemPosition(PointA);
                _gm.UpdateGemPosition(PointB);
                _gm.HasExtraTurn = true;  //whoopies doodle

                yield return new WaitForSeconds(_gm.TravelTime * 0.75f);
                _gm.LockMoving();
            }

            if (_gm.Grid.FindMoves().Count == 0)
            {
                OnScramble();
            }

            _gm.UpdateTileColors();
        }
        else
        {
            //deselect A?
            _gm.HasExtraTurn = true;
        }

        _isComplete = true;
        _gm.HintTimer = 0;
        yield return true;
    }

    public void OnScramble()
    {
        int MoveCount = _gm.Grid.FindMoves().Count;

        //Grid.Scramble();
        while (MoveCount < 2)
        {
            int IndexA = Random.Range(0, _gm.TilesTall * _gm.TilesWide);
            int IndexB = Random.Range(0, _gm.TilesTall * _gm.TilesWide);
            
            if (IndexA != IndexB)
            {
                _gm.Grid.SwapTiles(IndexA, IndexB, _gm.Grid.Points);
                if (_gm.Grid.FindSolutions().Count > 0)
                {
                    _gm.Grid.SwapTiles(IndexA, IndexB, _gm.Grid.Points);
                }
                else
                {
                    MoveCount = _gm.Grid.FindMoves().Count;
                }
            }
        }

        _gm.UpdateGemPositions();
    }

    private void CreateExplosions()
    {
        foreach (int index in _gm.Grid.FindSolutions())
        {
            GameObject explode = (GameObject)Instantiate(_gm.GemExplosion);
            ParticleSystem ps = explode.GetComponentInChildren<ParticleSystem>();

            switch (_gm.Grid.Points[index].ColorType)
            {
                case ColorType.Red:
                    //ps.startColor = Color.red;
                    ps.startColor = new Color(1.0f, 0, 0, 0.8f); 
                    break;
                case ColorType.Blue:
                    //ps.startColor = Color.blue;
                    ps.startColor = new Color(0.1f, 0.3f, 1f, 0.8f);
                    break;
                case ColorType.Brown:
                    ps.startColor = new Color(1.0f, 0.54f, 0, 0.8f); //orange?
                    break;
                case ColorType.Yellow:
                    //ps.startColor = Color.yellow;
                    ps.startColor = new Color(1, 0.843f, 0, 0.8f);
                    break;
                case ColorType.Purple:
                    //ps.startColor = Color.white;
                    ps.startColor = new Color(1, 1, 1, 0.8f);
                    break;
                case ColorType.Green:
                    //ps.startColor = Color.green;
                    ps.startColor = new Color(0.2f, 0.8f, 0.2f, 0.8f);
                    break;
                case ColorType.Sword:
                    //ps.startColor = Color.gray;
                    ps.startColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);
                    break;
            }

            explode.transform.localPosition = new Vector2(_gm.Grid.PointX(index), _gm.TilesTall - _gm.Grid.PointY(index));
            explode.transform.SetParent(_gm.Gameboard.transform, false);
        }
    }

}

