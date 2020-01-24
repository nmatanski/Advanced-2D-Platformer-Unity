using Platformer.Managers;
using UnityEngine;

namespace Platformer
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField]
        private Material selectedMaterial;

        [SerializeField]
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            private set { isSelected = value; }
        }

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        [Range(0, 1)]
        private float outlineThickness = 1f;

        [SerializeField]
        private bool isMaxThick = false;


        private GameObject selectedObject;
        private PlayerManager playerManager;


        private void Start()
        {
            playerManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PlayerManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider && hit.collider.gameObject == gameObject)
                {
                    selectedObject = hit.collider.gameObject;
                }
                else
                {
                    selectedObject = null;
                }
            }

            if (selectedObject)
            {
                playerManager.LastSelectedObject.SelectedGameObject = gameObject;
                IsSelected = true;
                outlineThickness = isMaxThick ? 63 : Mathf.Clamp(outlineThickness, 0, 1);
                selectedMaterial.SetFloat("_OutlineThickness", outlineThickness);
                selectedMaterial.SetColor("_OutlineColour", selectedColor);
            }
            else
            {
                IsSelected = false;
            }

            selectedMaterial.SetInt("_ShowOutline", IsSelected ? 1 : 0);
        }
    }
}
