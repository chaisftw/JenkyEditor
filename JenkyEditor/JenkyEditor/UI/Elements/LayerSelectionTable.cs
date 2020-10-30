using System;
using System.Collections.Generic;
using Jenky.Graphics;
using Jenky.UI;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JenkyEditor
{
    public class LayerSelectionTable : UIPanel
    {
        #region vars

        private int rows;
        
        private int spacing;

        private int cellWidth;
        private int cellHeight;

        private int physicalCellWidth;
        private int physicalCellHeight;
        
        public int SelectedID { get; private set; }

        private int selectableOffset;
        private int selectableMax;

        private bool hideSelection;

        private Rectangle selectionBounds;

        private InputHandler input;

        private Texture2D uiTexture;
        private Texture2D lineTexture;

        private SpriteFont font;

        private Color lineColor;
        private Color backgroundColor;
        private int lineThickness;

        private List<LayerCell> layers;

        private Action<int> HideLayer;
        private Action<int> ShowLayer;

        #endregion 

        #region init

        public LayerSelectionTable(int positionX, int positionY, int width, int height, int scale, int _rows, Action<int> _HideLayer, Action<int> _ShowLayer, Texture2D _uiTexture, Texture2D _lineTexture, SpriteFont _font, Color _backgroundColor,  Color _lineColor, InputHandler _input) : base(positionX, positionY, width, height, scale)
        {
            input = _input;
            font = _font;

            uiTexture = _uiTexture;
            lineTexture = _lineTexture;

            cellWidth = width;
            cellHeight = 16;
            rows = _rows;

            physicalCellWidth = cellWidth * scale;
            physicalCellHeight = cellHeight * scale;

            lineColor = _lineColor;
            backgroundColor = _backgroundColor;
            lineThickness = 2;

            layers = new List<LayerCell>();

            selectionBounds = new Rectangle(0, 0, physicalCellWidth, physicalCellHeight);

            hideSelection = true;
            SelectedID = -1;
            selectableOffset = 0;

            HideLayer = _HideLayer;
            ShowLayer = _ShowLayer;

            GetSpacing();
        }

        #endregion 

        #region methods

        public void Clear()
        {
            layers.Clear();
            SelectedID = -1;
            SetMax();
        }

        public void Deselect()
        {
            SelectedID = -1;
        }

        public void InsertLayer(string name)
        {
            layers.Add(new LayerCell(0, 0, cellWidth, cellHeight, scale, name, HideLayer, ShowLayer, uiTexture, lineTexture, font, lineColor, input));
            Refresh();
        }

        public void RemoveLayer(int depth)
        {
            layers.RemoveAt(depth);
            SelectedID = -1;
            Refresh();
        }

        public void RaiseLayer(int depth)
        {
            if (depth < layers.Count - 1)
            {
                var temp = layers[depth + 1];
                layers[depth + 1] = layers[depth];
                layers[depth] = temp;

                SelectedID++;
                Refresh();
            }
        }

        public void LowerLayer(int depth)
        {
            if (depth > 0)
            {
                var temp = layers[depth - 1];
                layers[depth - 1] = layers[depth];
                layers[depth] = temp;

                SelectedID--;
                Refresh();
            }

        }

        private void SetMax()
        {

            if (layers.Count - selectableOffset < rows)
            {
                selectableMax = layers.Count;
            }
            else
            {
                selectableMax = selectableOffset + rows;
            }
        }

        private void Refresh()
        {
            SetMax();
            hideSelection = true;
            int currentRow = selectableMax - selectableOffset - 1;
            for (int i = selectableOffset; i < selectableMax; i++)
            {
                layers[i].RePosition(new Vector2(position.X, position.Y + (currentRow * (physicalCellHeight + spacing))));
                
                if (i == SelectedID)
                {
                    HighlightSelection(layers[i]);
                }

                currentRow--;
            }
        }

        public void ScrollDown()
        {
            if (selectableOffset > 0)
            {
                selectableOffset--;
                Refresh();
            }
        }

        public void ScrollUp()
        {
            if (layers.Count - selectableOffset > rows)
            {
                selectableOffset++;
                Refresh();
            }

        }

        public void GetSpacing()
        {
            //Get spacing
            int combinedHeight = physicalCellHeight * rows;
            int freespace = physicalHeight - combinedHeight;

            spacing = freespace / (rows - 1);
        }

        public void CheckSelection()
        {
            var mousePosition = input.MousePosition();

            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].CheckSelection(mousePosition, i))
                {
                    HighlightSelection(layers[i]);
                    SelectedID = i;
                }
            }
        }

        private void HighlightSelection(LayerCell layer)
        {
            hideSelection = false;
            selectionBounds = new Rectangle((int)layer.position.X, (int)layer.position.Y, layer.GetPhysicalWidth(), layer.GetPhysicalHeight());
        }

        public void Update()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = selectableOffset; i < selectableMax; i++)
            {
                layers[i].Draw(spriteBatch);
            }

            if (SelectedID > -1)
            {
                if (!hideSelection)
                {
                    //drawing selection
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X, selectionBounds.Y, physicalCellWidth, lineThickness), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X, selectionBounds.Y + physicalCellHeight - 2, physicalCellWidth, lineThickness), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X + physicalCellWidth - 2, selectionBounds.Y, lineThickness, physicalCellHeight), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X, selectionBounds.Y, lineThickness, physicalCellHeight), Color.White);
                }

                //drawing border
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 2, 1), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), Color.White);
            }
            else
            {
                //drawing border
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 2, 1), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), lineColor);
            }
        }

        #endregion 
    }
}
