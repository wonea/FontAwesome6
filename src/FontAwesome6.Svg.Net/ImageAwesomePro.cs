﻿#if FontAwesomePro
using FontAwesome6;
using FontAwesome6.Extensions;
using FontAwesome6.Svg.Extensions;
using FontAwesome6.Shared.Extensions;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FontAwesome6.Svg
{
  /// <summary>
  /// Represents a control that draws an FontAwesome icon as an image.
  /// </summary>
  public partial class ImageAwesome
      : Image, ISpinable, IRotatable, IFlippable, IPulsable
  {
    /// <summary>
    /// Identifies the FontAwesome6.Svg.ImageAwesome.SecondaryColor dependency property.
    /// </summary>
    public static readonly DependencyProperty SecondaryColorProperty =
        DependencyProperty.Register("SecondaryColor", typeof(Brush), typeof(ImageAwesome), new PropertyMetadata(Brushes.Black, OnIconPropertyChanged));

    /// <summary>
    /// Identifies the FontAwesome6.Svg.ImageAwesome.PrimaryOpacity dependency property.
    /// </summary>
    public static readonly DependencyProperty PrimaryOpacityProperty =
        DependencyProperty.Register("PrimaryOpacity", typeof(double?), typeof(ImageAwesome), new PropertyMetadata(null, OnIconPropertyChanged));

    /// <summary>
    /// Identifies the FontAwesome6.Svg.ImageAwesome.SecondaryOpacity daryColor dependency property.
    /// </summary>
    public static readonly DependencyProperty SecondaryOpacityProperty =
        DependencyProperty.Register("SecondaryOpacity", typeof(double?), typeof(ImageAwesome), new PropertyMetadata(null, OnIconPropertyChanged));

    /// <summary>
    /// Identifies the FontAwesome6.Svg.ImageAwesome.SwapOpacity daryColor dependency property.
    /// </summary>
    public static readonly DependencyProperty SwapOpacityProperty =
        DependencyProperty.Register("SwapOpacity", typeof(bool?), typeof(ImageAwesome), new PropertyMetadata(null, OnIconPropertyChanged));

    /// <summary>
    /// Gets or sets the secondary brush of the icon. Changing this property will cause the icon to be redrawn.
    /// Duotone icons only!
    /// </summary>
    public Brush SecondaryColor
    {
      get { return (Brush)GetValue(SecondaryColorProperty); }
      set { SetValue(SecondaryColorProperty, value); }
    }


    /// <summary>
    /// Gets or sets the primary opacity of the icon. Changing this property will cause the icon to be redrawn.
    /// Duotone icons only!
    /// </summary>
    public double? PrimaryOpacity
    {
      get { return (double?)GetValue(PrimaryOpacityProperty); }
      set { SetValue(PrimaryOpacityProperty, value); }
    }

    /// <summary>
    /// Gets or sets the secondary opacity of the icon. Changing this property will cause the icon to be redrawn.
    /// Duotone icons only!
    /// </summary>
    public double? SecondaryOpacity
    {
      get { return (double?)GetValue(SecondaryOpacityProperty); }
      set { SetValue(SecondaryOpacityProperty, value); }
    }


    /// <summary>
    /// Gets or sets the swap opacity setting of the icon. Changing this property will cause the icon to be redrawn.
    /// Duotone icons only!
    /// </summary>
    public bool? SwapOpacity
    {
      get { return (bool?)GetValue(SwapOpacityProperty); }
      set { SetValue(SwapOpacityProperty, value); }
    }


    private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is not ImageAwesome control)
      {
        return;
      }

      if (control.Icon == EFontAwesomeIcon.None)
      {
        control.Source = null;
      }
      else
      {
        control.Source = control.Icon.CreateImageSource(control.PrimaryColor, control.SecondaryColor, control.SwapOpacity, control.PrimaryOpacity, control.SecondaryOpacity);
      }      
      
    }
  }
}
#endif