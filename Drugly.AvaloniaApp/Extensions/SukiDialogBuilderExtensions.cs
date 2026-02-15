using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;
using SukiUI.Controls;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.Extensions;

public static class SukiDialogBuilderExtensions
{
    extension(SukiDialogBuilder builder)
    {
        /// <summary>
        /// Presents <paramref name="content"/> inside a <see cref="GroupBox"/>.
        /// </summary>
        /// <param name="content">The content to present.</param>
        /// <returns>The <see cref="SukiDialogBuilder"/> instance.</returns>
        /// <remarks><see cref="ViewModelBase"/>s will be presented with a <see cref="ContentControl"/>.</remarks>
        public SukiDialogBuilder WithGroupedContent(object content)
        {
            return builder.WithGroupedContent(null, content);
        }

        /// <summary>
        /// Presents <paramref name="content"/> and an optional <paramref name="title"/> inside a <see cref="GroupBox"/>.
        /// </summary>
        /// <param name="title">An optional title displayed above the <paramref name="content"/>.</param>
        /// <param name="content">The content to present.</param>
        /// <returns>The <see cref="SukiDialogBuilder"/> instance.</returns>
        /// <remarks><see cref="ViewModelBase"/>s will be presented with a <see cref="ContentControl"/>.</remarks>
        public SukiDialogBuilder WithGroupedContent(object? title, object content)
        {
            return builder.WithContent(new GroupBox
            {
                Header = UseContentControlIfNeeded(title),
                Content = new ScrollViewer
                {
                    Content = UseContentControlIfNeeded(content)
                }
            });

            // Allows ViewModels to be passed as content and have their respective views rendered
            static object? UseContentControlIfNeeded(object? content)
            {
                return content is ViewModelBase
                    ? new ContentControl
                    {
                        Content = content
                    }
                    : content;
            }
        }

        /// <summary>
        /// Adds Support for awaiting the dialog to being closed via <see cref="SukiDialogBuilder.TryShowAsync(CancellationToken)" />.
        /// </summary>
        /// <remarks>
        /// Does not add any action buttons. The result from awaiting <see cref="SukiDialogBuilder.TryShowAsync(CancellationToken)" /> will always be <see langword="false"/>.
        /// </remarks>
        /// <returns>The <see cref="SukiDialogBuilder"/> instance.</returns>
        public SukiDialogBuilder WithoutResult()
        {
            builder.Completion = new TaskCompletionSource<bool>();
            return builder;
        }

        /// <inheritdoc cref="SukiUI.Dialogs.FluentSukiDialogBuilder.WithYesNoResult"/>
        /// <returns>The <see cref="SukiDialogBuilder"/> instance.</returns>
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
        /// <returns>The <see cref="SukiDialogBuilder"/> instance.</returns>
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