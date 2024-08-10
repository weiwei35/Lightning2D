using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    public ParallaxCameraDelegate onCameraTranslateY;
    public GameObject center;

    private float oldPositionX;
    private float oldPositionY;
    // bool isOut = false;

    void Start()
    {
        oldPositionX = transform.position.x;
        oldPositionY = transform.position.y;
    }

    void Update()
    {
        // if(Vector3.Distance(transform.position,center.transform.position) >= 6){
            // if(!isOut){
                // oldPosition = transform.position.x;
            //     isOut = true;
            // }
            if (transform.position.x != oldPositionX)
            {
                if (onCameraTranslate != null)
                {
                    float delta = oldPositionX - transform.position.x;
                    onCameraTranslate(delta);
                }

                oldPositionX = transform.position.x;
            }
            if (transform.position.y != oldPositionY)
            {
                if (onCameraTranslate != null)
                {
                    float delta = oldPositionY - transform.position.y;
                    onCameraTranslateY(delta);
                }

                oldPositionY = transform.position.y;
            }
        // }
        // if(Vector3.Distance(transform.position,center.transform.position) < 8){
        //     isOut = false;
        // }
    }
}