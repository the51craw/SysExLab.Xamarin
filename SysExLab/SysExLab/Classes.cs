using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SysExLab
{
    /// <summary>
    /// XAML layout classes
    /// </summary>
    
    class GridRow
    {
        public Grid Row { get; set; }
        public Grid[] Columns { get; set; }

        public GridRow(byte row, View[] controls = null, byte[] columnWiths = null, Boolean KeepAlignment = false)
        {
            try
            {
                Row = new Grid();
                Grid.SetRow(Row, row);
                Row.MinimumHeightRequest = 50;
                Row.SetValue(Grid.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                Row.SetValue(Grid.VerticalOptionsProperty, LayoutOptions.Start);
                Row.SetValue(Grid.ColumnSpacingProperty, 0);
                Row.SetValue(Grid.RowSpacingProperty, 0);
                Row.SetValue(Grid.PaddingProperty, 0);
                Row.SetValue(Grid.MarginProperty, 0);
                ColumnDefinition[] columnDefinitions = new ColumnDefinition[controls.Length];

                if (controls != null)
                {
                    Columns = new Grid[controls.Length];
                    for (byte i = 0; i < controls.Length; i++)
                    {
                        columnDefinitions[i] = new ColumnDefinition();
                        if (columnWiths == null || columnWiths.Length < i - 1)
                        {
                            columnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                        }
                        else
                        {
                            columnDefinitions[i].Width = new GridLength(columnWiths[i], GridUnitType.Star);
                        }
                        Row.ColumnDefinitions.Add(columnDefinitions[i]);
                        if (!KeepAlignment)
                        {
                            if (controls[i].GetType() == typeof(Button))
                            {
                                controls[i].SetValue(Button.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Button.VerticalOptionsProperty, LayoutOptions.Start);
                                controls[i].SetValue(Button.BorderWidthProperty, 1);
                                controls[i].SetValue(Button.BorderColorProperty, Color.Black);
                            }
                            else if (controls[i].GetType() == typeof(Switch))
                            {
                                controls[i].SetValue(Switch.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Switch.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            else if (controls[i].GetType() == typeof(LabeledSwitch))
                            {
                                controls[i].SetValue(LabeledSwitch.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledSwitch.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            else if (controls[i].GetType() == typeof(ListView))
                            {
                                controls[i].SetValue(ListView.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(ListView.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                            }
                            else if (controls[i].GetType() == typeof(Picker))
                            {
                                controls[i].SetValue(Picker.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Picker.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            else if (controls[i].GetType() == typeof(Label))
                            {
                                controls[i].SetValue(Label.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
                                controls[i].SetValue(Label.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            else if (controls[i].GetType() == typeof(Editor))
                            {
                                controls[i].SetValue(Editor.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Editor.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            else if (controls[i].GetType() == typeof(Image))
                            {
                                controls[i].SetValue(Image.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Image.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                            }
                            else if (controls[i].GetType() == typeof(LabeledText))
                            {
                                controls[i].SetValue(LabeledText.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledText.VerticalOptionsProperty, LayoutOptions.Start);
                                controls[i].SetValue(LabeledText.BackgroundColorProperty, Color.Red);
                            }
                            else if (controls[i].GetType() == typeof(LabeledTextInput))
                            {
                                controls[i].SetValue(LabeledTextInput.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(LabeledTextInput.VerticalOptionsProperty, LayoutOptions.Start);
                                controls[i].SetValue(LabeledTextInput.BackgroundColorProperty, Color.Red);
                            }
                            else if (controls[i].GetType() == typeof(Grid))
                            {
                                controls[i].SetValue(Grid.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                                controls[i].SetValue(Grid.VerticalOptionsProperty, LayoutOptions.Start);
                            }
                            if (i == 0)
                            {
                                controls[i].SetValue(View.MarginProperty, new Thickness(1));
                            }
                            else
                            {
                                controls[i].SetValue(View.MarginProperty, new Thickness(0, 1, 1, 1));
                            }
                        }
                        //controls[i].SetValue(Grid.BackgroundColorProperty, Color.Aquamarine);
                        //controls[i].SetValue(Grid.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                        //controls[i].SetValue(Grid.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
                        //controls[i].SetValue(Grid.MarginProperty, 0);
                        //controls[i].SetValue(Grid.PaddingProperty, 0);
                        //controls[i].SetValue(Grid.ColumnSpacingProperty, 0);
                        //controls[i].SetValue(Grid.RowSpacingProperty, 0);
                        controls[i].SetValue(Grid.ColumnProperty, i);
                        //Columns[i] = new Grid();
                        //Columns[i].SetValue(Grid.ColumnProperty, i);
                        //Columns[i].SetValue(Grid.MarginProperty, new Thickness(0));
                        //Columns[i].SetValue(Grid.ColumnSpacingProperty, 1);
                        //Columns[i].SetValue(Grid.BackgroundColorProperty, Color.AntiqueWhite);
                        //try
                        //{
                        //    Columns[i].Children.Add(controls[i]);
                        //}
                        //catch (Exception e)
                        //{
                        //    GC.Collect(10, GCCollectionMode.Forced);
                        //    Columns[i].Children.Add(controls[i]);
                        //}
                        try
                        {
                            //Row.Children.Add(Columns[i]);
                            Row.Children.Add(controls[i]);
                        }
                        catch (Exception e)
                        {
                            GC.Collect(10, GCCollectionMode.Forced);
                            Row.Children.Add(Columns[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    public class Hex2Midi
    {
        /// <summary>
        ///  In MIDI msb is not allowed for data, and addresses are sent as data.
        ///  This function helps adding two addresses with arbitrary number of bytes
        ///  taking into consideration that the values may only be 0 - 0x7f (0 - 127).
        ///  However, max number of bytes are 4, and the second argument must contain 
        ///  the same byte-count as the first argument.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="addition"></param>
        /// <returns></returns>
        public byte[] AddBytes128(byte[] arg1, byte[] arg2)
        {
            if (arg1.Length < arg2.Length)
            {
                return null;
            }
            if (arg1.Length > arg2.Length)
            {
                byte diff = (byte)(arg1.Length - arg2.Length);
                byte[] b = new byte[arg1.Length];
                for (byte i = diff; i < (byte)arg1.Length; i++)
                {
                    b[i] = arg2[i - diff];
                    //if (!(arg2.Length < 4 - i))
                    //{
                    //    b[i] = arg2[arg2.Length - i];
                    //}
                }
                arg2 = b;
                //arg2 = (byte[])b.Concat(arg2.AsEnumerable());
            }
            byte[] result = new byte[arg1.Length];
            UInt16[] temp = new UInt16[arg1.Length];
            for (byte i = 0; i < arg1.Length; i++)
            {
                temp[i] = (UInt16)(arg1[i] + arg2[i]);
            }

            for (byte i = (byte)(temp.Length - 1); i > 0; i--)
            {
                if (temp[i] > 127)
                {
                    if (i > 0)
                    {
                        temp[i - 1] += (UInt16)(temp[i] / 128);
                    }
                    temp[i] = (UInt16)(temp[i] % 128);
                }
            }

            for (byte i = 0; i < arg1.Length; i++)
            {
                result[i] = (byte)(temp[i]);
            }
            return result;
        }
    }
}
