namespace Defra.Imports.PageObjects.Controls
{
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;

    /// <summary>
    /// The flip switch control, used for two option fields.
    /// </summary>
    [PcfControl("MscrmControls.FlipSwitch.FlipSwitchControl")]
    public interface IFlipSwitchControl : IYesNo
    {
    }
}
