using Mcpgnz.DesktopFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DisplayNoteOnCollision : MonoBehaviour
{
    [Inject] private LevelParser _Parser;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This is for mines, who have solid bodies.
        OnAnyKindOfContact(collision.collider);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // This is for spikes, who you can pass through.
        OnAnyKindOfContact(collider);
    }

    private void OnAnyKindOfContact(Collider2D collider)
    {
        var bodyObject = collider.attachedRigidbody.gameObject;
        Debug.Log($"I collided with {bodyObject.name}.");

        // make sure it only hurts the player (who has Movement behavior)
        if (bodyObject.GetComponent<Movement>() == null) { return; }
        ShowNote();
    }

    private void ShowNote()
    {
        LevelParser.Note note = _Parser.GetNoteById(GetComponent<Actor>().Cell.Data);
        new MessageBox(title: "A Crippy note",
                       message: note.Message,
                       buttonLabels: new string[] { "Huh." },
                       iconPath: string.IsNullOrWhiteSpace(note.Icon) ? null : note.Icon)
            .ShowDialog();
    }
}
