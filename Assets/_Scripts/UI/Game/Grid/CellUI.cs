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
    public class CellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
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
        public static ReactiveProperty<bool> IsPointerDown { get; private set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsPointerEntered { get; private set; } = new ReactiveProperty<bool>(false);

        public int Row => _row;

        #endregion

        #region Unity Methods

        public void Reset()
        {
            backgroundImage.color = defaultColor;
        }

        void OnDestroy()
        {
            _cellUISelected.Dispose();
        }

        #endregion

        #region Public Methods

        public void ToggleSelect(bool isSelected)
        {
            _isSelected = isSelected;
        }

        public void Initialize(int row, int column, GridUI gridUI)
        {
            _row = row;
            _column = column;
            _gridUI = gridUI;

            IsPointerDown.Subscribe(isDown =>
            {
                if (!isDown)
                {
                    Reset();
                    ToggleSelect(false);
                }
            });
        }

        #endregion

        #region IPointerDownHandler Members

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPointerDown.Value = true;

            if (IsPointerEntered.Value && !_isSelected && _gridUI && _gridUI.IsWritingLocked == false)
            {
                _gridUI.RequestCellSelection(this);
            }
        }

        #endregion

        #region IPointerEnterHandler Members

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerEntered.Value = true;

            if (IsPointerDown.Value && !_isSelected && _gridUI && _gridUI.IsWritingLocked == false)
            {
                _gridUI.RequestCellSelection(this);
            }
        }

        #endregion

        #region IPointerExitHandler Members

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerEntered.Value = false;
        }

        #endregion

        #region IPointerUpHandler Members

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPointerDown.Value = false;
            _gridUI.OnCellPointerUp();
        }

        #endregion
    }
}
