// File: src/Systems/DialogueSystem.cs

using System.Collections.Generic;

namespace Opal.Systems
{
    public struct DialogueNode
    {
        public string Speaker;
        public string Text;
        public List<DialogueChoice> Choices;
        public string NextNodeId;

        public DialogueNode(string speaker, string text, string nextNode = null)
        {
            Speaker = speaker;
            Text = text;
            NextNodeId = nextNode;
            Choices = new List<DialogueChoice>();
        }
    }

    public struct DialogueChoice
    {
        public string Text;
        public string NextNodeId;
        public bool RequiresAugmentation;
        public float HumanityCost;

        public DialogueChoice(string text, string nextNode, bool needsAug = false, float humanityCost = 0f)
        {
            Text = text;
            NextNodeId = nextNode;
            RequiresAugmentation = needsAug;
            HumanityCost = humanityCost;
        }
    }

    public class DialogueSystem
    {
        private Dictionary<string, DialogueNode> _dialogues = new();
        private string _currentNodeId;
        private bool _isActive = false;

        public bool IsActive => _isActive;
        public DialogueNode CurrentNode => _isActive ? _dialogues[_currentNodeId] : default;

        public void LoadDialogues()
        {
            // Example dialogue fitting the cyberpunk theme
            var node1 = new DialogueNode("Eve", "The runes speak of forgotten wisdom, but the data feels... hollow.");
            node1.Choices.Add(new DialogueChoice("Search deeper in the network", "node2"));
            node1.Choices.Add(new DialogueChoice("[Augmented] Interface directly with the monument", "node3", true));
            _dialogues["start"] = node1;

            var node2 = new DialogueNode("System", "The network responds with fragmentary data streams...");
            _dialogues["node2"] = node2;

            var node3 = new DialogueNode("Eve", "The interface burns through synthetic flesh, revealing deeper truths.");
            _dialogues["node3"] = node3;
        }

        public void StartDialogue(string nodeId)
        {
            if (_dialogues.ContainsKey(nodeId))
            {
                _currentNodeId = nodeId;
                _isActive = true;
            }
        }

        public void SelectChoice(int choiceIndex)
        {
            if (!_isActive || choiceIndex < 0 || choiceIndex >= CurrentNode.Choices.Count)
                return;

            var choice = CurrentNode.Choices[choiceIndex];
            
            if (choice.NextNodeId != null && _dialogues.ContainsKey(choice.NextNodeId))
            {
                _currentNodeId = choice.NextNodeId;
            }
            else
            {
                _isActive = false;
            }
        }

        public void EndDialogue()
        {
            _isActive = false;
        }
    }
}
