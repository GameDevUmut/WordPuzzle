using System;
using R3;
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
        public static bool IsGridPointerDown;

        #region Serializable Fields

        [SerializeField] private TextMeshProUGUI character;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;

        #endregion

        #region Fields

        private char _characterValue;
        private bool _isSelected;
        private readonly Subject<CellUI> _cellUISelected = new();

        #endregion

        #region Properties

        public char CharacterValue
        {
            get => _characterValue;
            set
            {
                _characterValue = value;
                character.text = _characterValue.ToString();
            }
        }

        public Observable<CellUI> CellUISelected => _cellUISelected;

        #endregion

        #region Unity Methods

        void OnDestroy()
        {
            _cellUISelected.Dispose();
        }

        #endregion

        #region Public Methods

        public void OnUp()
        {
            backgroundImage.color = defaultColor;
        }

        public void ToggleSelect(bool isSelected)
        {
            _isSelected = isSelected;
        }

        #endregion

        #region IPointerMoveHandler Members

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsGridPointerDown && !_isSelected)
            {
                backgroundImage.color = selectedColor;
                _cellUISelected.OnNext(this);
                ToggleSelect(true);
            }
        }

        #endregion
    }
}
