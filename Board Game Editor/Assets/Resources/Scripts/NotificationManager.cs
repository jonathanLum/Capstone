using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NotificationManager : MonoBehaviour
{
    public Queue<string> notificationQueue;

    public IEnumerator NotifyLoop()
    {
        notificationQueue = new Queue<string>();
        Transform parent = GameObject.Find("Notifications").transform;

        while (true)
        {
            Vector3 position = new Vector3(0, 0, 0);

            while (notificationQueue.Count > 0)
            {
                string text = notificationQueue.Dequeue();
                GameObject notification = (GameObject)GameObject.Instantiate(
                                           Resources.Load("UI/Notification"), new Vector3(0, 0, 0),
                                           Quaternion.identity, parent);
                position.y = -((parent.childCount - 1) * notification.GetComponent<RectTransform>().sizeDelta.y + (parent.childCount * 10));
                notification.GetComponent<RectTransform>().localPosition = position;
                notification.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
                StartCoroutine(DisplayNotification(notification));
            }

            position.y = 0;
            foreach (Transform child in parent)
            {
                position.y -= child.gameObject.GetComponent<RectTransform>().sizeDelta.y + 10;
                child.gameObject.GetComponent<RectTransform>().localPosition = position;
            }
            yield return null;
        }
    }

    private IEnumerator DisplayNotification(GameObject notification)
    {
        notification.SetActive(true);
        yield return new WaitForSeconds(2);
        notification.SetActive(false);
        DestroyImmediate(notification);
    }
    public void Notify(string text)
    {
        notificationQueue.Enqueue(text);
    }
}

