﻿namespace SkincareBookingSystem.Models.Dto.DurationItem;

public class CreateDurationItemDto
{
    public Guid ServiceId { get; set; }
    public Guid ServiceDurationId { get; set; }
}