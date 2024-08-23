using System.ComponentModel;

namespace MD.Common.WPF;

public interface IHasProperty
{
    public event PropertyChangedEventHandler? PropertyChanged;
}