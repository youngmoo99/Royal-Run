using UnityEngine;

public class Apple : Pickup
{
    protected override void OnPickUp()
    {
        Debug.Log("Power up!");
    }
}
