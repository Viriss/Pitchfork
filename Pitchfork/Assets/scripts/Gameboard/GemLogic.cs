using UnityEngine;

public class GemLogic : MonoBehaviour
{
    public int TileID;
    public Vector2 StartPoint;
    public Vector2 NextPoint;
    public bool hasGravity;
    public ColorType CurrentColorType;
    public float TravelTime;

    public delegate void DoGemClick(int TileID);
    public event DoGemClick OnGemClick;
    public delegate void DoGemDrag(int TileID);
    public event DoGemDrag OnGemDrag;

    private float _timeTraveled;
    private bool isFalling;

    // Update is called once per frame
    void Update()
    {
        if (hasGravity)
        {
            if (!isFalling)
            {
                isFalling = true;
                _timeTraveled = 0;
            }

            Vector3 start = new Vector3(StartPoint.x, StartPoint.y, this.transform.position.z);
            Vector3 end = new Vector3(NextPoint.x, NextPoint.y, this.transform.position.z);

            this.transform.localPosition = Vector3.Lerp(start, end, _timeTraveled / TravelTime);

            if (_timeTraveled / TravelTime >= 1)
            {

                StartPoint = NextPoint;
                isFalling = false;
                hasGravity = false;
            }
            _timeTraveled += Time.deltaTime;
        }
    }
    public void StopMoving()
    {
        hasGravity = false;
        isFalling = false;
        StartPoint = NextPoint;
    }
    public void GemClick()
    {
        OnGemClick.Invoke(TileID);
    }
    public void GemDrag()
    {
        OnGemDrag.Invoke(TileID);
    }
}