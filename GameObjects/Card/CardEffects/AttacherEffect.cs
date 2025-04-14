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
        Self,
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
            Name = $"Attach @{target}: {attachment.Name}"; // naming convention up for change
            this.target = target;
            this.attachment = attachment;
        }

        public void Perform()
        {
            switch (target)
            {
                case TargetMode.Deck:
                    Board.Instance.PlayerDeck.ApplyDeckwideEffect(attachment);
                    break;
                case TargetMode.EnemyDeck:
                    Board.Instance.CurrentEnemy.Deck.ApplyDeckwideEffect(attachment);
                    break;
                case TargetMode.NextCard:
                    Board.Instance.PlayerEffectOnPlay = attachment;
                    break;
                case TargetMode.EnemyNextCard:
                    Board.Instance.EnemyEffectOnPlay = attachment;
                    break;
                case TargetMode.Self:
                    Owner.AddEffect(attachment);
                    break;
            }
        }
    }
}
