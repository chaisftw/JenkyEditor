using System;
using System.Collections.Generic;
using Jenky.Graphics;
using Jenky.UI;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JenkyEditor
{
    public class ItemSelectionGrid : UIPanel
    {
        #region vars

        private bool hideSelection;

        private int rows;
        private int columns;

        private int spacingX;
        private int spacingY;

        private int iconWidth;
        private int iconHeight;

        private int physicalIconWidth;
        private int physicalIconHeight;

        public int SelectedID { get; private set; }
        
        private int selectableOffset;
        private int selectableMax;

        private Rectangle wrapBounds;

        private Rectangle selectionBounds;

        private InputHandler input;

        private Texture2D itemTexture;
        private Texture2D lineTexture;

        private SpriteFont font;

        private Color lineColor;
        private Color backgroundColor;
        private int lineThickness;

        private List<ItemSelectable> items;
        
        #endregion 

        #region init

        public ItemSelectionGrid(int positionX, int positionY, int width, int height, int scale, int _iconWidth, int _iconHeight, int _columns, int _rows, Texture2D _itemTexture, Texture2D _lineTexture, SpriteFont _font, Color _backgroundColor,  Color _lineColor, InputHandler _input, int _lineThickness = 2) : base(positionX, positionY, width, height, scale)
        {
            input = _input;
            font = _font;

            itemTexture = _itemTexture;
            lineTexture = _lineTexture;

            iconWidth = _iconWidth;
            iconHeight = _iconHeight;

            columns = _columns;
            rows = _rows;

            physicalIconWidth = iconWidth * scale;
            physicalIconHeight = iconHeight * scale;

            lineColor = _lineColor;
            backgroundColor = _backgroundColor;
            lineThickness = _lineThickness;

            items = new List<ItemSelectable>();

            wrapBounds = new Rectangle((int)position.X + lineThickness, (int)position.Y + lineThickness, physicalWidth - (lineThickness * 2), physicalHeight - (lineThickness * 2));
            selectionBounds = new Rectangle(0, 0, physicalIconWidth, physicalIconHeight);

            hideSelection = true;
            SelectedID = -1;
            selectableOffset = 0;

            GetSpacing();
        }

        #endregion 

        #region methods

        public void Clear()
        {
            items.Clear();
            hideSelection = true;
            SelectedID = -1;
            selectableOffset = 0;
            SetMax();
        }

        public void Deselect()
        {
            SelectedID = -1;
        }

        public void InsertItem(int itemID, string itemName, StillFrame icon)
        {
            items.Add(new ItemSelectable(0, 0, iconWidth, iconHeight, scale, itemID, itemName, icon, itemTexture, lineTexture, font, backgroundColor, lineColor, input));
            items.Sort((x, y) => x.itemID.CompareTo(y.itemID));
            Refresh();
        }

        public void RemoveItem(int itemID)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemID == itemID)
                {
                    items.RemoveAt(i);
                    items.Sort((x, y) => x.itemID.CompareTo(y.itemID));
                    SelectedID = -1;
                    Refresh();
                    break;
                }
            }
        }

        private void SetMax()
        {
            int gridSpace = columns * rows;

            if (items.Count - selectableOffset < gridSpace)
            {
                selectableMax = items.Count;
            }
            else
            {
                selectableMax = selectableOffset + gridSpace;
            }
        }

        private void Refresh()
        {
            int currentColumn = 0;
            int currentRow = 0;

            SetMax();
            hideSelection = true;

            for (int i = selectableOffset; i < selectableMax; i++)
            {
                items[i].RePosition(new Vector2(wrapBounds.X + (currentColumn * (physicalIconWidth + spacingX)), wrapBounds.Y + (currentRow * (physicalIconHeight + spacingY))), font);

                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                {
                    currentColumn++;
                }

                if(items[i].itemID == SelectedID)
                {
                    HighlightSelection(items[i]);
                }
            }
        }

        public void ScrollUp()
        {
            if(selectableOffset > columns)
            {
                selectableOffset -= columns;
                if(selectableOffset < 0)
                {
                    selectableOffset = 0;
                }

                Refresh();
            }
            else if(selectableOffset > 0)
            {
                selectableOffset = 0;

                Refresh();
            }
        }

        public void ScrollDown()
        {
            int gridSpace = columns * rows;
            int remainder = items.Count % columns;

            if (items.Count - selectableOffset > gridSpace)
            {
                selectableOffset += columns;
                Refresh();
            }

        }

        public void GetSpacing()
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

            for (int i = selectableOffset; i < selectableMax; i++)
            {
                if (items[i].InBounds(mousePosition))
                {
                    HighlightSelection(items[i]);
                    SelectedID = items[i].itemID;
                }
            }
        }

        private void HighlightSelection(ItemSelectable item)
        {
            hideSelection = false;
            selectionBounds = new Rectangle((int)item.position.X, (int)item.position.Y, item.GetPhysicalWidth(), item.GetPhysicalHeight());
        }

        public void SetItemTexture(Texture2D newTexture)
        {
            itemTexture = newTexture;

            foreach (ItemSelectable item in items)
            {
                item.SetItemTexture(newTexture);
            }
        }

        public void Update()
        {
            for (int i = selectableOffset; i < selectableMax; i++)
            {
                items[i].Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(SelectedID > -1)
            {
                if (!hideSelection)
                {
                    //drawing selection
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y - lineThickness, physicalIconWidth + (lineThickness * 2), lineThickness), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y + physicalIconHeight, physicalIconWidth + (lineThickness * 2), lineThickness), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X + physicalIconWidth, selectionBounds.Y - lineThickness, lineThickness, physicalIconHeight + (lineThickness * 2)), Color.White);
                    spriteBatch.Draw(lineTexture, new Rectangle(selectionBounds.X - lineThickness, selectionBounds.Y - lineThickness, lineThickness, physicalIconHeight + (lineThickness * 2)), Color.White);
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


            for (int i = selectableOffset; i < selectableMax; i++)
            {
                items[i].Draw(spriteBatch);
            }
        }

        public void DrawTooltip(SpriteBatch spriteBatch)
        {
            for (int i = selectableOffset; i < selectableMax; i++)
            {
                items[i].DrawTooltip(spriteBatch, SelectedID);
            }
        }

        #endregion 
    }
}
