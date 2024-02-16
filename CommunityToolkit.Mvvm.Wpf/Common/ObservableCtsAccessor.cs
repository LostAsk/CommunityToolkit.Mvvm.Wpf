﻿

using System.ComponentModel;
using System.Windows;


namespace CommunityToolkit.Common
{
    /// <summary>
    /// Class that wraps an object, so that other classes can notify for Change events. Typically, this class is set as
    /// a Dependency Property on DependencyObjects, and allows other classes to observe any changes in the Value.
    /// </summary>
    /// <remarks>
    /// This class is required, because in Silverlight, it's not possible to receive Change notifications for Dependency properties that you do not own.
    /// </remarks>
    /// <typeparam name="T">The type of the property that's wrapped in the Observable object</typeparam>
    public partial class ObservableCtsAccessor<T> : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Identifies the Value property of the ObservableCtsAccessor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "This is the pattern for WPF dependency properties")]
        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(T), typeof(ObservableCtsAccessor<T>), new PropertyMetadata(null, ValueChangedCallback));

        /// <summary>
        /// Event that gets invoked when the Value property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The value that's wrapped inside the ObservableCtsAccessor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ObservableCtsAccessor<T> thisInstance = ((ObservableCtsAccessor<T>)d);
            PropertyChangedEventHandler eventHandler = thisInstance.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(thisInstance, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}
