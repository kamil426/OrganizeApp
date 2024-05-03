using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace OrganizeApp.Client.Components
{
    public partial class Modal
    {
        private string modalDisplayClass = string.Empty;
        private bool showBackdrop = false;
        private string xCloseButtonDisplay = "block";

        [Parameter]
        public bool IsXCloseButtonVisible { get; set; } = true;

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string OkButtonText { get; set; } = "Ok";

        [Parameter]
        public string CancelButtonText { get; set; } = "Anuluj";

        [Parameter]
        public string YesButtonText { get; set; } = "Tak";

        [Parameter]
        public string NoButtonText { get; set; } = "Nie";

        [Parameter]
        public string HeaderColor { get; set; } = "#1e90ff;";

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public ButtonStyle ButtonStyleId { get; set; }
        public enum ButtonStyle
        {
            Ok = 0,
            Cancel,
            OkCancel,
            YesNoCancel,
            YesNo
        }

        [Parameter]
        public IconStyle IconStyleId { get; set; }
        public enum IconStyle
        {
            None = 0,
            Info,
            Question,
            Warning,
            Error
        }

        [Parameter]
        public EventCallback OnClickButtonNo { get; set; }
        private void No(MouseEventArgs e)
        {
            OnClickButtonNo.InvokeAsync();
        }

        [Parameter]
        public EventCallback OnClickButtonYes { get; set; }
        private void Yes(MouseEventArgs e)
        {
            OnClickButtonYes.InvokeAsync();
        }

        [Parameter]
        public EventCallback OnClickButtonOk { get; set; }
        private void Ok(MouseEventArgs e)
        {
            OnClickButtonOk.InvokeAsync();
        }

        [Parameter]
        public EventCallback OnClickButtonClose { get; set; }

        private void CloseModal(MouseEventArgs e)
        {
            OnClickButtonClose.InvokeAsync();
            Close();
        }

        protected override void OnParametersSet()
        {
            if (IsXCloseButtonVisible)
                xCloseButtonDisplay = "block";
            else
                xCloseButtonDisplay = "none";
            base.OnParametersSet();
        }

        public void Open()
        {
            modalDisplayClass = "modal-display";
            showBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            modalDisplayClass = string.Empty;
            showBackdrop = false;
            StateHasChanged();
        }
    }
}

