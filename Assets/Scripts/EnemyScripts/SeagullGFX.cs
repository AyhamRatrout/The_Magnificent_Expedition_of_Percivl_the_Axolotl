using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SeagullGFX : MonoBehaviour
{
    public AIPath aiPath;

    void Update()
    {
        if(this.aiPath.desiredVelocity.x >= 0.01)
        {
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (this.aiPath.desiredVelocity.x <= - 0.01)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
