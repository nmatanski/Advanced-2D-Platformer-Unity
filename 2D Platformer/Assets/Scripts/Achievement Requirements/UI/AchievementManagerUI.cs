using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Achievements.UI
{
    public class AchievementManagerUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject achievementPrefab;

        [SerializeField]
        private Sprite[] sprites;

        // Start is called before the first frame update
        private void Start()
        {
            CreateAchievementUI("General", "title", "descr", 5, 0);
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void CreateAchievementUI(string category, string title, string description, int points, int spriteIndex)
        {
            var achievement = Instantiate(achievementPrefab);
            SetAchievementInfoUI(category, achievement, title, description, points, spriteIndex);
        }

        private void SetAchievementInfoUI(string category, GameObject achievement, string title, string description, int points, int spriteIndex)
        {
            achievement.transform.SetParent(GameObject.Find(category).transform);
            achievement.transform.localScale = Vector3.one;
            achievement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
            achievement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = points.ToString();
            achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites[spriteIndex];
        }
    }
}
