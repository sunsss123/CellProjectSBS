using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreen : MonoBehaviour
{
    public List<TItleText> titletexts;
    public int index;
    public void StartGame()
    {
        SceneManager.LoadScene("Cell_002 1");
        removeEvents();
    }
    public void removeEvents()
    {
        foreach (TItleText t in titletexts)
        {
            t.removeevent();
        }
    }
    public void handletitle()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index++;
            if (index >= titletexts.Count)
                index = titletexts.Count - 1;
            changehub(index - 1, index);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
            if (index < 0)
                index = 0;
            changehub(index +1, index);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            titletexts[index].ButtonActive();
        }

    }
    public void changehub(int before,int after)
    {
        titletexts[before].DeActiveImageHub();
        titletexts[after].ActiveImageHub();
    }
    public void quitgame()
    {
        removeEvents();
        Application.Quit();
    }
    public void InitText()
    {
        index = 0;
        titletexts[index].ActiveImageHub();
        titletexts[0].ButtonEffect += StartGame;
        titletexts[titletexts.Count - 1].ButtonEffect += quitgame;
    }
    private void Start()
    {
        InitText();
    }
    // Update is called once per frame
    void Update()
    {
        handletitle();
    }
}
