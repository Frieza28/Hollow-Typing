using UnityEngine;

public class PlatformRotateOnTyping : MonoBehaviour
{
    public void Rotate()
    {
        // Rotaciona 90º para a esquerda
        transform.Rotate(0, 0, 90);
        // Podes adicionar efeitos/sons/anim
    }
}
