using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    private Text title;
    private Text clickToPlay;

    [SerializeField]
    private float showDelay;
    [SerializeField]
    private float showTime;

    private float currentDelay, currentShow;

    private bool showFirst = true;
    // Start is called before the first frame update
    void Start()
    {
        foreach (RectTransform t in transform) {
            if (t.name == "Title") {
                title = t.GetComponent<Text>();
            } else if (t.name == "Start") {
                clickToPlay = t.GetComponent<Text>();
            }
        }
        Debug.Log("title is " + title);
        Debug.Log("start is " + clickToPlay);
        currentDelay = 0;
        currentShow = showTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDelay < showDelay) {
            currentDelay += Time.deltaTime;
            if (currentDelay >= showDelay) {
                currentShow = 0;
            }
        } else if (currentShow < showTime) {
            currentShow = Mathf.Clamp(currentShow + Time.deltaTime, 0f, showTime);
            float completed = currentShow / showTime;
            if (showFirst) {
                Color c = title.color;
                c.a = completed;
                title.color = c;
                if (completed == 1f) {
                    currentDelay = 0f;
                    showFirst = false;
                }
            } else {
                Color c = clickToPlay.color;
                c.a = completed;
                clickToPlay.color = c;
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("Level1");
        }
    }
}
