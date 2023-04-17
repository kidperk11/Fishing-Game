using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTETickerController : MonoBehaviour
{
    [SerializeField] private float QTEMaxTime = 1;
    public float QTETimer;
    public RectTransform tickerTransform;
    public RectTransform startPoint;
    public RectTransform endPoint;

    bool activateTicker;
    public GameObject boxPattern;
    public QTEActivationBox currentActivationBox;
    public List<QTEActivationBox> allActivationBoxes;

    public EnemyHealthAndQTE currentEnemy;
    public HarpoonController currentHarpoon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTicker();
    }

    private void MoveTicker()
    {
        if (activateTicker)
        {
            QTETimer += Time.deltaTime;
            float percentageComplete = QTETimer / QTEMaxTime;
            tickerTransform.anchoredPosition = Vector3.Lerp(startPoint.anchoredPosition, endPoint.anchoredPosition, percentageComplete);
            if(tickerTransform.anchoredPosition == endPoint.anchoredPosition)
            {
                Debug.Log("Ticker has reached the end point, now failing Ticker");
                FailTicker();
            }
            Debug.Log("Count for currentEnemy.allBoxes: " + currentEnemy.allBoxes.Count);
        }
    }

    public void ActivateTicker(EnemyHealthAndQTE enemyHealth, HarpoonController harpoon)
    {
        Debug.Log("The ticker has been activated. Current Enemy Attack Pattern: " + enemyHealth.boxPattern);
        currentEnemy = enemyHealth;
        currentHarpoon = harpoon;
        boxPattern = currentEnemy.boxPattern;
        boxPattern.SetActive(true);
        Debug.Log("Test value for allActivationBoxes: " + allActivationBoxes);
        foreach(QTEActivationBox box in currentEnemy.allBoxes)
        {
            allActivationBoxes.Add(box);
        }
        QTETimer = 0;
        tickerTransform.anchoredPosition = startPoint.anchoredPosition;
        activateTicker = true;
        //NOTE: Add code to make a sound effect and play a special animation
    }

    public void FailTicker()
    {
        Debug.Log("The ticker has failed, now resetting instance.");
        boxPattern = null;
        currentActivationBox = null;
        allActivationBoxes.Clear();
        foreach(QTEActivationBox box in currentEnemy.allBoxes)
        {
            box.gameObject.SetActive(true);
        }
        currentEnemy.boxPattern.SetActive(false);
        currentEnemy = null;
        currentHarpoon.ResetHarpoon();
        currentHarpoon = null;
        QTETimer = 0;
        activateTicker = false;
        //NOTE: Add code to make a sound effect and play a special animation
    }

    public void SuccessfulInput()
    {
        Debug.Log("Count for currentEnemy.allBoxes: " + currentEnemy.allBoxes.Count);
        Debug.Log("A successful input has been triggered");
        allActivationBoxes.Remove(currentActivationBox);
        Debug.Log("Count for currentEnemy.allBoxes: " + currentEnemy.allBoxes.Count);
        currentActivationBox.gameObject.SetActive(false);
        currentActivationBox = null;
        Debug.Log(allActivationBoxes);
        if(allActivationBoxes.Count == 0)
        {
            //NOTE: This code needs to be altered to move the enemy and the harpoon back to the player
            Debug.Log("QTE has been successfully completed.");
            currentActivationBox = null;
            allActivationBoxes.Clear();
            activateTicker = false;
            Debug.Log("Count for currentEnemy.allBoxes: " + currentEnemy.allBoxes.Count);

            foreach (QTEActivationBox box in currentEnemy.allBoxes)
            {
                Debug.Log(box);
                box.gameObject.SetActive(true);
            }
            currentEnemy.boxPattern.SetActive(false);
            currentHarpoon.ResetHarpoon();
            currentHarpoon = null;
            QTETimer = 0;
            Destroy(currentEnemy.gameObject);
            currentEnemy = null;

        }
        //NOTE: Add code to make a sound effect and play a special animation
    }
}
