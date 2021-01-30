using System;

namespace Levels.LevelScripts
{
    public class Wave
    {
        private readonly WaveData _data;
        private float _timer = 0;

        public Wave(WaveData data)
        {
            _data = data;
            _timer = _data.when;
        }

        public bool Update(float delta)
        {
            _timer -= delta;
            return _timer <= 0;
        }

        public WaveData GetData()
        {
            return _data;
        }
    }
}