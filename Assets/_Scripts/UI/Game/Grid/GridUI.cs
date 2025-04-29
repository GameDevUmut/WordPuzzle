using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.Game.Grid
{
    public class GridUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        #region Serializable Fields

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private FlexibleGridLayout gridLayout;
        [Header("Sentence Properties")]
        [SerializeField] private TextMeshProUGUI formedSentenceText;
        [SerializeField] private Color formedSentenceErrorColor;
        [SerializeField] private Color formedSentenceSuccessColor;

        #endregion

        #region Fields

        private List<CellUI> _cells = new List<CellUI>();
        private Color _formedSentenceDefaultColor;
        private IGridService _gridService;
        private bool _isWritingLocked = false;
        private CellUI _lastSelectedCell;
        private StringBuilder _stringBuilder = new StringBuilder();
        private ITrieService _trieService;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _formedSentenceDefaultColor = formedSentenceText.color;
        }

        #endregion

        #region Public Methods

        public void EnterCharacter(CellUI cell)
        {
            _stringBuilder.Append(cell.CharacterValue);
            formedSentenceText.text = _stringBuilder.ToString();
        }

        public void RequestCellSelection(CellUI cell)
        {
            if (_lastSelectedCell == null || IsNeighbor(_lastSelectedCell, cell))
            {
                cell.backgroundImage.color = cell.selectedColor;
                cell.CellUISelected.OnNext(cell);
                cell.ToggleSelect(true);
                _lastSelectedCell = cell;
            }
        }

        #endregion

        #region Private Methods

        [Inject]
        private void Construct(IGridService gridService, ITrieService trieService)
        {
            _trieService = trieService;
            _gridService = gridService;

            _gridService.GridCreated.Subscribe(_ => CreateGridUI()).AddTo(this);
        }

        private void CreateGridUI()
        {
            _cells.Clear();
            foreach (Transform child in cellParent)
            {
                Destroy(child.gameObject);
            }

            int rows = _gridService.GridRows;
            int columns = _gridService.GridColumns;
            gridLayout.columns = columns;
            gridLayout.rows = rows;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GameObject cellInstance = Instantiate(cellPrefab, cellParent);
                    CellUI cellUI = cellInstance.GetComponentInChildren<CellUI>();
                    if (cellUI != null)
                    {
                        _cells.Add(cellUI);
                        char character = _gridService.GetCellCharacter(r, c);
                        cellUI.CharacterValue = character;
                        cellUI.SetGridPosition(r, c, this);
                        cellUI.CellUISelected.Subscribe(EnterCharacter).AddTo(cellUI);
                    }
                    else
                    {
                        Debug.LogError($"Cell prefab does not have a CellUI component attached.", cellInstance);
                    }
                }
            }

            _lastSelectedCell = null;
        }

        private async UniTask CheckWord()
        {
            _isWritingLocked = true;
            var inputString = _stringBuilder.ToString();
            if (inputString.Equals(String.Empty)) return;
            var success = await _trieService.TestifyWord(inputString);

            Color targetColor = success ? formedSentenceSuccessColor : formedSentenceErrorColor;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(formedSentenceText.DOColor(targetColor, 0.2f));
            if (!success)
            {
                // Add horizontal shake if the word is incorrect
                sequence.Join(formedSentenceText.transform.DOShakePosition(0.15f,
                    strength: new Vector3(20, 0, 0),
                    vibrato: 10,
                    randomness: 90,
                    fadeOut: true));
            }

            sequence.Append(formedSentenceText.DOColor(_formedSentenceDefaultColor, 0.2f));

            await sequence.Play();

            await UniTask.Delay(success ? 1000 : 300);

            _stringBuilder.Clear();
            formedSentenceText.text = "";
            _isWritingLocked = false;
        }

        private bool IsNeighbor(CellUI a, CellUI b)
        {
            int dr = Math.Abs(a.Row - b.Row);
            int dc = Math.Abs(a.Column - b.Column);
            return (dr <= 1 && dc <= 1) && !(dr == 0 && dc == 0);
        }

        #endregion

        #region IPointerDownHandler Members

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isWritingLocked) return;
            CellUI.IsGridPointerDown = true;
        }

        #endregion

        #region IPointerUpHandler Members

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isWritingLocked || !CellUI.IsGridPointerDown) return;
            CellUI.IsGridPointerDown = false;
            foreach (var cell in _cells)
            {
                cell.OnUp();
                cell.ToggleSelect(false);
            }

            _lastSelectedCell = null;
            CheckWord().Forget();
        }

        #endregion
    }
}
