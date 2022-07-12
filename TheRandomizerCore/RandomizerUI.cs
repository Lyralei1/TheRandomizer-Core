using Sims3.SimIFace;
using Sims3.UI;
using System;
using System.Text;

namespace Lyralei.TheRandomizerCore
{
    public class ThreeButtonDialogRandomizer : ModalDialog
    {
        public enum ButtonPressed
        {
            FirstButton,
            SecondButton,
            ThirdButton
        }

        public enum ControlID : uint
        {
            kPromptTextID = 1u,
            kFirstButtonID,
            kSecondButtonID,
            kThirdButtonID
        }

        public const string kLayoutName = "ThreeButtonDialog";

        public const int kWinExportID = 65536;

        public ButtonPressed mResult;

        public ButtonPressed Result
        {
            get
            {
                return mResult;
            }
        }

        public static ButtonPressed Show(string promptText, string firstButton, string secondButton, string thirdButton)
        {
            return Show(promptText, firstButton, secondButton, thirdButton, new Vector2(-1f, -1f));
        }

        public static ButtonPressed Show(string promptText, string firstButton, string secondButton, string thirdButton, Vector2 position)
        {
            if (ModalDialog.EnableModalDialogs)
            {
                using (ThreeButtonDialogRandomizer threeButtonDialog = new ThreeButtonDialogRandomizer(promptText, firstButton, secondButton, thirdButton, position))
                {
                    threeButtonDialog.StartModal();
                    return threeButtonDialog.Result;
                }
            }
            return ButtonPressed.FirstButton;
        }

        public ThreeButtonDialogRandomizer(string promptText, string firstButtonCaption, string secondButtonCaption, string thirdButtonCaption, Vector2 position)
            : base("ThreeButtonDialog", 65536, true, PauseMode.PauseSimulator, null)
        {
            if ((WindowBase)base.mModalDialogWindow != (WindowBase)null)
            {
                Text text = base.mModalDialogWindow.GetChildByID(1u, false) as Text;
                if ((WindowBase)text != (WindowBase)null)
                {
                    text.Caption = promptText;
                }
                Button button = base.mModalDialogWindow.GetChildByID(2u, false) as Button;
                if ((WindowBase)button != (WindowBase)null)
                {
                    button.TooltipText = firstButtonCaption;
                    button.Caption = RandomizerUIUtils.CreateEllipsis(firstButtonCaption);
                    button.Click += OnButtonClick;
                }
                Button button2 = base.mModalDialogWindow.GetChildByID(3u, false) as Button;
                if ((WindowBase)button2 != (WindowBase)null)
                {
                    button2.TooltipText = secondButtonCaption;
                    button2.Caption = RandomizerUIUtils.CreateEllipsis(secondButtonCaption); 
                    button2.Click += OnButtonClick;
                }
                Button button3 = base.mModalDialogWindow.GetChildByID(4u, false) as Button;
                if ((WindowBase)button3 != (WindowBase)null)
                {
                    button3.TooltipText = thirdButtonCaption;
                    button3.Caption = RandomizerUIUtils.CreateEllipsis(thirdButtonCaption);
                    button3.Click += OnButtonClick;
                }
                Rect area = base.mModalDialogWindow.Area;
                float num = area.BottomRight.x - area.TopLeft.x;
                float num2 = area.BottomRight.y - area.TopLeft.y;
                float num3 = position.x;
                float num4 = position.y;
                if (num3 < 0f && num4 < 0f)
                {
                    Rect area2 = base.mModalDialogWindow.Parent.Area;
                    float num5 = area2.BottomRight.x - area2.TopLeft.x;
                    float num6 = area2.BottomRight.y - area2.TopLeft.y;
                    num3 = (float)Math.Round((double)((num5 - num) / 2f));
                    num4 = (float)Math.Round((double)((num6 - num2) / 2f));
                }
                area.Set(num3, num4, num3 + num, num4 + num2);
                base.mModalDialogWindow.Area = area;
                base.OkayID = 2u;
                base.CancelID = 4u;
                base.SelectedID = 2u;
            }
        }

        public void OnButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
        {
            if (eventArgs.ButtonID == 2)
            {
                mResult = ButtonPressed.FirstButton;
                eventArgs.Handled = true;
                EndDialog(2u);
            }
            else if (eventArgs.ButtonID == 3)
            {
                mResult = ButtonPressed.SecondButton;
                eventArgs.Handled = true;
                EndDialog(3u);
            }
            else if (eventArgs.ButtonID == 4)
            {
                mResult = ButtonPressed.ThirdButton;
                eventArgs.Handled = true;
                EndDialog(4u);
            }
        }

        public override void OnTriggerOk()
        {
            mResult = ButtonPressed.FirstButton;
            base.OnTriggerOk();
        }

        public override void OnTriggerCancel()
        {
            mResult = ButtonPressed.ThirdButton;
            base.OnTriggerCancel();
        }

        public override void OnTriggerForward()
        {
            if (base.SelectedID == 2)
            {
                base.SelectedID = 3u;
            }
            else if (base.SelectedID == 3)
            {
                base.SelectedID = 4u;
            }
            else if (base.SelectedID == 4)
            {
                base.SelectedID = 2u;
            }
        }

        public override void OnTriggerBackward()
        {
            if (base.SelectedID == 2)
            {
                base.SelectedID = 4u;
            }
            else if (base.SelectedID == 3)
            {
                base.SelectedID = 2u;
            }
            else if (base.SelectedID == 4)
            {
                base.SelectedID = 3u;
            }
        }
    

    }

    public class TwoButtonRandomizerDialog : ModalDialog
    {
        public enum ControlID : uint
        {
            kPromptTextID = 1u,
            kOKButtonID,
            kCancelButtonID
        }

        public const string kLayoutName = "TwoButtonDialog";

        public const int kWinExportID = 65536;

        public bool mResult;

        public bool Result
        {
            get
            {
                return mResult;
            }
        }

        public static bool Show(string promptText, string buttonTrue, string buttonFalse)
        {
            return Show(promptText, buttonTrue, buttonFalse, true, true, new Vector2(0f, 0f), false);
        }

        public static bool Show(string promptText, string buttonTrue, string buttonFalse, bool disableTooltip)
        {
            return Show(promptText, buttonTrue, buttonFalse, true, true, new Vector2(0f, 0f), disableTooltip);
        }

        public static bool Show(string promptText, string buttonTrue, string buttonFalse, Vector2 position)
        {
            return Show(promptText, buttonTrue, buttonFalse, true, true, position, false);
        }

        public static bool Show(string promptText, string buttonTrue, string buttonFalse, bool trueButtonEnabled, bool falseButtonEnabled, Vector2 position, bool disableTooltip)
        {
            if (ModalDialog.EnableModalDialogs)
            {
                using (TwoButtonRandomizerDialog twoButtonDialog = new TwoButtonRandomizerDialog(promptText, buttonTrue, buttonFalse, trueButtonEnabled, falseButtonEnabled, position, disableTooltip))
                {
                    twoButtonDialog.StartModal();
                    return twoButtonDialog.Result;
                }
            }
            return false;
        }

        public TwoButtonRandomizerDialog(string promptText, string trueButtonText, string falseButtonText, bool trueButtonEnabled, bool falseButtonEnabled, Vector2 position, bool disableTooltip)
            : this(promptText, trueButtonText, falseButtonText, trueButtonEnabled, falseButtonEnabled, position, disableTooltip, "TwoButtonDialog")
        {
        }

        public TwoButtonRandomizerDialog(string promptText, string trueButtonText, string falseButtonText, bool trueButtonEnabled, bool falseButtonEnabled, Vector2 position, bool disableTooltip, string layoutName)
            : base(layoutName, 65536, true, PauseMode.PauseSimulator, null, "ui_window_drop", "ui_hardwindow_close")
        {
            if ((WindowBase)base.mModalDialogWindow != (WindowBase)null)
            {
                float num = 0f;
                Text text = base.mModalDialogWindow.GetChildByID(1u, false) as Text;
                if ((WindowBase)text != (WindowBase)null)
                {
                    float height = text.Area.Height;
                    text.Caption = promptText;
                    text.AutoSize(true);
                    if (text.Area.Height > height)
                    {
                        num = text.Area.Height - height;
                    }
                    else
                    {
                        text.Area = new Rect(text.Area.TopLeft, new Vector2(text.Area.BottomRight.x, text.Area.TopLeft.y + height));
                    }
                }
                Button button = base.mModalDialogWindow.GetChildByID(2u, false) as Button;
                if ((WindowBase)button != (WindowBase)null)
                {
                    button.TooltipText = trueButtonText;
                    button.Caption = RandomizerUIUtils.CreateEllipsis(trueButtonText);
                    button.Enabled = trueButtonEnabled;

                }
                Button button2 = base.mModalDialogWindow.GetChildByID(3u, false) as Button;
                if ((WindowBase)button2 != (WindowBase)null)
                {
                    button2.TooltipText = falseButtonText;
                    button2.Caption = RandomizerUIUtils.CreateEllipsis(falseButtonText);
                    button2.Enabled = falseButtonEnabled;

                }
                Rect area = base.mModalDialogWindow.Area;
                area.Height += num;
                base.mModalDialogWindow.Area = area;
                if (position.x < 0f && position.y < 0f)
                {
                    base.mModalDialogWindow.CenterInParent();
                }
                else
                {
                    position.x = Math.Max(Math.Min(1f, position.x), -1f);
                    position.y = Math.Max(Math.Min(1f, position.y), -1f);
                    float num2 = area.BottomRight.x - area.TopLeft.x;
                    float num3 = area.BottomRight.y - area.TopLeft.y;
                    Rect area2 = base.mModalDialogWindow.Parent.Area;
                    float num4 = area2.BottomRight.x - area2.TopLeft.x;
                    float num5 = area2.BottomRight.y - area2.TopLeft.y;
                    float num6 = num5 * 0.5f;
                    float num7 = num4 * 0.5f;
                    float num8 = num6 * position.y * (num5 - num3) / num5;
                    float num9 = num7 * position.x * (num4 - num2) / num4;
                    float num10 = (float)Math.Round((double)(num7 + num9 - num2 * 0.5f));
                    float num11 = (float)Math.Round((double)(num6 + num8 - num3 * 0.5f));
                    area.Set(num10, num11, num10 + num2, num11 + num3);
                    base.mModalDialogWindow.Area = area;
                }
                Button button3 = base.mModalDialogWindow.GetChildByID(2u, false) as Button;
                if ((WindowBase)button3 != (WindowBase)null)
                {
                    button3.Click += OnButtonClick;
                    if (disableTooltip)
                    {
                        button3.TooltipText = "";
                    }
                }
                Button button4 = base.mModalDialogWindow.GetChildByID(3u, false) as Button;
                if ((WindowBase)button4 != (WindowBase)null)
                {
                    button4.Click += OnButtonClick;
                    if (disableTooltip)
                    {
                        button4.TooltipText = "";
                    }
                }
                base.OkayID = 2u;
                base.CancelID = 3u;
                base.SelectedID = 2u;
            }
        }

        public void OnButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
        {
            if (eventArgs.ButtonID == 2)
            {
                mResult = true;
                eventArgs.Handled = true;
                EndDialog(2u);
            }
            else
            {
                mResult = false;
                eventArgs.Handled = true;
                EndDialog(3u);
            }
        }

        public override void OnTriggerOk()
        {
            mResult = true;
            base.OnTriggerOk();
        }

        public override void OnTriggerCancel()
        {
            mResult = false;
            base.OnTriggerCancel();
        }

        public override void OnTriggerForward()
        {
            if (base.SelectedID == 2)
            {
                base.SelectedID = 3u;
            }
            else
            {
                base.SelectedID = 2u;
            }
        }

        public override void OnTriggerBackward()
        {
            if (base.SelectedID == 2)
            {
                base.SelectedID = 3u;
            }
            else
            {
                base.SelectedID = 2u;
            }
        }
    }

    public class RandomizerUIUtils
    {
        public static string CreateEllipsis(string original)
        {
            StringBuilder sb = new StringBuilder();
            string newString = original;

            if (original.Length > 17 )
            {
                newString = original.Substring(0, 17);
                newString += "...";
            }
            return newString;
        }
    }

}
