using UnityEngine;

public class RuneTypingRotate : RuneTypingPoint
{
    public GameObject platformToRotate;
    [Header("Transformações (Inspector)")]
    public Vector3 finalPosition;
    public Vector3 finalRotation;
    public Vector3 initialPosition;
    public Vector3 initialRotation;

    protected override void Start()
    {
        base.Start();
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = initialPosition;
            platformToRotate.transform.eulerAngles = initialRotation;
        }
    }

    protected override void OnTypingSuccess()
    {
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = finalPosition;
            platformToRotate.transform.eulerAngles = finalRotation;
        }
        base.OnTypingSuccess();
    }

    protected override void OnTypingFail()
    {
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = initialPosition;
            platformToRotate.transform.eulerAngles = initialRotation;
        }
        base.OnTypingFail();
    }
}
