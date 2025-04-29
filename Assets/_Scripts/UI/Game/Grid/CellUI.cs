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
        [SerializeField] public Image backgroundImage;
        [SerializeField] private Color defaultColor;
        [SerializeField] public Color selectedColor;

        #endregion

        #region Fields

        private readonly Subject<CellUI> _cellUISelected = new();

        private char _characterValue;
        private int _column;
        private GridUI _gridUI;
        private bool _isSelected;
        private int _row;

        #endregion

        #region Properties

        public Subject<CellUI> CellUISelected => _cellUISelected;

        public char CharacterValue
        {
            get => _characterValue;
            set
            {
                _characterValue = value;
                character.text = _characterValue.ToString();
            }
        }

        public int Column => _column;

        public int Row => _row;

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

        public void SetGridPosition(int row, int column, GridUI gridUI)
        {
            _row = row;
            _column = column;
            _gridUI = gridUI;
        }

        #endregion

        #region IPointerMoveHandler Members

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsGridPointerDown && !_isSelected)
            {
                if (_gridUI != null)
                {
                    _gridUI.RequestCellSelection(this);
                }
            }
        }

        #endregion
    }
}
