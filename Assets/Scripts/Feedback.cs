using TapticPlugin;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnBlockPlaced += () =>
        {
#if UNITY_IOS
            TapticManager.Impact(ImpactFeedback.Medium);
#else
            Handheld.Vibrate();
#endif
        };

        GameManager.Instance.OnBlockPlaced += () =>
        {
#if UNITY_IOS
            TapticManager.Notification(NotificationFeedback.Error);
#else
            Handheld.Vibrate();
#endif
        };
    }
}
