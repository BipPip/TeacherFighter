using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeLock : MonoBehaviour
{
    public GameObject scroll;
    float scroll_pos = 0;
    float[] pos;
    
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length -1f);
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if(Input.GetKeyDown("d"))
        {
            scroll_pos = scroll.GetComponent<Scrollbar>().value + 1;
        }else if(Input.GetKeyDown("a")){
            scroll_pos = scroll.GetComponent<Scrollbar>().value - 1;
        }
        else{
            for(int i = 0; i < pos.Length; i++)
            {
                if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scroll.GetComponent<Scrollbar>().value = Mathf.Lerp(scroll.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    
                }
            }
        }

        for(int i = 0; i < pos.Length; i++)
        {
            if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance /2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f,1f), 0.1f);

                    // here is where it knows what button is selected

                for(int a = 0; a < pos.Length; a++)
                {
                    if(a != i)
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                }
            }
        }
    }
}
