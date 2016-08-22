using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace questionnaire.answer
{
    public class ButtonSpaceBehaviour : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public string SelectedLabel { get; private set; }

        private List<Button> buttons = new List<Button>();

        public void Create(string[] labels)
        {
            float width = GetComponent<RectTransform>().rect.width;
            float inset = 0;
            float size = width/labels.Length;
            foreach (string label in labels)
            {
                buttons.Add(CreateButton(label, inset, size));
                inset += size;
                
            }
        }

        public Button CreateButton(string label, float inset, float size)
        {
            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(this.transform,false);
            button.GetComponentInChildren<Text>().text = label;
            button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, size);
            Button buttonComponent = button.GetComponent<Button>();
            button.GetComponent<Button>().onClick.AddListener(() => { ButtonSelected(label, buttonComponent); });
            return buttonComponent;
        }

        public void ButtonSelected(string buttonLabel, Button button)
        {
            foreach (Button b in buttons)
            {
                b.GetComponent<Image>().color = Color.white;
            }
            
            if (SelectedLabel == null)
            {
                button.GetComponent<Image>().color = Color.grey;
                SelectedLabel = buttonLabel;
            }
            else if (!SelectedLabel.Equals(buttonLabel))
            {
                button.GetComponent<Image>().color = Color.grey;
                SelectedLabel = buttonLabel;
            }
            else
                SelectedLabel = null;
        }

        public void Create(int likertScaleLength)
        {
            string[] labels = new string[likertScaleLength];
            for (int i=1;i<=likertScaleLength;i++)
            {
                labels[i-1] = i.ToString();
            }
            Create(labels);
        }
    }
}