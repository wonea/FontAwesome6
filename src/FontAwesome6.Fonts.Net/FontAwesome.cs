﻿#if FontAwesomePro
using FontAwesome6.Extensions;
#endif
using FontAwesome6.Fonts.Extensions;
using FontAwesome6.Shared.Extensions;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FontAwesome6.Fonts
{
    /// <summary>
    /// Provides a lightweight control for displaying a FontAwesome icon as text.
    /// </summary>
    public class FontAwesome : TextBlock, ISpinable, IRotatable, IFlippable, IPulsable
    {
        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
                DependencyProperty.Register("Icon", typeof(EFontAwesomeIcon), typeof(FontAwesome), new PropertyMetadata(EFontAwesomeIcon.None, OnIconPropertyChanged));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool), typeof(FontAwesome), new PropertyMetadata(false, OnSpinPropertyChanged, SpinCoerceValue));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.SpinDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinDurationProperty =
            DependencyProperty.Register("SpinDuration", typeof(double), typeof(FontAwesome), new PropertyMetadata(1d, SpinDurationChanged, SpinDurationCoerceValue));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Pulse dependency property.
        /// </summary>
        public static readonly DependencyProperty PulseProperty =
            DependencyProperty.Register("Pulse", typeof(bool), typeof(FontAwesome), new PropertyMetadata(false, OnPulsePropertyChanged, PulseCoerceValue));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.PulseDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty PulseDurationProperty =
            DependencyProperty.Register("PulseDuration", typeof(double), typeof(FontAwesome), new PropertyMetadata(1d, PulseDurationChanged, PulseDurationCoerceValue));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Rotation dependency property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(FontAwesome), new PropertyMetadata(0d, RotationChanged, RotationCoerceValue));

        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.FlipOrientation dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipOrientationProperty =
            DependencyProperty.Register("FlipOrientation", typeof(EFlipOrientation), typeof(FontAwesome), new PropertyMetadata(EFlipOrientation.Normal, FlipOrientationChanged));

        private readonly FontFamily _initialFontFamily;

        static FontAwesome()
        {
            OpacityProperty.OverrideMetadata(typeof(FontAwesome), new UIPropertyMetadata(1.0, OpacityChanged));
        }

        public FontAwesome()
        {
            _initialFontFamily = FontFamily;
            IsVisibleChanged += (s, a) => CoerceValue(SpinProperty);
        }

        private static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SpinProperty);
        }

        /// <summary>
        /// Gets or sets the FontAwesome icon. Changing this property will cause the icon to be redrawn.
        /// </summary>
        public EFontAwesomeIcon Icon
        {
            get { return (EFontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome)
            {
                return;
            }

            var icon = (EFontAwesomeIcon)d.GetValue(IconProperty);            

#if FontAwesomePro
            if (icon.IsDuotone())
            {
                return;
            }
#endif

#if NET40
            d.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
#endif            
            d.SetValue(FontFamilyProperty, icon.GetFontFamily() ?? fontAwesome._initialFontFamily);
            d.SetValue(TextProperty, icon.GetUnicode());
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
        }

        /// <summary>
        /// Gets or sets the current spin (angle) animation of the icon.
        /// </summary>
        public bool Spin
        {
            get { return (bool)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                fontAwesome.BeginSpin();
            }
            else
            {
                fontAwesome.StopSpin();
                fontAwesome.SetRotation();
            }
        }

        private static object SpinCoerceValue(DependencyObject d, object basevalue)
        {
            var fontAwesome = (FontAwesome)d;

            return !fontAwesome.IsVisible || fontAwesome.Opacity == 0.0 || fontAwesome.SpinDuration == 0.0 ? false : basevalue;
        }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will stop and start the spin animation.
        /// </summary>
        public double SpinDuration
        {
            get { return (double)GetValue(SpinDurationProperty); }
            set { SetValue(SpinDurationProperty, value); }
        }

        private static void SpinDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome || !fontAwesome.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
            {
                return;
            }

            fontAwesome.StopSpin();
            fontAwesome.BeginSpin();
        }

        private static object SpinDurationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : value;
        }

        /// <summary>
        /// Gets or sets the current pulse animation of the icon.
        /// </summary>
        public bool Pulse
        {
            get { return (bool)GetValue(PulseProperty); }
            set { SetValue(PulseProperty, value); }
        }

        private static void OnPulsePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                fontAwesome.BeginPulse();
            }
            else
            {
                fontAwesome.StopPulse();
                fontAwesome.SetRotation();
            }
        }

        private static object PulseCoerceValue(DependencyObject d, object basevalue)
        {
            var fontAwesome = (FontAwesome)d;

            return !fontAwesome.IsVisible || fontAwesome.Opacity == 0.0 || fontAwesome.PulseDuration == 0.0 ? false : basevalue;
        }

        /// <summary>
        /// Gets or sets the duration of the pulse animation (in seconds). This will stop and start the pulse animation.
        /// </summary>
        public double PulseDuration
        {
            get { return (double)GetValue(PulseDurationProperty); }
            set { SetValue(PulseDurationProperty, value); }
        }

        private static void PulseDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome || !fontAwesome.Pulse || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
            {
                return;
            }

            fontAwesome.StopPulse();
            fontAwesome.BeginPulse();
        }

        private static object PulseDurationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : value;
        }

        /// <summary>
        /// Gets or sets the current rotation (angle).
        /// </summary>
        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        private static void RotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome || fontAwesome.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
            {
                return;
            }

            fontAwesome.SetRotation();
        }

        private static object RotationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : (val > 360 ? 360d : value);
        }

        /// <summary>
        /// Gets or sets the current orientation (horizontal, vertical).
        /// </summary>
        public EFlipOrientation FlipOrientation
        {
            get { return (EFlipOrientation)GetValue(FlipOrientationProperty); }
            set { SetValue(FlipOrientationProperty, value); }
        }

        private static void FlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FontAwesome fontAwesome || !(e.NewValue is EFlipOrientation) || e.NewValue.Equals(e.OldValue))
            {
                return;
            }

            fontAwesome.SetFlipOrientation();
        }
    }
}
