using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.Grid
{
    /// <summary>
    /// Handles UI interactions for a cell in the grid.
    /// </summary>
    public class CellUI : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private TextMeshProUGUI character;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;
        
        public Action<CellUI> OnCellUISelected;
        public static bool IsGridPointerDown;
        
        private char _characterValue;
        private bool _isSelected;
        
        
        
        public char CharacterValue
        {
            get => _characterValue;
            set
            {
                _characterValue = value;
                character.text = _characterValue.ToString();
            }
        }
        
        public void OnUp()
        {
            backgroundImage.color = defaultColor;
        }
        
        public void ToggleSelect(bool isSelected)
        {
            _isSelected = isSelected;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsGridPointerDown && !_isSelected)
            {
                backgroundImage.color = selectedColor;
                OnCellUISelected?.Invoke(this);
                ToggleSelect(true);
            }
        }
    }
}
