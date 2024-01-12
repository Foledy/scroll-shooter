using UnityEngine;

namespace Time
{
    public class TimeManager : MonoBehaviour
    {
        public static bool IsStopped { get; private set; }

        public static void StopGame()
        {
            IsStopped = true;

            UnityEngine.Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            IsStopped = false;

            UnityEngine.Time.timeScale = 1;
        }
    }
}