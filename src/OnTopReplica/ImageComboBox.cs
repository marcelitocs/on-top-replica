using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {
    /// <summary>
    /// A combo box that can display images.
    /// </summary>
    class ImageComboBox : ComboBox {

        /// <summary>
        /// Creates a new instance of the image combo box.
        /// </summary>
        public ImageComboBox() {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Overridden. Draws the items in the combo box.
        /// </summary>
        /// <param name="ea">Draw item event arguments.</param>
        protected override void OnDrawItem(DrawItemEventArgs ea) {
            ea.DrawBackground();
            ea.DrawFocusRectangle();

            if (ea.Index == -1)
                return;

            Rectangle bounds = ea.Bounds;
            var foreBrush = new SolidBrush(ea.ForeColor);
            int textLeftBound = (IconList == null) ? bounds.Left : bounds.Left + IconList.ImageSize.Width;

            var drawObject = Items[ea.Index];
            if (drawObject is ImageComboBoxItem) {
                var drawItem = (ImageComboBoxItem)drawObject;

                if (drawItem.ImageListIndex != -1 && IconList != null) {
                    //ea.Graphics.FillRectangle(Brushes.Gray, bounds.Left, bounds.Top, IconList.ImageSize.Width, IconList.ImageSize.Height);
                    ea.Graphics.DrawImage(IconList.Images[drawItem.ImageListIndex], bounds.Left, bounds.Top);
                }

                ea.Graphics.DrawString(drawItem.Text, ea.Font, foreBrush, textLeftBound, bounds.Top);
            }
            else {
                ea.Graphics.DrawString(drawObject.ToString(), ea.Font, foreBrush, textLeftBound, bounds.Top);
            }

            base.OnDrawItem(ea);
        }

        /// <summary>
        /// Gets or sets the image list to use for the icons.
        /// </summary>
        public ImageList IconList { get; set; }

    }

    /// <summary>
    /// An item in an image combo box.
    /// </summary>
    class ImageComboBoxItem {

        /// <summary>
        /// Creates a new instance of the image combo box item.
        /// </summary>
        public ImageComboBoxItem() {
            Text = "";
            ImageListIndex = -1;
        }

        /// <summary>
        /// Creates a new instance of the image combo box item.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public ImageComboBoxItem(string text) {
            if (text == null)
                throw new ArgumentNullException();

            Text = text;
            ImageListIndex = -1;
        }

        /// <summary>
        /// Creates a new instance of the image combo box item.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="imageListIndex">The index of the image in the image list.</param>
        public ImageComboBoxItem(string text, int imageListIndex) {
            if (text == null)
                throw new ArgumentNullException();

            Text = text;
            ImageListIndex = imageListIndex;
        }

        /// <summary>
        /// Gets the text to display.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the index of the image in the image list.
        /// </summary>
        public int ImageListIndex { get; private set; }

        /// <summary>
        /// Gets or sets the tag object.
        /// </summary>
        public object Tag { get; set; }

    }
}
