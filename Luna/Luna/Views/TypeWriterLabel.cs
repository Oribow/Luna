using Luna.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Views
{
    class TypeWriterLabel : Label
    {
        public static readonly BindableProperty TextToWriteProperty = BindableProperty.Create(nameof(TextToWrite), typeof(string), typeof(TypeWriterLabel), null, propertyChanged: OnTextToTypeChanged);
        public static readonly BindableProperty TimePerCharacterProperty = BindableProperty.Create(nameof(TimePerCharacter), typeof(int), typeof(TypeWriterLabel), 50);
        public static readonly BindableProperty IsWritingProperty = BindableProperty.Create(nameof(IsWriting), typeof(bool), typeof(TypeWriterLabel), false, BindingMode.OneWayToSource);
        public static readonly BindableProperty FinishedWritingProperty = BindableProperty.Create(nameof(FinishedWriting), typeof(ICommand), typeof(TypeWriterLabel), null, BindingMode.OneWay);
        public static readonly BindableProperty IsTypingAnimationEnabledProperty = BindableProperty.Create(nameof(IsTypingAnimationEnabled), typeof(bool), typeof(TypeWriterLabel), true, BindingMode.OneWay, propertyChanged: OnIsTypingAnimationEnabledChanged);
        public static readonly BindableProperty KeepConsistentSizeProperty = BindableProperty.Create(nameof(KeepConsistentSize), typeof(bool), typeof(TypeWriterLabel), false, BindingMode.OneWay);


        static void OnTextToTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var label = (TypeWriterLabel)bindable;
            if (label.IsTypingAnimationEnabled)
            {
                label.IsWriting = true;
                label.TextTypingAnimation((string)newValue, label.TimePerCharacter).ContinueWith((task) =>
                {
                    label.IsWriting = false;
                    Device.InvokeOnMainThreadAsync(() =>
                    {
                        label.FinishedWriting?.Execute(label.Text);
                        label.OnTypingFinished?.Invoke();
                    });
                });
            }
            else
            {
                label.Text = label.TextToWrite;
            }
        }

        static void OnIsTypingAnimationEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var label = (TypeWriterLabel)bindable;
            if (!label.IsTypingAnimationEnabled)
            {
                label.AbortAnimation("TextTypingAnimation");
            }
        }

        public virtual string TextToWrite
        {
            get { return (string)GetValue(TextToWriteProperty); }
            set { SetValue(TextToWriteProperty, value); }
        }
        public int TimePerCharacter
        {
            get { return (int)GetValue(TimePerCharacterProperty); }
            set { SetValue(TimePerCharacterProperty, value); }
        }
        public bool IsWriting
        {
            get { return (bool)GetValue(IsWritingProperty); }
            set { SetValue(IsWritingProperty, value); }
        }public bool KeepConsistentSize
        {
            get { return (bool)GetValue(KeepConsistentSizeProperty); }
            set { SetValue(KeepConsistentSizeProperty, value); }
        }
        public bool IsTypingAnimationEnabled
        {
            get { return (bool)GetValue(IsTypingAnimationEnabledProperty); }
            set { SetValue(IsTypingAnimationEnabledProperty, value); }
        }
        public ICommand FinishedWriting
        {
            get { return (ICommand)GetValue(FinishedWritingProperty); }
            set { SetValue(FinishedWritingProperty, value); }
        }

        public bool FastForward()
        {
            return this.AbortAnimation(nameof(LabelExtensions.TextTypingAnimation));
        }

        public event Action OnTypingFinished;

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (KeepConsistentSize)
            {
                string truncatedText = Text;
                Text = TextToWrite;
                var m = base.OnMeasure(widthConstraint, heightConstraint);
                Text = truncatedText;

                return m;
            }
            else
            {
                return base.OnMeasure(widthConstraint, heightConstraint);
            }
        }
    }
}
