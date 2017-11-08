#region

using System;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        private int _canTpCooldownTime;
        private float _bleeding;
        private float _healing;
        private int _newbieTime;
        private int _pendantReady;

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffectIndex.Paused))
                return false;
            if (HasConditionEffect(ConditionEffectIndex.Invisible))
                return false;
            if (_newbieTime > 0)
                return false;
            return true;
        }

        private void HandleEffects(RealmTime time)
        {
            if (HasConditionEffect(ConditionEffectIndex.Healing))
            {
                if (_healing > 1)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + (int) _healing);
                    _healing -= (int) _healing;
                    UpdateCount++;
                }
                _healing += 28*(time.thisTickTimes/1000f);
            }
            if (HasConditionEffect(ConditionEffectIndex.Quiet) &&
                Mp > 0)
            {
                Mp = 0;
                UpdateCount++;
            }
            if (HasConditionEffect(ConditionEffectIndex.Bleeding) &&
                HP > 1)
            {
                if (_bleeding > 1)
                {
                    HP -= (int) _bleeding;
                    _bleeding -= (int) _bleeding;
                    UpdateCount++;
                }
                _bleeding += 28*(time.thisTickTimes/1000f);
            }

            if (_newbieTime > 0)
            {
                _newbieTime -= time.thisTickTimes;
                if (_newbieTime < 0)
                    _newbieTime = 0;
            }
            if (_canTpCooldownTime > 0)
            {
                _canTpCooldownTime -= time.thisTickTimes;
                if (_canTpCooldownTime < 0)
                    _canTpCooldownTime = 0;
            }
        }

        private bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffectIndex.Sick) || HasConditionEffect(ConditionEffectIndex.Bleeding) || OxygenBar == 0)
                return false;
            return true;
        }

        private bool CanMpRegen()
        {
            if (HasConditionEffect(ConditionEffectIndex.Quiet) || _ninjaShoot)
                return false;
            return true;
        }


        internal void SetNewbiePeriod()
        {
            _newbieTime = 3000;
        }

        internal void SetTpDisabledPeriod()
        {
            _canTpCooldownTime = 10*1000;
        }

        public bool TpCooledDown()
        {
            if (_canTpCooldownTime > 0)
                return false;
            return true;
        }
    }
}