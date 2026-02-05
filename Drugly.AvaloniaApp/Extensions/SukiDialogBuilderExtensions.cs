using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.Extensions;

public static class SukiDialogBuilderExtensions
{
    extension(SukiDialogBuilder builder)
    {
        /// <summary>
        /// Adds Support for awaiting the dialog to being closed via <see cref="SukiDialogBuilder.TryShowAsync(CancellationToken)" />
        /// </summary>
        /// <remarks>
        /// Does not add any action buttons. The result from awaiting <see cref="SukiDialogBuilder.TryShowAsync(CancellationToken)" /> will always be <see langword="false"/>.
        /// </remarks>
        public SukiDialogBuilder WithoutResult()
        {
            builder.Completion = new TaskCompletionSource<bool>();
            return builder;
        }

        /// <inheritdoc cref="SukiUI.Dialogs.FluentSukiDialogBuilder.WithYesNoResult"/>
        public SukiDialogBuilder WithColoredYesNoResult(object? yesButtonContent, object? noButtonContent)
        {
            builder.Completion = new TaskCompletionSource<bool>();

            builder.AddActionButton(yesButtonContent, _ => builder.Completion.SetResult(true), true, ["Flat"]);
            builder.AddActionButton(noButtonContent, _ => builder.Completion.SetResult(false), true, ["Danger"]);

            return builder;
        }

        /// <summary>
        /// Provides a callback that will be called when this dialog is dismissed for any reason.
        /// </summary>
        /// <param name="proc">The callback, with the dialog result as a parameter.</param>
        public SukiDialogBuilder OnClosed(Action<bool> proc)
        {
            if (builder.Completion is null)
            {
                throw new InvalidOperationException($"{nameof(SukiDialogBuilder)} is not configured properly: {nameof(SukiDialogBuilder.Completion)} was null.");
            }

            // Cannot use builder.SetOnDismissed()/builder.Dialog.OnDismissed because we need to guarantee we run last
            builder.Completion.Task.ContinueWith(task => proc(task.Result));

            return builder;
        }
    }
}