﻿using DG.Tweening;
using Platformer.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Interactables
{
    public class SelectableObject : MonoBehaviour
    {
        private Material selectedMaterial;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        //[Range(0, 1)]
        private float outlineThickness = 1f;
        public float OutlineThickness { get => outlineThickness; set => outlineThickness = value; }

        [SerializeField]
        [Range(0, 100)]
        private float hoverOutlineThicknessDecreaseMultiplier = 50f;

        [SerializeField]
        private bool isMaxThick = false;

        [SerializeField]
        private List<GameObject> hoveredSelectables;
        public List<GameObject> HoveredSelectables { get => hoveredSelectables; private set => hoveredSelectables = value; }



        private GameObject selectedObject;
        private GameObject hoveredObject;
        private PlayerManager playerManager;

        public bool IsSelected { get; private set; } = false;

        public bool IsHovered { get; private set; } = false;


        private void Start()
        {
            //playerManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PlayerManager>();
            playerManager = Managers.Managers.Player;
            selectedMaterial = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                hoveredObject = gameObject;

                HoveredSelectables.Add(hoveredObject);

                if (Input.GetMouseButtonDown(0))
                {
                    selectedObject = gameObject;
                }
            }
            else
            {
                HoveredSelectables.Remove(hoveredObject);

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

                var tempOutlineThickness = (isMaxThick ? 6.3f : OutlineThickness) * hoverOutlineThicknessDecreaseMultiplier / 100f;

                if (selectedObject)
                {
                    playerManager.LastSelectedObject.SelectedGameObject = selectedObject;
                    IsSelected = true;

                    tempOutlineThickness = isMaxThick ? 63 : Mathf.Clamp(OutlineThickness, 0, 1000);
                    float duration = isMaxThick ? .3f : .4f;

                    selectedMaterial.DOFloat(tempOutlineThickness, "_OutlineThickness", duration).SetEase(Ease.OutCubic);
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

            bool hasOutline = IsHovered || IsSelected;
            if (!hasOutline)
            {
                selectedMaterial.DOFloat(0, "_OutlineThickness", .05f);
            }

            selectedMaterial.SetInt("_ShowOutline", hasOutline ? 1 : 0);
        }
    }
}
