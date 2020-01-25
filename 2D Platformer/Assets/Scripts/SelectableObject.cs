using DG.Tweening;
using Platformer.Managers;
using UnityEngine;

namespace Platformer
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField]
        private Material selectedMaterial;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        [Range(0, 1)]
        private float outlineThickness = 1f;

        [SerializeField]
        private bool isMaxThick = false;

        private GameObject selectedObject;
        private GameObject hoveredObject;
        private PlayerManager playerManager;

        public bool IsSelected { get; private set; } = false;

        public bool IsHovered { get; private set; } = false;


        private void Start()
        {
            playerManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PlayerManager>();
        }

        private void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                hoveredObject = gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    selectedObject = gameObject;
                }
            }
            else
            {
                hoveredObject = null;

                if (Input.GetMouseButtonDown(0))
                {
                    selectedObject = null;
                }
            }


            if (hoveredObject)
            {
                IsHovered = true;
                selectedMaterial.SetColor("_OutlineColour", selectedColor);

                var tempOutlineThickness = outlineThickness / 2f;

                if (selectedObject)
                {
                    playerManager.LastSelectedObject.SelectedGameObject = selectedObject;
                    IsSelected = true;

                    tempOutlineThickness = isMaxThick ? 63 : Mathf.Clamp(outlineThickness, 0, 1);
                    selectedMaterial.DOFloat(tempOutlineThickness, "_OutlineThickness", .4f).SetEase(Ease.OutCubic);
                }
                else
                {
                    selectedMaterial.DOFloat(tempOutlineThickness, "_OutlineThickness", .1f);
                }
            }
            else
            {
                IsHovered = false;
            }

            if (!selectedObject)
            {
                IsSelected = false;
            }

            selectedMaterial.SetInt("_ShowOutline", IsHovered || IsSelected ? 1 : 0);
        }
    }
}
