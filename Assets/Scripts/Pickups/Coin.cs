using UnityEngine;

public class Coin : Pickup
{
    protected override void OnPickUp()
    {
        Debug.Log("ADD 100 points");
    }
}
