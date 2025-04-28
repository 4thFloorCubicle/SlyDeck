using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlyDeck.GameObjects.Boards;

// Authors: Cooper Fleishman, Shane Packard
namespace SlyDeck.GameObjects.Card.CardEffects
{
    /// <summary>
    /// Different targets for an AttacherEffect to attach to
    /// </summary>
    public enum TargetMode
    {
        PlayerDeck,
        EnemyDeck,
        PlayerNextCardPlayed,
        EnemyNextCardPlayed,
        Self,
    }

    /// <summary>
    /// An effect that allows card to effect others
    /// </summary>
    internal class AttacherEffect : ICardEffect
    {
        public Card Owner { get; set; }
        public string Name { get; set; }
        public float AbilityPower { get; set; }
        public float TempAbilityPower { get; set; }

        private List<ICardEffect> attachments;
        private TargetMode target;

        public TargetMode Target
        {
            get { return target; }
        }

        public List<ICardEffect> Attachments
        {
            get { return attachments; }
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
            attachments = new List<ICardEffect>();
            attachments.Add(attachment);
        }

        public AttacherEffect(List<ICardEffect> attachments, TargetMode target)
        {
            Name = $"Attach @{target}: {attachments[0].Name}"; // naming convention up for change
            this.target = target;
            this.attachments = attachments;
        }

        public void Perform()
        {
            foreach (ICardEffect attachment in attachments)
            {
                switch (target)
                {
                    case TargetMode.PlayerDeck:
                        Board.Instance.PlayerDeck.ApplyDeckwideEffect(attachment);
                        break;
                    case TargetMode.EnemyDeck:
                        Board.Instance.CurrentEnemy.Deck.ApplyDeckwideEffect(attachment);
                        break;
                    case TargetMode.PlayerNextCardPlayed:
                        Board.Instance.PlayerEffectOnPlay = attachment;
                        break;
                    case TargetMode.EnemyNextCardPlayed:
                        Board.Instance.EnemyEffectOnPlay = attachment;
                        break;
                    case TargetMode.Self:
                        Owner.AddEffect(attachment);
                        break;
                }
            }
        }

        public void Perform(bool isOwnerPlayer)
        {
            foreach (ICardEffect attachment in attachments)
            {
                if (isOwnerPlayer)
                {
                    switch (target)
                    {
                        case TargetMode.PlayerDeck:
                            Board.Instance.PlayerDeck.ApplyDeckwideEffect(attachment);
                            break;
                        case TargetMode.EnemyDeck:
                            Board.Instance.CurrentEnemy.Deck.ApplyDeckwideEffect(attachment);
                            break;
                        case TargetMode.PlayerNextCardPlayed:
                            Board.Instance.PlayerEffectOnPlay = attachment;
                            break;
                        case TargetMode.EnemyNextCardPlayed:
                            Board.Instance.EnemyEffectOnPlay = attachment;
                            break;
                        case TargetMode.Self:
                            Owner.AddEffect(attachment);
                            break;
                    }
                }
                else
                {
                    switch (target)
                    {
                        case TargetMode.PlayerDeck:
                            Board.Instance.CurrentEnemy.Deck.ApplyDeckwideEffect(attachment);
                            break;
                        case TargetMode.EnemyDeck:
                            Board.Instance.PlayerDeck.ApplyDeckwideEffect(attachment);
                            break;
                        case TargetMode.PlayerNextCardPlayed:
                            Board.Instance.EnemyEffectOnPlay = attachment;
                            break;
                        case TargetMode.EnemyNextCardPlayed:
                            Board.Instance.PlayerEffectOnPlay = attachment;
                            break;
                        case TargetMode.Self:
                            Owner.AddEffect(attachment);
                            break;
                    }
                }
            }
        }
    }
}
