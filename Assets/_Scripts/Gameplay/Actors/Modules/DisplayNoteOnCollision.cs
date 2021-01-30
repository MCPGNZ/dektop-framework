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
        // make sure it only collides with the player
        if (collision.gameObject.GetComponent<Movement>() == null) { return; }

        LevelParser.Note note = _Parser.GetNoteById(GetComponent<Actor>().Cell.Data);
        new MessageBox("A Crippy note", note.Message, new string[] { "Huh." }, string.IsNullOrWhiteSpace(note.Icon) ? null : note.Icon).Show();

        Destroy(gameObject);
    }
}
