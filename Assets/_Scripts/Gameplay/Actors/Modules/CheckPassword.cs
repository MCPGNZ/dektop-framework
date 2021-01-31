using Mcpgnz.DesktopFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CheckPassword : MonoBehaviour
{
    [Inject] private LevelParser _Parser;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // make sure it only collides with the player
        if (collision.gameObject.GetComponent<Movement>() == null) { return; }

        LevelParser.Note note = _Parser.GetNoteById(GetComponent<Actor>().Cell.Data);
        string answer = TextInput.Show(title: "A challenge!",
                                       message: note.Message,
                                       iconPath: string.IsNullOrWhiteSpace(note.Icon) ? null : note.Icon);
        string correctAnswer = GetComponent<Actor>().Cell.Parameters;
        if (answer == correctAnswer)
        {
            Sounds.WindowsLogon.Play();
            Destroy(gameObject);
        }
        else
        {
            Sounds.WindowsLogoffSound.Play();
        }
    }
}
