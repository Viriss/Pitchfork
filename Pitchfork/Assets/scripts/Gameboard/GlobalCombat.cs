using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GlobalCombat : MonoBehaviour
{
    public static GlobalCombat GM;

    public oGrid Grid;
    public AudioClip[] Clips;
    public List<oPlayer> Players;

    public GameObject UnitCard;
    public string TeamTurn;
    public bool HasExtraTurn;
    public bool StartOfGame;

    public int TilesWide = 8;
    public int TilesTall = 8;
    public float TravelTime = 0.25f;
    public float DragDistance = 5.0f;

    public float HintTimer = 0;
    private float _hintTimeout = 15.0f;

    public GameObject Gem;
    public GameObject Gameboard;
    public GameObject GemExplosion;
    public GameObject GemHint;
    public GameObject TurnIndicator;
    public GameObject TeamA;
    public GameObject TeamB;
    public GameObject UI;

    public GameObject CardDetailUI;

    public Sprite GemRed;
    public Sprite GemGreen;
    public Sprite GemBlue;
    public Sprite GemPurple;
    public Sprite GemBrown;
    public Sprite GemYellow;
    public Sprite GemSword;

    public delegate void Click_Gem(int TileID);
    public event Click_Gem OnClickGem;

    public delegate void Drag_Gem(int TileID);
    public event Drag_Gem OnDragGem;

    public delegate void Click_Card(Guid CardID);
    public event Click_Card OnClickCard;

    public delegate void Click_CardButton(int ButtonID);
    public event Click_CardButton OnClickCardButton;

//    public int Score;

    private List<oStateMachine> States;
    private AudioSource audio;
    private List<GameObject> Cards;
    private UICardDetail UICardDetail;
    

    void Awake()
    {
        if (GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;

        DontDestroyOnLoad(this);

        TeamTurn = "Good";
        HasExtraTurn = false;
        StartOfGame = true;
        Players = new List<oPlayer>();

        //Score = 0;

        Grid = new oGrid(TilesTall, TilesWide);
        Grid.CreateStableGrid();

        States = new List<oStateMachine>();
        Cards = new List<GameObject>();

        UI.SetActive(true);
        UICardDetail = CardDetailUI.GetComponent<UICardDetail>();
        UICardDetail.DoCloseWindow();

        try
        {
            AddState(gameObject.AddComponent(typeof(oState_Begin)) as oState_Begin);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();

        TeamA = this.transform.FindChild("TeamA").gameObject;
        TeamB = this.transform.FindChild("TeamB").gameObject;
    }

    void Update()
    {
        CleanStateList();

        if (States.Count > 0)
        {
            States[0].DoUpdate();
        }
        else
        {
            //whoops?
            throw new System.Exception("no states found?");
        }

        HintTimer += Time.deltaTime;
        if (HintTimer >= _hintTimeout)
        {
            ShowHint();
        }
    }

    public void AddState(Type NextState)
    {
        //oStateMachine x = Gameboard.AddComponent(NextState.GetType()) as oStateMachine;
        oStateMachine xx;
        xx = gameObject.AddComponent(NextState) as oStateMachine;


        States.Add(xx);
    }
    public void AddState(oStateMachine NextState)
    {
        States.Add(NextState);
    }

    public void DoUnitCardClick(Guid CardGuid)
    {
        oPlayer player = GetPlayerByCardGuid(CardGuid);
        oUnit unit = GetUnitByGuid(CardGuid);

        Texture2D newImage = (Texture2D)Resources.Load("card/" + unit.CardImage);
        Rect rec = new Rect(0, 0, newImage.width, newImage.height);

        UICardDetail.CardImage.GetComponentInChildren<Image>().sprite = Sprite.Create(newImage, rec, new Vector2(0.5f, 0.5f));

        UICardDetail.ButtonA.gameObject.SetActive(false);
        UICardDetail.ButtonB.gameObject.SetActive(false);
        if (player.TeamType == TeamType.Good)
        {
            if (unit.ManaRatio == 1)
            {
                UICardDetail.ButtonA.gameObject.SetActive(true);
                UICardDetail.ButtonB.gameObject.SetActive(true);
            }
        }

        UICardDetail.DoShowWindow();
    }
    public void DoCloseCardUI()
    {
        UICardDetail.DoCloseWindow();
    }

    public oGridPoint GetPointByTileID(int TileID)
    {
        foreach (oGridPoint p in Grid.Points)
        {
            if (p.TileID == TileID) { return p; }
        }
        return null;
    }
    public oGridPoint GetPointByIndex(int Index)
    {
        foreach (oGridPoint p in Grid.Points)
        {
            if (p.Index == Index) { return p; }
        }
        return null;
    }
    public void UpdateGemPositions()
    {
        foreach (oGridPoint p in Grid.Points)
        {
            UpdateGemPosition(p);
        }
    }

    public void DealDamageToFirstUnit(TeamType Team, int Damage)
    {
        oPlayer p = GetPlayerByTeamType(Team);
        p.ReceiveDamage(p.GetFirstActiveUnit().ID, Damage);
    }

    public void DisplayTeams()
    {
        int index = 0;

        foreach (oPlayer player in Players)
        {
            index = 0;
            foreach (oUnit u in player.Team)
            {
                if (player.TeamType == TeamType.Good)
                {
                    GameObject unitCard = (GameObject)Instantiate(UnitCard);
                    unitCard.transform.localPosition = new Vector2(0, index * -2);
                    unitCard.transform.SetParent(TeamA.transform, false);

                    UnitCard uc = unitCard.GetComponent<UnitCard>();
                    uc.CardImage = (Texture)Resources.Load("card/" + u.CardImage, typeof(Texture));
                    uc.SetUnitData(u);

                    index += 1;

                    Cards.Add(unitCard);
                }
                if (player.TeamType == TeamType.Bad)
                {
                    GameObject unitCard = (GameObject)Instantiate(UnitCard);
                    unitCard.transform.localPosition = new Vector2(0, index * -2);
                    unitCard.transform.SetParent(TeamB.transform, false);

                    UnitCard uc = unitCard.GetComponent<UnitCard>();
                    uc.CardImage = (Texture)Resources.Load("card/" + u.CardImage, typeof(Texture));
                    uc.SetUnitData(u);

                    index += 1;

                    Cards.Add(unitCard);
                }
            }
        }
    }
    public void UpdateTeam()
    {
        foreach (GameObject card in Cards)
        {
            UnitCard uc = card.GetComponent<UnitCard>();
            uc.SetUnitData(GetUnitByGuid(uc.Unit.ID));
            uc.RefreshUnitData();
        }
    }
    public void UpdateTurnIndicator()
    {
        if (TeamTurn == "Good")
        {
            TurnIndicator.transform.position = new Vector3(-5.5f, 4.45f);
        }
        if (TeamTurn == "Bad")
        {
            TurnIndicator.transform.position = new Vector3(5.65f, 4.45f);
        }

    }
    public oUnit GetUnitByGuid(Guid guid)
    {
        foreach(oPlayer p in Players)
        {
            foreach(oUnit u in p.Team)
            {
                if (u.ID == guid) { return u; }
            }
        }
        return null;
    }
    public oPlayer GetPlayerByCardGuid(Guid guid)
    {
        foreach (oPlayer p in Players)
        {
            foreach (oUnit u in p.Team)
            {
                if (u.ID == guid) { return p; }
            }
        }
        return null;
    }
    public oPlayer GetPlayerByTeamType(TeamType TeamType)
    {
        foreach(oPlayer p in Players)
        {
            if (p.TeamType == TeamType) { return p; }
        }
        return null;
    }


    public void GetGemClick(int TileID)
    {
        if (OnClickGem != null)
        {
            OnClickGem.Invoke(TileID);
        }
    }
    public void GetGemDrag(int TileID)
    {
        try {
            if (OnDragGem != null)
                OnDragGem.Invoke(TileID);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(TileID);
        }
    }
    public void GetCardClick(Guid ID)
    {
        if (OnClickCard != null)
        {
            OnClickCard.Invoke(ID);
        }
    }
    public void GetCardButtonClick(int ButtonID)
    {
        if (OnClickCardButton != null)
        {
            OnClickCardButton.Invoke(ButtonID);
        }
    }

    public void LockMoving()
    {
        foreach (oGridPoint point in Grid.Points)
        {
            Transform gem = Gameboard.transform.Find("gem" + point.TileID.ToString());
            GemLogic gl = gem.GetComponent<GemLogic>();
            gl.StopMoving();
            gem.transform.localPosition = gl.NextPoint;
        }
    }
    public void ShiftEmptyUp()
    {
        int cntEmpty = 0;
        for (int x = 0; x < TilesWide; x++)
        {
            cntEmpty = Grid.CountEmptyInColumn(x);
            if (cntEmpty > 0)
            {
                foreach (oGridPoint p in Grid.GetPointsInColumn(x))
                {
                    if (p.ColorType == ColorType.empty)
                    {
                        Transform gem = Gameboard.transform.Find("gem" + p.TileID.ToString());
                        GemLogic gl = gem.GetComponent<GemLogic>();
                        gl.StartPoint = new Vector2(Grid.PointX(p.Index), TilesTall - Grid.PointY(p.Index) + cntEmpty);
                        gl.NextPoint = new Vector2(Grid.PointX(p.Index), TilesTall - Grid.PointY(p.Index));
                        gl.hasGravity = true;
                        gem.localPosition = gl.StartPoint;
                    }
                }
            }
        }
    }
    public void ShowHint()
    {
        //foreach (oSwapPoints sp in Grid.FindMoves()) { }
        oSwapPoints sp = Grid.FindMoves()[0];

        GameObject hint1 = (GameObject)Instantiate(GemHint);
        hint1.transform.localPosition = new Vector2(Grid.PointX(sp.IndexA), TilesTall - Grid.PointY(sp.IndexA));
        hint1.transform.SetParent(Gameboard.transform, false);

        GameObject hint2 = (GameObject)Instantiate(GemHint);
        hint2.transform.localPosition = new Vector2(Grid.PointX(sp.IndexB), TilesTall - Grid.PointY(sp.IndexB));
        hint2.transform.SetParent(Gameboard.transform, false);

        HintTimer = 0;
    }
    public void SetTravelTime(float TravelTime)
    {
        foreach (oGridPoint point in Grid.Points)
        {
            Transform gem = Gameboard.transform.Find("gem" + point.TileID.ToString());
            GemLogic gl = gem.GetComponent<GemLogic>();
            gl.TravelTime = TravelTime;
        }
    }

    public void PlaySound()
    {
        audio.pitch = UnityEngine.Random.Range(0.6f, 1.6f);
        audio.PlayOneShot(Clips[2]);
    }

    public void CleanStateList()
    {
        foreach (oStateMachine sm in States)
        {
            if (sm.State == MachineState.Exited)
            {
                Destroy(sm);
                States.Remove(sm);
                CleanStateList();
                return;
            }
        }
    }
    public void UpdateGemPosition(oGridPoint point)
    {
        Transform gem = Gameboard.transform.Find("gem" + point.TileID.ToString());
        GemLogic gl = gem.GetComponent<GemLogic>();
        gl.NextPoint = new Vector2(Grid.PointX(point.Index), TilesTall - Grid.PointY(point.Index));
        gl.hasGravity = true;
    }
    public void UpdateTileColors()
    {
        Transform gem;

        foreach (oGridPoint p in Grid.Points)
        {
            gem = (Transform)Gameboard.transform.Find("gem" + p.TileID);

            if (gem != null)
            {
                gem.GetComponent<SpriteRenderer>().enabled = true;
                switch (p.ColorType)
                {
                    case ColorType.Red:
                        gem.GetComponent<SpriteRenderer>().sprite = GemRed;
                        break;
                    case ColorType.Green:
                        gem.GetComponent<SpriteRenderer>().sprite = GemGreen;
                        break;
                    case ColorType.empty:
                        gem.GetComponent<SpriteRenderer>().enabled = false;
                        break;
                    case ColorType.Blue:
                        gem.GetComponent<SpriteRenderer>().sprite = GemBlue;
                        break;
                    case ColorType.Purple:
                        gem.GetComponent<SpriteRenderer>().sprite = GemPurple;
                        break;
                    case ColorType.Brown:
                        gem.GetComponent<SpriteRenderer>().sprite = GemBrown;
                        break;
                    case ColorType.Yellow:
                        gem.GetComponent<SpriteRenderer>().sprite = GemYellow;
                        break;
                    case ColorType.Sword:
                        gem.GetComponent<SpriteRenderer>().sprite = GemSword;
                        break;
                }
            }
        }
    }

}
