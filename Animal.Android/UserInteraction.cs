
using Android.App;
using Android.Content;
using Animal.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(Animal.Droid.UserInteraction))]

namespace Animal.Droid
{//Copy-Paste
    class UserInteraction : IUserInteraction
    {
        Context context = Xamarin.Forms.Forms.Context;

        public Task ShowMessageAsync(string message)
        {
            return AlertAsync(message, "", "OK", true);
        }

        public Task AlertAsync(string message, string title, string okButton, bool cancellable)
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.SynchronizationContext.Post(ignored => //делает диспатч сообщения на UI (основном потоке интерфейса)
            {
                if (context == null)
                    return;

                try
                {
                    new AlertDialog.Builder(context)
                                               .SetMessage(message)
                                      .SetTitle(title)
                                            .SetCancelable(cancellable)
                                               .SetOnCancelListener(new UserInteractionListener(() => tcs.TrySetResult(true)))
                                               .SetPositiveButton(okButton, (sender, e) => tcs.TrySetResult(true)) //2 лямбды не может быть подряд, поскольку она уже есть, поэтому убрали; иначе было бы "() => tcs.TrySetResult(true)"
                                      .Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }, null);
            
            return tcs.Task;
        }
    }
}