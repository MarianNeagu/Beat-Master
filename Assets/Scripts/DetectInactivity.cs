using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DetectInactivity : MonoBehaviour, IDragHandler
{

    //public ScrollRect scrollRect_GenresTabs;
    //public ScrollRect scrollRect_Melodies;
    private bool hasDragged;
    public int indexScrollRect; // 0 - genres, 1 - melodies
    [SerializeField] GameObject scrollRect_GenresTabs;
    [SerializeField] GameObject scrollRect_Melodies;

    void Start()
    {
        hasDragged = false;
        Invoke("ShowScrollIndicators", 7f);
        if (indexScrollRect == 0)
            scrollRect_GenresTabs.SetActive(false);
        else scrollRect_Melodies.SetActive(false);
    }
    public void OnDrag(PointerEventData data)
    {
        hasDragged = true;
        if (indexScrollRect == 0)
            scrollRect_GenresTabs.SetActive(false);
        else scrollRect_Melodies.SetActive(false);
    }

    public void ShowScrollIndicators()
    {
        if(!hasDragged)
        {
            if (indexScrollRect == 0)
                scrollRect_GenresTabs.SetActive(true);
            else scrollRect_Melodies.SetActive(true);
        }
    }
    
}
