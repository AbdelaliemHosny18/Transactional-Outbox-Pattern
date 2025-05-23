﻿namespace Transactional_Outbox_Pattern_Use_Api.Data.Entities
{
    public class OutboxMessage
    {


        public OutboxMessage(Guid id, DateTime time, string type, string data)
        {
            Id = id;
            Time = time;
            Type = type;
            Data = data;
        }
        public void MarkAsProcessed(DateTime processingTime)
        {
            IsProcessed = true;
            ProcessingTime = processingTime;
        }
        public Guid Id { get; private set; }
        public DateTime Time { get; private set; }
        public string Type { get; private set; }
        public string Data { get; private set; }

        public bool IsProcessed { get; private set; }
        public DateTime? ProcessingTime { get; private set; }
    }
}
