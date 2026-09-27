using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    // Ditambahkan HANYA supaya BaseWeapon/PistolWeapon/MeleeWeapon bisa compile
    // (mereka butuh tahu arah hadap player). Tidak mengubah cara gerak sama sekali.
    public Vector2 LastMovementDirection { get; private set; } = Vector2.right;

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current.aKey.isPressed) move.x = -1;
        if (Keyboard.current.dKey.isPressed) move.x = 1;

        transform.Translate(move * speed * Time.deltaTime);

        if (move.sqrMagnitude > 0.01f)
        {
            LastMovementDirection = move.normalized;
        }
    }
}