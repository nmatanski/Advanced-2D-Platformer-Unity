using System;
using UnityEngine;

namespace Platformer.Managers
{
    [Serializable]
    public class SelectedObject
    {
        [SerializeField]
        private GameObject selectedGameObject;
        public GameObject SelectedGameObject
        {
            get { return selectedGameObject; }
            set { selectedGameObject = value; }
        }

        public bool HasSelectedObject { get => SelectedGameObject && SelectedGameObject.GetComponent<SelectableObject>().IsSelected; }
    }
}

