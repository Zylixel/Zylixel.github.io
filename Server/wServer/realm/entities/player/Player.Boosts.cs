namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private bool _lootDropBoostFreeTimer;
        private bool _lootTierBoostFreeTimer;
        private bool _ninjaShoot;
        private bool _ninjaFreeTimer;
        private bool _xpFreeTimer;

        public void HandleBoosts()
        {
            if (_ninjaShoot && _ninjaFreeTimer)
            {
                if (Mp > 0)
                {
                    _ninjaFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(100, (w, t) =>
                    {
                        Mp -= 1;
                        if (Mp <= 0)
                            ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Speedy, DurationMS = 0 });
                        _ninjaFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }

                if (XpBoosted && _xpFreeTimer)
            {
                if (XpBoostTimeLeft > 0)
                {
                    _xpFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        XpBoostTimeLeft -= 1;
                        if (XpBoostTimeLeft <= 0)
                            XpBoosted = false;
                        _xpFreeTimer = true;
                        UpdateCount++;
                    }));
                }
                else
                    XpBoosted = false;
            }

            if (LootDropBoost && _lootDropBoostFreeTimer)
            {
                if (LootDropBoostTimeLeft > 0)
                {
                    _lootDropBoostFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        LootDropBoostTimeLeft -= 1;
                        _lootDropBoostFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }

            if (LootTierBoost && _lootTierBoostFreeTimer)
            {
                if (LootTierBoostTimeLeft > 0)
                {
                    _lootTierBoostFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        LootTierBoostTimeLeft -= 1;
                        _lootTierBoostFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }
        }
    }
}
