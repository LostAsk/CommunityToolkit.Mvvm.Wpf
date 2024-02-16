namespace CommunityToolkit.Mvvm.Navigation.Regions
{
    /// <summary>
    /// Defines an interfaces to adapt an object and bind it to a new <see cref="IRegion"/>.
    /// </summary>
    public interface IRegionAdapter
    {
        /// <summary>
        /// Adapts an object and binds it to a new <see cref="IRegion"/>.
        /// </summary>
        /// <param name="regionTarget">��������ڿؼ�����</param>
        /// <param name="regionName">�����ʶ</param>
        /// <returns>The new instance of <see cref="IRegion"/> that the <paramref name="regionTarget"/> is bound to.</returns>
        IRegion Initialize(object regionTarget, string regionName);
    }
}
