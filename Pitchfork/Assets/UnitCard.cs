using UnityEngine;
using System;

public class UnitCard : MonoBehaviour {

    public Texture CardImage;
    public ColorIdentity ColorIdentity;
    public oUnit Unit;

    public GameObject HighlightIcon;
    public GameObject HighlightA;
    public GameObject HighlightB;

    public Texture GemRed;
    public Texture GemBlue;
    public Texture GemGreen;
    public Texture GemYellow;
    public Texture GemBrown;
    public Texture GemPurple;

    public int Attack;
    public int Defense;
    public int Health;

    public delegate void DoCardClick(Guid ID);
    public event DoCardClick OnCardClick;

    private Renderer _rend;
    private Renderer _colorRend;
    private TextMesh _attack;
    private TextMesh _defense;
    private TextMesh _health;

	// Use this for initialization
	void Start () {
        _attack = this.transform.Find("Attack").GetComponent<TextMesh>();
        _defense = this.transform.Find("Defense").GetComponent<TextMesh>();
        _health = this.transform.Find("Health").GetComponent<TextMesh>();

        _attack.text = Attack.ToString();
        _defense.text = Defense.ToString();
        _health.text = Health.ToString();

        _rend = GetComponent<Renderer>();
        _rend.material.SetTexture("_MainTex", CardImage);

        _colorRend = this.transform.FindChild("ColorIdentity").GetComponent<Renderer>();

        ShowHighlight(false);

        RefreshUnitData();

        OnCardClick += GlobalCombat.GM.GetCardClick;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseUp()
    {
        OnCardClick.Invoke(Unit.ID);
    }

    public void SetUnitData(oUnit value)
    {
        Unit = value;
        //RefreshUnitData();
    }
    public void RefreshUnitData()
    {
        if (Unit != null)
        {
            Attack = Unit.CurAtt;
            Defense = Unit.CurDef;
            Health = Unit.CurHP;

            _attack.text = Attack.ToString();
            _defense.text = Defense.ToString();
            _health.text = Health.ToString();

            ColorIdentity = Unit.ColorIdentity;

            UpdateColorIdentity("_MainTex", ColorIdentity);

            _colorRend.material.SetFloat("_Perc", Unit.ManaRatio);

            if (Unit.ManaRatio == 1)
            {
                ShowHighlight(true);
            }
            else
            {
                ShowHighlight(false);
            }

            if (Unit.CurHP ==0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    public void ShowHighlight(bool Show)
    {
        HighlightIcon.SetActive(Show);
        HighlightA.SetActive(Show);
        HighlightB.SetActive(Show);
    }

    private void UpdateColorIdentity(string Texture, ColorIdentity colorID)
    {
        ColorIdentity nextColor;
        nextColor = colorID;

        switch (colorID)
        {
            case ColorIdentity.Red:
            case ColorIdentity.RedBlue:
            case ColorIdentity.RedBrown:
            case ColorIdentity.RedGreen:
            case ColorIdentity.RedPurple:
            case ColorIdentity.RedYellow:
                _colorRend.material.SetTexture(Texture, GemRed);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Red);
                break;
            case ColorIdentity.Blue:
            case ColorIdentity.BlueBrown:
            case ColorIdentity.BlueGreen:
            case ColorIdentity.BluePurple:
            case ColorIdentity.BlueYellow:
                _colorRend.material.SetTexture(Texture, GemBlue);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Blue);
                break;
            case ColorIdentity.Green:
            case ColorIdentity.GreenBrown:
            case ColorIdentity.GreenPurple:
            case ColorIdentity.GreenYellow:
                _colorRend.material.SetTexture(Texture, GemGreen);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Green);
                break;
            case ColorIdentity.Yellow:
            case ColorIdentity.YellowBrown:
            case ColorIdentity.YellowPurple:
                _colorRend.material.SetTexture(Texture, GemYellow);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Yellow);
                break;
            case ColorIdentity.Brown:
            case ColorIdentity.PurpleBrown:
                _colorRend.material.SetTexture(Texture, GemBrown);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Brown);
                break;
            case ColorIdentity.Purple:
                _colorRend.material.SetTexture(Texture, GemPurple);
                nextColor = (ColorIdentity)((int)colorID - (int)ColorIdentity.Purple);
                //_colorRend.material.SetTexture("_MainTex", GemPurple);
                //_colorRend.material.SetTexture("_SecondTex", GemPurple);
                break;
        }

        if (Texture == "_MainTex")
        {
            if (nextColor > ColorIdentity.Sword)
            {
                UpdateColorIdentity("_SecondTex", nextColor);
            }
            else
            {
                UpdateColorIdentity("_SecondTex", colorID);
            }
        }
    }
}
