﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AudioBand.Extensions;
using AudioBand.Resources;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Custom button.
    /// </summary>
    public class CustomButton : AudioBandControl
    {
        private IImage _image;
        private Color _defaultBackColor = Color.Transparent;
        private IImage _currentImage;

        /// <summary>
        /// Gets or sets the button's image.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public IImage Image
        {
            get => _image;
            set
            {
                _image = value;
                _currentImage = value;
                InvokeRefresh(); // refresh because when the image changes, the button will be in a normal state
            }
        }

        /// <summary>
        /// Gets or sets the buttons image when hovered.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public IImage HoveredImage { get; set; }

        /// <summary>
        /// Gets or sets the buttons image when clicked.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public IImage ClickedImage { get; set; }

        /// <summary>
        /// Gets or sets the default back color.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color DefaultBackgroundColor
        {
            get => _defaultBackColor;
            set
            {
                _defaultBackColor = value;
                BackColor = _defaultBackColor;
                InvokeRefresh();
            }
        }

        /// <summary>
        /// Gets or sets the hover back color.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color HoveredBackgroundColor { get; set; } = Color.FromArgb(25, 255, 255, 255);

        /// <summary>
        /// Gets or sets the clicked back color.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color ClickedBackgroundColor { get; set; } = Color.FromArgb(15, 255, 255, 255);

        /// <inheritdoc/>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _currentImage = HoveredImage;
            BackColor = HoveredBackgroundColor;
            InvokeRefresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _currentImage = Image;
            BackColor = DefaultBackgroundColor;
            InvokeRefresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!e.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            _currentImage = ClickedImage;
            BackColor = ClickedBackgroundColor;
            InvokeRefresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!e.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            _currentImage = HoveredImage;
            BackColor = HoveredBackgroundColor;
            InvokeRefresh();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var image = _currentImage.GetScaledSize(Width, Height))
            {
                var centerX = (e.ClipRectangle.Width - image.Width) / 2;
                var centerY = (e.ClipRectangle.Height - image.Height) / 2;
                graphics.DrawImageUnscaled(image, new Point(centerX, centerY));
            }
        }
    }
}
