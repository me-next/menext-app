using System;
namespace MeNext
{
    /// <summary>
    /// Represents an object which cares if the UI might need to be updated
    /// </summary>
    public interface IUIChangeListener
    {
        /// <summary>
        /// Called whenever there is the possibility of a UI update being needed
        /// </summary>
        void SomethingChanged();
    }
}
