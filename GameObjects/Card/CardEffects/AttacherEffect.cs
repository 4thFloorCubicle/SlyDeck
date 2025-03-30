using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlyDeck.GameObjects.Boards;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card.CardEffects
{
    /// <summary>
    /// Different targets for an AttacherEffect to attach to
    /// </summary>
    public enum TargetMode
    {
        Deck,
        EnemyDeck,
        NextCard,
        EnemyNextCard,
    }

    /// <summary>
    /// An effect that allows card to effect others
    /// </summary>
    internal class AttacherEffect : ICardEffect
    {
        public Card Owner { get; set; }
        public string Name { get; set; }

        private ICardEffect attachment;
        private TargetMode target;
        public TargetMode Target
        {
            get { return target; }
        }

        public ICardEffect Attachment
        {
            get { return attachment; }
        }

        /// <summary>
        /// Creates a new Attacher effect
        /// </summary>
        /// <param name="attachment">The effect to attach to the other cards.</param>
        /// <param name="target"></param>
        public AttacherEffect(ICardEffect attachment, TargetMode target)
        {
            Name = $"Attach @{target}: {attachment.Name}";
            this.target = target;
            this.attachment = attachment;
        }

        public void Perform()
        {
            switch (target)
            {
                case TargetMode.Deck:
                    foreach (Card card in Board.Instance.PlayerDeck.Cards)
                    {
                        card.AddEffect(attachment);
                    }
                    break;
                case TargetMode.EnemyDeck:
                    foreach (Card card in Board.Instance.CurrentEnemy.Deck.Cards)
                    {
                        card.AddEffect(attachment);
                    }
                    break;
                case TargetMode.NextCard:
                    Board.Instance.PlayerDeck.TopCard.AddEffect(attachment);
                    break;
                case TargetMode.EnemyNextCard:
                    Board.Instance.CurrentEnemy.Deck.TopCard.AddEffect(attachment);
                    break;
            }
        }
    }
}
