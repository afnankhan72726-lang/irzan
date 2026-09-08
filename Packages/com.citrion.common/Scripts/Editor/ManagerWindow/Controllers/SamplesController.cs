using UnityEngine;
using UnityEngine.UIElements;

namespace CitrioN.Common.Editor
{
  [CreateAssetMenu(fileName = "SamplesController_",
                   menuName = "CitrioN/Common/ScriptableObjects/VisualTreeAsset/Controller/Samples")]
  public class SamplesController : ScriptableVisualTreeAssetController
  {
    public string groupName;

    public VisualTreeAsset listItemTemplate;

    public string headerLabelClass = "manager__tab__title";

    public string descriptionLabelClass = "label__description";

    public string headerText = "Samples & Demos";

    [TextArea(2, 99)]
    public string description;

    public override void Setup(VisualElement root)
    {
      if (!string.IsNullOrEmpty(groupName) && listItemTemplate != null)
      {
        new UnityPackageBundleDataList(groupName, listItemTemplate, root);
      }

      if (root == null) { return; }

      var headerLabel = root.Q<Label>(className: headerLabelClass);
      UIToolkitUtilities.SetText(headerLabel, headerText);
      var descriptionLabel = root.Q<Label>(className: descriptionLabelClass);

      if (descriptionLabel != null)
      {
        UIToolkitUtilities.SetText(descriptionLabel, description);
        if (string.IsNullOrEmpty(description))
        {
          descriptionLabel.Show(false);
        }
      }
    }
  }
}