using System.Collections.Generic;
using UnityEngine;

public class ChangePnaels : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    private int _index = 0;

    public void NextAutor()
    {
        int next = _index + 1;
        if (next >= panels.Count) { next = 0; _index = 0; }
        ShowPanel(next);
    }
    public void PreviousAutor()
    {
        int previous = _index - 1;
        if (previous < 0) { previous = panels.Count - 1; _index = panels.Count - 1; }
        ShowPanel(previous);
    }
    protected void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Count) return;
        for (int i = 0; i < panels.Count; i++)
            panels[i].SetActive(i == index);

        _index = index;

    }
}
