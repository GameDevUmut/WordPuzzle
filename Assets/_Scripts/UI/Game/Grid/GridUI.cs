using System.Collections.Generic;
using System.Text;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace UI.Game.Grid
{
    public class GridUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        #region Serializable Fields

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private TextMeshProUGUI formedSentenceText;

        #endregion

        #region Fields

        private List<CellUI> _cells = new List<CellUI>();


        private IGridService _gridService;
        private StringBuilder _stringBuilder = new StringBuilder();

        #endregion

        #region Unity Methods

        private void Start()
        {
            CreateGridUI();
        }

        #endregion

        #region Public Methods

        public void EnterCharacter(CellUI cell)
        {
            _stringBuilder.Append(cell.CharacterValue);
            formedSentenceText.text = _stringBuilder.ToString();
        }

        #endregion

        #region Private Methods

        [Inject]
        private void Construct(IGridService gridService)
        {
            _gridService = gridService;
        }

        private void CreateGridUI()
        {
            int rows = _gridService.GridRows;
            int columns = _gridService.GridColumns;

            gridLayoutGroup.constraintCount = columns;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GameObject cellInstance = Instantiate(cellPrefab, cellParent);
                    CellUI cellUI = cellInstance.GetComponent<CellUI>();
                    if (cellUI != null)
                    {
                        _cells.Add(cellUI);
                        char character = _gridService.GetCellCharacter(r, c);
                        cellUI.CharacterValue = character;
                        cellUI.OnCellUISelected += EnterCharacter;
                    }
                    else
                    {
                        Debug.LogError($"Cell prefab does not have a CellUI component attached.", cellInstance);
                    }
                }
            }
        }

        #endregion

        #region IPointerDownHandler Members

        public void OnPointerDown(PointerEventData eventData)
        {
            CellUI.IsGridPointerDown = true;
        }

        #endregion

        #region IPointerUpHandler Members

        public void OnPointerUp(PointerEventData eventData)
        {
            CellUI.IsGridPointerDown = false;
            foreach (var cell in _cells)
            {
                cell.OnUp();
                cell.ToggleSelect(false);
            }

            _gridService.TestifyWord(_stringBuilder.ToString());

            _stringBuilder.Clear();
            formedSentenceText.text = "";
        }

        #endregion
    }
}
