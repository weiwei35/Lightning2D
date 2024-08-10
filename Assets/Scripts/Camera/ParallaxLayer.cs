using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        transform.localPosition = newPos;
    }
    public void MoveY(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.y -= delta * parallaxFactor;

        transform.localPosition = newPos;
    }

}