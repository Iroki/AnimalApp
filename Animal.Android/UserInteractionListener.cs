using Android.Content;
using System;

namespace Animal.Droid
{// Copy-Paste
    internal class UserInteractionListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
    {
        private readonly Action _listenerAction;

        public UserInteractionListener(Action listenerAction)
        {
            _listenerAction = listenerAction;
        }

        public void OnCancel(IDialogInterface dialog)
        {
            _listenerAction?.Invoke();
        }
    }
}
