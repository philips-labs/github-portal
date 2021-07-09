using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GithubPortal.Shared
{
    public partial class Modal
    {
        [Parameter]
        public RenderFragment Header { get; set; }

        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public RenderFragment Footer { get; set; }

        [Parameter]
        public bool IsOpen { get; set; } = true;

        [Parameter]
        public string ModalClass { get; set; }

        [Parameter]
        public string HeaderClass { get; set; }

        [Parameter]
        public string BodyClass { get; set; }

        [Parameter]
        public string FooterClass { get; set; }

        public void CloseModal()
        {
            IsOpen = false;
        }

        private bool _isInside;
        private void PossiblyCloseModal()
        {
            if (!_isInside)
            {
                CloseModal();
            }
        }
        private void MouseIn()
        {
            _isInside = true;
        }

        private void MouseOut()
        {
            _isInside = false;
        }
    }
}
