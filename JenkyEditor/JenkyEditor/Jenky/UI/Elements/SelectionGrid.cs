using System;
using System.Collections.Generic;
using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class SelectionGrid : UIPanel
    {
        #region vars

        private int rows;
        private int columns;

        private int spacingX;
        private int spacingY;

        private int iconWidth;
        private int iconHeight;

        private int physicalIconWidth;
        private int physicalIconHeight;

        public int selectedID;

        private Rectangle wrapBounds;

        private Rectangle selectionBounds;
        private Texture2D iconTexture;
        private Texture2D lineTexture;
        private Color lineColor;

        private int lineThickness;

        private SortedDictionary<int,Selectable> items;
        InputHandler input;

        #endregion

        #region init

        public SelectionGrid(int positionX, int positionY, int width, int height, int scale, int _iconWidth, int _iconHeight, int _columns, int _rows, InputHandler _input, Texture2D _iconTexture, Texture2D _lineTexture, Color _lineColor, int _lineThickness = 2) : base(positionX, positionY, width, height, scale)
        {
            input = _input;

            iconTexture = _iconTexture;
            lineTexture = _lineTexture;

            iconWidth = _iconWidth;
            iconHeight = _iconHeight;

            columns = _columns;
            rows = _rows;

            physicalIconWidth = iconWidth * scale;
            physicalIconHeight = iconHeight * scale;

            lineColor = _lineColor;
            lineThickness = _lineThickness;

            items = new SortedDictionary<int, Selectable>();

            wrapBounds = new Rectangle((int)position.X + lineThickness, (int)position.Y + lineThickness, physicalWidth - (lineThickness * 2), physicalHeight - (lineThickness * 2));
            selectionBounds = new Rectangle(0, 0, physicalIconWidth, physicalIconHeight);

            selectedID = -1;

            GetSpacing();
        }

        #endregion

        #region methods

        public void Clear()
        {
            items.Clear();
            selectedID = -1;
        }

        private Vector2 GetNextPosition()
        {
            int currentColumn = 0;
            int currentRow = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                {
                    currentColumn++;
                }
            }

            return new Vector2(wrapBounds.X + (currentColumn * (physicalIconWidth + spacingX)), wrapBounds.Y + (currentRow * (physicalIconHeight + spacingY)));

        }

        public void InsertItem(int itemID, StillFrame icon)
        {
            items.Add(itemID, new Selectable(0, 0, iconWidth, iconHeight, scale, itemID, iconTexture, icon));
            Refresh();
        }

        public void RemoveItem(int itemID)
        {
            foreach (KeyValuePair<int, Selectable> item in items)
            {
                if (item.Value.itemID == itemID)
                {
                    items.Remove(itemID);
                    Refresh();
                    return;
                }
            }
        }

        private void Refresh()
        {
            int currentColumn = 0;
            int currentRow = 0;
            
            foreach(KeyValuePair<int, Selectable> item in items)
            {
                item.Value.RePosition(new Vector2(wrapBounds.X + (currentColumn * (physicalIconWidth + spacingX)), wrapBounds.Y + (currentRow * (physicalIconHeight + spacingY))));

                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                {
                    currentColumn++;
                }
            }

            selectedID = -1;
        }

        private void GetSpacing()
        {
            int freespace;
            //Get x spacing
            int combinedWidth = physicalIconWidth * columns;
            freespace = wrapBounds.Width - combinedWidth;

            spacingX = freespace / (columns - 1);

            //Get y spacing
            int combinedHeight = physicalIconHeight * rows;
            freespace = wrapBounds.Height - combinedHeight;

            spacingY = freespace / (rows - 1);
        }

        public void CheckSelection()
        {
            var mousePosition = input.MousePosition();

            foreach (KeyValuePair<int, Selectable> item in items)
            {
                if (item.Value.InBounds(mousePosition))
                {
                    HighlightSelection(item.Value);
                    selectedID = item.Value.itemID;
                }
            }
        }

        private void HighlightSelection(Selectable item)
        {
            selectionBounds = new Rectangle((int)item.position.X, (int)item.position.Y, item.GetPhysicalWidth(), item.GetPhysicalHeight());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<int, Selectable> item in items)
            {
                item.Value.Draw(spriteBatch);
            }

            if(selectedID >= 0)
            {
                //drawing selection
                spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y - lineThickness, physicalIconWidth + (lineThickness * 2), lineThickness), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y + physicalIconHeight, physicalIconWidth + (lineThickness * 2), lineThickness), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X + physicalIconWidth, selectionBounds.Y - lineThickness, lineThickness, physicalIconHeight + (lineThickness * 2)), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y - lineThickness, lineThickness, physicalIconHeight + (lineThickness * 2)), lineColor);
            }
        }

        public void VisualDebug(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, physicalWidth, 1), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y + physicalHeight - 1, physicalWidth, 1), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth - 1, (int)position.Y, 1, physicalHeight), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, 1, physicalHeight), Color.Red);

            spriteBatch.Draw(lineTexture, new Rectangle(wrapBounds.X, wrapBounds.Y, wrapBounds.Width, 1), Color.Blue);
            spriteBatch.Draw(lineTexture, new Rectangle(wrapBounds.X, wrapBounds.Y + wrapBounds.Height - 1, wrapBounds.Width, 1), Color.Blue);
            spriteBatch.Draw(lineTexture, new Rectangle(wrapBounds.X + wrapBounds.Width - 1, wrapBounds.Y, 1, wrapBounds.Height), Color.Blue);
            spriteBatch.Draw(lineTexture, new Rectangle(wrapBounds.X, wrapBounds.Y, 1, wrapBounds.Height), Color.Blue);
        }

        #endregion 
    }
}
