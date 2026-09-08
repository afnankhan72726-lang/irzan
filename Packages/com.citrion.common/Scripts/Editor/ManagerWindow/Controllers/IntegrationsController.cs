using UnityEngine;
using UnityEngine.UIElements;

namespace CitrioN.Common.Editor
{
  [CreateAssetMenu(fileName = "IntegrationsController",
                   menuName = "CitrioN/Common/ScriptableObjects/VisualTreeAsset/Controller/Integrations")]
  public class IntegrationsController : ScriptableVisualTreeAssetController
  {
    public string groupName;

    public VisualTreeAsset listItemTemplate;

    public VisualTreeAsset tagTemplate;

    public string headerLabelClass = "manager__tab__title";

    public string descriptionLabelClass = "label__description";

    public string headerText = "Integrations";

    [TextArea(2, 99)]
    public string description;

    protected UnityPackageBundleDataScrollViewList list;

    public override void Setup(VisualElement root)
    {
      if (!string.IsNullOrEmpty(groupName) && listItemTemplate != null)
      {
        list = new UnityPackageBundleDataScrollViewList(groupName, listItemTemplate, tagTemplate, root);
      }

      if (root == null) { return; }

      UIToolkitUtilities.SetText(root.Q<Label>(className: headerLabelClass), headerText);
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